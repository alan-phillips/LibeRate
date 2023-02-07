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

namespace LibeRate.Droid.Services
{
    public class LibraryService : ILibraryService
    {
        public async Task AddBookToLibrary(string bookId, string language, string status)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference cr = db.Collection("users").Document(App.CurrentUser.Id).Collection(language+"-library");
            JavaDictionary<string, object> libraryBook = new JavaDictionary<string, object>
            {
                { "book", bookId },
                { "status", status }
            };
            await cr.Add(libraryBook);
        }
    }
}