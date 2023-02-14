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
            }

            library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");
            await library.Document("library-data").Update("books."+bookId, status);
            UpdateCounts(language);
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
    }
}