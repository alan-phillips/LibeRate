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
            CollectionReference library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language+"-library");
            JavaDictionary<string, object> libraryBook = new JavaDictionary<string, object>
            {
                { "status", status }
            };
            await library.Document(bookId).Set(libraryBook);
            UpdateCounts(language);
        }

        private async Task UpdateCounts(string language)
        {
            await Task.Delay(2000);
            FirebaseFirestore db = FirebaseFirestore.Instance;
            CollectionReference library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");

            Query query = library.WhereEqualTo("status", "Wishlist");
            AggregateQuery aggQuery = query.Count();
            AggregateQuerySnapshot result = (AggregateQuerySnapshot) await aggQuery.Get(AggregateSource.Server);
            long wishlistCount = result.Count;

            library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");

            Query query2 = library.WhereEqualTo("status", "Owned");
            AggregateQuery aggQuery2 = query2.Count();
            AggregateQuerySnapshot result2 = (AggregateQuerySnapshot)await aggQuery2.Get(AggregateSource.Server);
            long ownedCount = result2.Count;

            library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");

            Query query3 = library.WhereEqualTo("status", "Read");
            AggregateQuery aggQuery3 = query3.Count();
            AggregateQuerySnapshot result3 = (AggregateQuerySnapshot)await aggQuery3.Get(AggregateSource.Server);
            long readCount = result3.Count;

            library = db.Collection("users").Document(App.CurrentUser.Id).Collection(language + "-library");
            JavaDictionary<string, object> libraryData = new JavaDictionary<string, object>
            {
                { "wishlist_count", wishlistCount },
                { "owned_count", ownedCount },
                { "read_count", readCount }
            };
            library.Document("library-data").Set(libraryData);
        }
    }
}