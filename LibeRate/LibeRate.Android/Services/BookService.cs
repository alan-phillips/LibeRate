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
    public class BookService : Java.Lang.Object, IBookService, IOnCompleteListener
    {
        List<Book> books = new List<Book>();
        Stack<DocumentSnapshot> pageBottoms = new Stack<DocumentSnapshot>();
        bool hasReadBooks = false;

        public async Task<Book> GetBook(string languageID, string ID)
        {
            books.Clear();
            hasReadBooks = false;
            FirebaseFirestore db = FirebaseFirestore.Instance;

            DocumentReference query = db.Collection(languageID + "-books")
                .Document(ID);

            query.Get(Source.Cache).AddOnCompleteListener(this);

            for (int i = 0; i < 50; i++)
            {
                await System.Threading.Tasks.Task.Delay(100);
                if (hasReadBooks) break;
            }
            return books.ElementAt(0);
        }
        public async Task<List<Book>> GetBooks(string languageID, int pageNumber, Dictionary<string, object> filterSettings, bool previous)
        {
            books.Clear();
            hasReadBooks = false;
            FirebaseFirestore db = FirebaseFirestore.Instance;

            int itemsPerPage = (int)filterSettings["items_per_page"];

            CollectionReference cr = db.Collection(languageID + "-books");

            Query query; 

            if ((string)filterSettings["search_query"] != "")
            {
                string searchQuery = (string)filterSettings["search_query"];
                query = cr.WhereGreaterThanOrEqualTo("title", searchQuery)
                    .WhereLessThanOrEqualTo("title", searchQuery+"\uf8ff");
            } else
            {
                query = cr.OrderBy("difficulty_rating")
                    .WhereGreaterThanOrEqualTo("difficulty_rating", (int)filterSettings["lower_difficulty"])
                    .WhereLessThanOrEqualTo("difficulty_rating", (int)filterSettings["higher_difficulty"]);
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
            
            query.Get().AddOnCompleteListener(this);
            
            for (int i = 0; i < 50; i++)
            {
                await System.Threading.Tasks.Task.Delay(100);
                if (hasReadBooks) break;
            }

            return books;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            var snapshot = (QuerySnapshot)task.Result;
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
            hasReadBooks = true;
        }

        public void ResetService()
        {
            books.Clear();
            pageBottoms.Clear();
        }
    }
}