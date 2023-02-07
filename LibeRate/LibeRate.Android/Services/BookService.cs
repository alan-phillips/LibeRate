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
        List<DocumentSnapshot> pageBottoms = new List<DocumentSnapshot>();
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
        public async Task<List<Book>> GetBooks(string languageID, int pageNumber, int itemsPerPage)
        {
            books.Clear();
            hasReadBooks = false;
            FirebaseFirestore db = FirebaseFirestore.Instance;

            Query query;
            if (pageNumber== 1)
            {
                query = db.Collection(languageID + "-books")
                    .OrderBy("difficulty_rating")
                    .Limit(itemsPerPage);
            } else
            {
                query = db.Collection(languageID + "-books")
                    .OrderBy("difficulty_rating")
                    .StartAfter(pageBottoms.ElementAt(pageNumber-2))
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
                    pageBottoms.Add(documents.Last());  
                }
                hasReadBooks= true;
            }
        }
    }
}