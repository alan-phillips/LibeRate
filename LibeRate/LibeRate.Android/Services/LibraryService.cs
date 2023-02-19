using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibeRate.Services;
using System.Threading.Tasks;
using Firebase.Firestore;
using Android.Gms.Extensions;
using LibeRate.Models;

namespace LibeRate.Droid.Services
{
    public class LibraryService : ILibraryService
    {
        public async Task AddBookToLibrary(string bookId, string language, string status)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language+"-library");

            var result = await library.Document("library-data").Get().ToAwaitableTask();
            JavaDictionary<string, string> books = ConvertFirestoreResultToDictionary(result);

            if(!books.ContainsKey(bookId))
            {
                await db.Collection(language + "-books").Document(bookId).Update("user_count", FieldValue.Increment(1));
                library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");
                await library.Document("library-data").Update("books." + bookId, status);
                await UpdateCounts(language);
                if (status == "Read")
                {
                    await CreateGradings(bookId, language);
                }
            }//Don't perform update if selected status is the same
            else if (books[bookId] != status)
            {
                library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");
                await library.Document("library-data").Update("books." + bookId, status);
                await UpdateCounts(language);
                if (status == "Read")
                {
                    await CreateGradings(bookId, language);
                }
            }
            
        }

        public async Task<List<Book>> GetLibraryBooks(string language, string status)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");

            var result = await library.Document("library-data").Get().ToAwaitableTask();
            JavaDictionary<string, string> books = ConvertFirestoreResultToDictionary(result);
            
            List<string> bookIds= new List<string>();
            foreach(var book in books)
            {
                if(book.Value == status)
                {
                    bookIds.Add(book.Key);
                }
            }

            BookService bs = new BookService();
            List<Book> resultBooks = await bs.GetBooksFromList(language, bookIds);
            return resultBooks;
        }

        public async Task<List<Grading>> GetGradings(string language)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference cr = db.Collection("users")
                    .Document(App.CurrentUser.Id)
                    .Collection(language + "-library")
                    .Document("library-data")
                    .Collection("gradings");

            var result = await cr.Get().ToAwaitableTask();
            List<Grading> gradings = await ConvertFirestoreResultToGradingList(result);

            return gradings;
        }

        public async Task CompleteGrading(string gradingId, string language, string status)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference cr = db.Collection("users")
                    .Document(App.CurrentUser.Id)
                    .Collection(language + "-library")
                    .Document("library-data")
                    .Collection("gradings");

            JavaDictionary<string, object> data = new JavaDictionary<string, object>
            {
                { "status", status }
            };

            await cr.Document(gradingId).Set(data, SetOptions.Merge());

            DocumentReference dr = db.Collection("users")
                    .Document(App.CurrentUser.Id)
                    .Collection(language + "-library")
                    .Document("library-data")
                    .Collection("gradings")
                    .Document("grading-data");
            await dr.Update("available_gradings", FieldValue.Increment(-1));
            dr = db.Collection("users")
                    .Document(App.CurrentUser.Id)
                    .Collection(language + "-library")
                    .Document("library-data")
                    .Collection("gradings")
                    .Document("grading-data");
            await dr.Update("completed_gradings", FieldValue.Increment(1));
        }

        private async Task UpdateCounts(string language)
        {
            await Task.Delay(2000);
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");

            var result = await library.Document("library-data").Get().ToAwaitableTask();
            JavaDictionary<string, string> books = ConvertFirestoreResultToDictionary(result);
            
            long wishlistCount = 0;
            long ownedCount = 0;
            long readCount = 0;

            foreach(var book in books) 
            { 
                switch (book.Value)
                {
                    case "Wishlist":
                        wishlistCount++;
                        break;
                    case "Owned":
                        ownedCount++;
                        break;
                    case "Read":
                        readCount++;
                        break;
                }
                    
            }
            JavaDictionary<string, object> libraryData = new JavaDictionary<string, object>
            {
                { "wishlist_count", wishlistCount },
                { "owned_count", ownedCount },
                { "read_count", readCount }
            };
            await library.Document("library-data").Set(libraryData, SetOptions.Merge());
        }

        private async Task CreateGradings(string bookId, string language)
        {
            await Task.Delay(2000);
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");

            var result = await library.Document("library-data").Get().ToAwaitableTask();
            JavaDictionary<string, string> books = ConvertFirestoreResultToDictionary(result);
            JavaDictionary<string, int> counts = ConvertFirestoreResultToIntDictionary(result);

            if (counts["read_count"] >= 5)
            {
                BookService bs = new BookService();
                Book addedBook = await bs.GetBook(language, bookId);
                int newGradingCount = 0;

                foreach(var book in books)
                {
                    if(book.Value == "Read")
                    {
                        Book compareBook = await bs.GetBook(language, book.Key);
                        var x = addedBook.DifficultyRating - compareBook.DifficultyRating;
                        if(x >= -5 || x <= 5)
                        {
                            await CreateGrading(addedBook.Id, compareBook.Id, language);
                            newGradingCount++;
                        }
                    }
                }

                if(newGradingCount > 0)
                {
                    CollectionReference gradings = db.Collection("users")
                    .Document(App.CurrentUser.Id)
                    .Collection(language + "-library")
                    .Document("library-data")
                    .Collection("gradings");

                    await gradings.Document("grading-data").Update("available_gradings", FieldValue.Increment(newGradingCount));
                }
            }
        }

        private async Task CreateGrading(string book1, string book2, string language)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference gradings = db.Collection("users")
                .Document(App.CurrentUser.Id)
                .Collection(language + "-library")
                .Document("library-data")
                .Collection("gradings");

            JavaDictionary<string, object> data = new JavaDictionary<string, object>
            {
                {"book1", book1 },
                {"book2", book2 },
                {"status", "ungraded" }
            };

            await gradings.Add(data);
        }

        private async Task<List<Grading>> ConvertFirestoreResultToGradingList(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            List<Grading> gradings = new List<Grading>();
            BookService bs = new BookService();
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                foreach (DocumentSnapshot doc in documents)
                {
                    if(doc.Id != "grading-data")
                    {
                        if(doc.Get("status").ToString() == "ungraded")
                        {
                            string id = doc.Id.ToString();  
                            string id1 = doc.Get("book1").ToString();
                            string id2 = doc.Get("book2").ToString();

                            Book book1 = await bs.GetBook(App.CurrentUser.TargetLanguage, id1);
                            Book book2 = await bs.GetBook(App.CurrentUser.TargetLanguage, id2);

                            Grading g = new Grading(id, book1, book2);
                            gradings.Add(g);
                        }
                    }
                }
            }
            return gradings;
        }

        private JavaDictionary<string, string> ConvertFirestoreResultToDictionary(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            JavaDictionary<string, string> dict = new JavaDictionary<string, string>();  
            if(snapshot.Exists())
            {
                dict = snapshot.Get("books").JavaCast<JavaDictionary<string, string>>();
            }
            return dict;
        }

        private JavaDictionary<string, int> ConvertFirestoreResultToIntDictionary(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            JavaDictionary<string, int> dict = new JavaDictionary<string, int>();
            if(snapshot.Exists())
            {
                dict.Add("wishlist_count", (int)snapshot.Get("wishlist_count"));
                dict.Add("owned_count", (int)snapshot.Get("owned_count"));
                dict.Add("read_count", (int)snapshot.Get("read_count"));
            }
            return dict;
        }
    }
}