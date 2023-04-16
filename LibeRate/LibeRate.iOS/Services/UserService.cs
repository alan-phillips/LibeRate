using Firebase.CloudFirestore;
using Foundation;
using LibeRate.Models;
using LibeRate.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace LibeRate.iOS.Services
{
    public class UserService : IUserService
    {
        public async Task CreateUserProfile(string userId, string username)
        {
            Firestore db = Firestore.SharedInstance;
            DocumentReference dr = db.GetCollection("users").GetDocument(userId);

            var keys = new string[] { "username", "target_language", "date_created" };
            var values = new object[] { username, "", DateTime.Now.ToString() };
            var data = NSDictionary.FromObjectsAndKeys(values, keys);

            await dr.SetDataAsync((NSDictionary<NSString, NSObject>)data);
        }

        public async Task<User> GetUser(string userId)
        {
            Firestore db = Firestore.SharedInstance;

            var result = await db.GetCollection("users").GetDocument(userId).GetDocumentAsync();
            User user = ConvertFirestoreResultToUser(result);

            CollectionReference gradings = db.GetCollection("users")
            .GetDocument(user.Id)
                .GetCollection(user.TargetLanguage + "-library")
                .GetDocument("library-data")
                .GetCollection("gradings");

            var gradingData = await gradings.GetDocument("grading-data").GetDocumentAsync();
            int availableGradings = ConvertFirestoreResultToInt(gradingData);
            user.CanGradeBooks = (availableGradings >= 1);

            return user;
        }

        public async Task SetTargetLanguage(string userId, string languageid)
        {
            Firestore db = Firestore.SharedInstance;
            var result = await db.GetCollection("users")
               .GetDocument(userId)
               .GetCollection(languageid + "-library")
               .GetDocument("library-data")
               .GetDocumentAsync();
            
            if(!result.Exists)
            {
                var keys = new string[] { "owned_count", "read_count", "wishlist_count", "books" };
                var values = new object[] { 0, 0, 0, new Dictionary<string, string>{ { "base", "null" } } };
                var data = NSDictionary.FromObjectsAndKeys(values, keys);

                await db.GetCollection("users")
                    .GetDocument(userId)
                    .GetCollection(languageid + "-library")
                    .GetDocument("library-data")
                    .SetDataAsync((NSDictionary<NSString, NSObject>)data);

                var keys2 = new string[] { "available_gradings", "completed_gradings" };
                var values2 = new object[] { 0, 0 };
                var data2 = NSDictionary.FromObjectsAndKeys(values2, keys2);

                await db.GetCollection("users")
                    .GetDocument(userId)
                    .GetCollection(languageid + "-library")
                    .GetDocument("library-data")
                    .GetCollection("gradings")
                    .GetDocument("grading-data")
                    .SetDataAsync((NSDictionary<NSString, NSObject>)data2);
            }

            var keys3 = new string[] { "target_language" };
            var values3 = new object[] { languageid };
            var data3 = NSDictionary.FromObjectsAndKeys(values3, keys3);

            await db.GetCollection("users").GetDocument(userId).SetDataAsync((NSDictionary<NSString, NSObject>)data3, true);
        }

        private User ConvertFirestoreResultToUser(DocumentSnapshot result)
        {
            var snapshot = result;
            User user = new User();

            if (snapshot.Exists)
            {
                var data = snapshot.Data;
                user.Id = snapshot.Id;
                user.Username = data["username"].ToString();
                user.TargetLanguage = data["target_language"].ToString();
                user.AccountCreated = DateTime.Parse(data["date_created"].ToString());
            }

            return user;
        }

        private int ConvertFirestoreResultToInt(DocumentSnapshot result)
        {
            var snapshot = result;
            int resultInt = 0;
            if (snapshot.Exists)
            {
                var data = snapshot.Data;
                resultInt = (data["available_gradings"] as NSNumber).Int32Value; 
            }
            return resultInt;
        }
    }
}