using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LibeRate.Services;
using LibeRate.Models;
using Firebase.Firestore;
using Android.Gms.Extensions;
using Java.Util;
using Java.Interop;
using Firebase.Annotations;
using Android.Gms.Tasks;

namespace LibeRate.Droid.Services
{
    public class BookService : IBookService
    {
        Stack<DocumentSnapshot> pageBottoms = new Stack<DocumentSnapshot>();

        public async Task<Book> GetBook(string languageID, string ID)
        {
            return null;
        }

        public async Task<List<Book>> GetBooks(string languageID, int pageNumber, Dictionary<string, object> filterSettings, bool previous)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;

            int itemsPerPage = (int)filterSettings["items_per_page"];
            string searchQuery = (string)filterSettings["search_query"];
            int lowerDifficulty = (int)filterSettings["lower_difficulty"];
            int higherDifficulty = (int)filterSettings["higher_difficulty"];

            CollectionReference cr = db.Collection(languageID + "-books");

            Query query; 

            if (searchQuery != "")
            {
                query = cr.WhereGreaterThanOrEqualTo("title", searchQuery)
                    .WhereLessThanOrEqualTo("title", searchQuery+"\uf8ff");
            } else
            {
                query = cr.OrderBy("difficulty_rating")
                    .WhereGreaterThanOrEqualTo("difficulty_rating", lowerDifficulty)
                    .WhereLessThanOrEqualTo("difficulty_rating", higherDifficulty);
            }

            if (pageNumber== 1)
            {
                pageBottoms.Clear();
                query = query.Limit(itemsPerPage);
            } else
            {
                if (previous) 
                { 
                    pageBottoms.Pop();
                    pageBottoms.Pop();
                }
                query = query.StartAfter(pageBottoms.Peek())
                    .Limit(itemsPerPage);
            }
            
            var result = await query.Get().ToAwaitableTask();
            List<Book> books = ConvertFirestoreResultToBookList(result);

            return books;
        }

        private List<Book> ConvertFirestoreResultToBookList(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            List<Book> books = new List<Book>();
            if(!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                foreach(DocumentSnapshot doc in documents)
                {
                    Book b = new Book(doc.Id,
                        doc.Get("title").ToString(), 
                        doc.Get("author").ToString(), 
                        doc.Get("cover_image").ToString(), 
                        ((int)doc.Get("difficulty_rating")));
                    books.Add(b);
                }
                if(!pageBottoms.Contains(documents.Last())) 
                {
                    pageBottoms.Push(documents.Last());  
                }
            } else
            {
                pageBottoms.Push(null);
            }
            return books;
        }

        public void ResetService()
        {
            pageBottoms.Clear();
        }
    }
}