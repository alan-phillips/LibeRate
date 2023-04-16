using Foundation;
using LibeRate.Models;
using LibeRate.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Firebase.CloudFirestore;

namespace LibeRate.iOS.Services
{
    public class BookService : IBookService
    {
        Stack<DocumentSnapshot> pageBottoms = new Stack<DocumentSnapshot>();

        public async Task CreateBookRequest(string languageID, Dictionary<string, object> requestData)
        {
            Firestore db = Firestore.SharedInstance;

            var keys = new string[] { "amazon_url", "estimated_difficulty", "request_user_id", "request_user_name", "request_status", "date_requested" };
            var values = new object[] { requestData["amazon_url"],
            requestData["estimated_difficulty"],
            requestData["request_user_id"],
            requestData["request_user_name"],
            "unfulfilled",
            DateTime.Now.ToString() };
             
            var data = NSDictionary.FromObjectsAndKeys(values, keys);


            db.GetCollection(languageID + "-book-requests").AddDocument((NSDictionary<NSString, NSObject>)data);
        }

        public async Task<Book> GetBook(string languageID, string ID)
        {
            Firestore db = Firestore.SharedInstance;
            DocumentReference dr = db.GetCollection(languageID + "-books").GetDocument(ID);
            
            var result = await dr.GetDocumentAsync();
            Book book = ConvertFirestoreResultToBook(result);

            return book;
        }

        public async Task<List<Book>> GetBooks(string languageID, int pageNumber, Dictionary<string, object> filterSettings, bool previous, bool reloaded)
        {
            Firestore db = Firestore.SharedInstance;

            int itemsPerPage = (int)filterSettings["items_per_page"];
            string searchQuery = (string)filterSettings["search_query"];
            searchQuery = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                ToTitleCase(searchQuery.ToLower());
            string filter = (string)filterSettings["filter"];
            int lowerDifficulty = (int)filterSettings["lower_difficulty"];
            int higherDifficulty = (int)filterSettings["higher_difficulty"];

            Query query = db.GetCollection(languageID + "-books");

            if (searchQuery != "")
            {
                query = query.WhereGreaterThanOrEqualsTo("title", searchQuery)
                    .WhereLessThanOrEqualsTo("title", searchQuery + "\uf8ff");
            }
            else
            {
                switch (filter)
                {
                    case "Popularity":
                        query = query.OrderedBy("user_count", true);
                        break;

                    case "Difficulty (Ascending)":
                        query = query.OrderedBy("difficulty_rating", false)
                    .WhereGreaterThanOrEqualsTo("difficulty_rating", lowerDifficulty)
                    .WhereLessThanOrEqualsTo("difficulty_rating", higherDifficulty);
                        break;

                    case "Difficulty (Descending)":
                        query = query.OrderedBy("difficulty_rating", true)
                    .WhereGreaterThanOrEqualsTo("difficulty_rating", lowerDifficulty)
                    .WhereLessThanOrEqualsTo("difficulty_rating", higherDifficulty);
                        break;
                }

            }

            if (pageNumber == 1)
            {
                pageBottoms.Clear();
                query = query.LimitedTo(itemsPerPage);
            }
            else
            {
                if (previous)
                {
                    pageBottoms.Pop();
                    pageBottoms.Pop();
                }
                if (reloaded)
                {
                    pageBottoms.Pop();
                }
                query = query.StartingAfter(pageBottoms.Peek())
                    .LimitedTo(itemsPerPage);
            }

            var result = await query.GetDocumentsAsync();
            List<Book> books = ConvertFirestoreResultToBookList(result);

            return books;
        }

        public async Task<List<Book>> GetBooksFromList(string languageID, List<string> bookIds)
        {
            List<Book> books = new List<Book>();

            foreach (string bookId in bookIds)
            {
                Book b = await GetBook(languageID, bookId);
                books.Add(b);
            }

            return books;
        }

        public void ResetService()
        {
            pageBottoms.Clear();
        }

        public async Task SetDifficultyRating(string languageID, string bookID, float difficulty)
        {
            Firestore db = Firestore.SharedInstance;
            var keys = new string[] { "difficulty_rating" };
            var values = new object[] { difficulty };
            var data = NSDictionary.FromObjectsAndKeys(values, keys);

            await db.GetCollection(languageID + "-books").GetDocument(bookID).SetDataAsync((NSDictionary<NSString, NSObject>)data, true);
        }

        private Book ConvertFirestoreResultToBook(DocumentSnapshot result)
        {
            var snapshot = result;
            Book book = new Book();
            if (snapshot.Exists)
            {
                var data = snapshot.Data;
                book.Id = snapshot.Id;
                book.Title = data["title"].ToString();
                book.Author = data["author"].ToString();
                book.ImageURL = data["cover_image"].ToString();
                book.DifficultyRating = (data["difficulty_rating"] as NSNumber).FloatValue;
                book.UserCount = (data["user_count"] as NSNumber).Int32Value;
            }
            return book;
        }

        private List<Book> ConvertFirestoreResultToBookList(QuerySnapshot result)
        {
            var snapshot = result;
            List<Book> books = new List<Book>();
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                foreach (DocumentSnapshot doc in documents)
                {
                    Book b = ConvertFirestoreResultToBook(doc);
                    books.Add(b);
                }
                if (!pageBottoms.Contains(documents.Last()))
                {
                    pageBottoms.Push(documents.Last());
                }
            }
            else
            {
                pageBottoms.Push(null);
            }
            return books;
        }
    }
}