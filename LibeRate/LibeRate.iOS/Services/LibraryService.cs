using Firebase.CloudFirestore;
using Foundation;
using LibeRate.Models;
using LibeRate.Services;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace LibeRate.iOS.Services
{
    public class LibraryService : ILibraryService
    {
        public async Task AddBookToLibrary(string userId, string bookId, string language, string status)
        {
            Firestore db = Firestore.SharedInstance;
            CollectionReference library = db.GetCollection("users").GetDocument(userId).GetCollection(language + "-library");

            var result = await library.GetDocument("library-data").GetDocumentAsync();
            Dictionary<string, string> books = ConvertFirestoreResultToDictionary(result);

            if (!books.ContainsKey(bookId))
            {
                await db.GetCollection(language + "-books").GetDocument(bookId).UpdateDataAsync("user_count", FieldValue.FromIntegerIncrement(1));
                library = db.GetCollection("users").GetDocument(userId).GetCollection(language + "-library");
                await library.GetDocument("library-data").UpdateDataAsync("books." + bookId, status);
                await UpdateCounts(userId, language);
                if (status == "Read")
                {
                    await CreateGradings(userId, bookId, language);
                }
            }//Don't perform update if selected status is the same
            else if (books[bookId] != status)
            {
                library = db.GetCollection("users").GetDocument(userId).GetCollection(language + "-library");
                await library.GetDocument("library-data").UpdateDataAsync("books." + bookId, status);
                await UpdateCounts(userId, language);
                if (status == "Read")
                {
                    await CreateGradings(userId, bookId, language);
                }
            }
        }

        public Task CompleteGrading(string userId, string gradingId, string language, string status)
        {
            throw new NotImplementedException();
        }

        public Task<List<Grading>> GetGradings(string userId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<List<Book>> GetLibraryBooks(string userId, string language, string status)
        {
            throw new NotImplementedException();
        }

        private async Task UpdateCounts(string userId, string language)
        {
            await Task.Delay(2000);
            Firestore db = Firestore.SharedInstance;
            CollectionReference library = db.GetCollection("users").GetDocument(userId).GetCollection(language + "-library");

            var result = await library.GetDocument("library-data").GetDocumentAsync();
            Dictionary<string, string> books = ConvertFirestoreResultToDictionary(result);

            long wishlistCount = 0;
            long ownedCount = 0;
            long readCount = 0;

            foreach (var book in books)
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

            var keys = new string[] { "wishlist_count", "owned_count", "read_count" };
            var values = new object[] { wishlistCount, ownedCount, readCount };
            var data = NSDictionary.FromObjectsAndKeys(values, keys);

            await library.GetDocument("library-data").SetDataAsync((NSDictionary<NSString, NSObject>)data, true);
        }

        private async Task CreateGradings(string userId, string bookId, string language)
        {
            await Task.Delay(2000);
            Firestore db = Firestore.SharedInstance;
            CollectionReference library = db.GetCollection("users").GetDocument(userId).GetCollection(language + "-library");

            var result = await library.GetDocument("library-data").GetDocumentAsync();
            Dictionary<string, string> books = ConvertFirestoreResultToDictionary(result);
            Dictionary<string, int> counts = ConvertFirestoreResultToIntDictionary(result);

            if (counts["read_count"] >= 2)
            {
                BookService bs = new BookService();
                Book addedBook = await bs.GetBook(language, bookId);
                int newGradingCount = 0;

                foreach (var book in books)
                {
                    if (book.Value == "Read" && book.Key != addedBook.Id)
                    {
                        Book compareBook = await bs.GetBook(language, book.Key);
                        var x = addedBook.DifficultyRating - compareBook.DifficultyRating;
                        if (x >= -5 && x <= 5)
                        {
                            await CreateGrading(userId, addedBook.Id, compareBook.Id, language);
                            newGradingCount++;
                        }
                    }
                }

                if (newGradingCount > 0)
                {
                    CollectionReference gradings = db.GetCollection("users")
                    .GetDocument(userId)
                    .GetCollection(language + "-library")
                    .GetDocument("library-data")
                    .GetCollection("gradings");

                    var keys = new string[] { "available_gradings" };
                    var values = new object[] { newGradingCount+1 };
                    var data = NSDictionary.FromObjectsAndKeys(values, keys);

                    await gradings.GetDocument("grading-data").UpdateDataAsync((NSDictionary<NSObject, NSObject>)data);
                    await CurrentUser.LoadUser(userId);
                }
            }
        }

        private async Task CreateGrading(string userId, string book1, string book2, string language)
        {
            Firestore db = Firestore.SharedInstance;
            CollectionReference gradings = db.GetCollection("users")
                .GetDocument(userId)
                .GetCollection(language + "-library")
                .GetDocument("library-data")
                .GetCollection("gradings");

            var keys = new string[] { "book1", "book2", "status" };
            var values = new object[] { book1, book2, "ungraded" };
            var data = NSDictionary.FromObjectsAndKeys(values, keys);

            gradings.AddDocument((NSDictionary<NSString, NSObject>)data);
        }

        private async Task<List<Grading>> ConvertFirestoreResultToGradingList(string language, QuerySnapshot result)
        {
            var snapshot = result;
            List<Grading> gradings = new List<Grading>();
            BookService bs = new BookService();
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                foreach (DocumentSnapshot doc in documents)
                {
                    if (doc.Id != "grading-data")
                    {
                        var data = doc.Data;
                        if (data["status"].ToString() == "ungraded")
                        {
                            string id = doc.Id.ToString();
                            string id1 = data["book1"].ToString();
                            string id2 = data["book2"].ToString();

                            Book book1 = await bs.GetBook(language, id1);
                            Book book2 = await bs.GetBook(language, id2);

                            Grading g = new Grading(id, book1, book2);
                            gradings.Add(g);
                        }
                    }
                }
            }
            return gradings;
        }

        private Dictionary<string, string> ConvertFirestoreResultToDictionary(DocumentSnapshot result)
        {
            var snapshot = result;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (snapshot.Exists)
            {
                var data = snapshot.Data;
                var dict2 = (data["books"] as NSDictionary);
            }
            return dict;
        }

        private Dictionary<string, int> ConvertFirestoreResultToIntDictionary(DocumentSnapshot result)
        {
            var snapshot = (DocumentSnapshot)result;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            if (snapshot.Exists)
            {
                var data = snapshot.Data;

                dict.Add("wishlist_count", (data["wishlist_count"] as NSNumber).Int32Value);
                dict.Add("owned_count", (data["owned_count"] as NSNumber).Int32Value);
                dict.Add("read_count", (data["read_count"] as NSNumber).Int32Value);
            }
            return dict;
        }
    }
}