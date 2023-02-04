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
        bool hasReadBooks = false;
        public async Task<List<Book>> GetBooks(string languageID)
        {
            hasReadBooks = false;
            FirebaseFirestore db = FirebaseFirestore.Instance;
            Query query = db.Collection(languageID + "-books").OrderBy("difficulty_rating");
            query.Get().AddOnCompleteListener(this);
            
            for (int i = 0; i < 25; i++)
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
                books.Clear();
                foreach(DocumentSnapshot doc in documents)
                {
                    Book b = new Book(doc.Get("title").ToString(), 
                        doc.Get("author").ToString(), 
                        doc.Get("cover_image").ToString(), 
                        ((int)doc.Get("difficulty_rating")));
                    books.Add(b);
                }
                
            }
            hasReadBooks = true;
        }
    }
}