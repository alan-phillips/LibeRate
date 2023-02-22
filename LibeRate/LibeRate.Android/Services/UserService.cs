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
using Firebase.Auth;
using LibeRate.Services;
using LibeRate.Models;
using Firebase.Firestore;
using Android.Gms.Extensions;
using Java.Util;
using Java.Interop;
using Android.Gms.Tasks;
using Task = System.Threading.Tasks.Task;
using AndroidX.Core.Util;
using IntelliJ.Lang.Annotations;

namespace LibeRate.Droid.Services
{
    public class UserService : IUserService
    {
        public async Task<User> GetUser(string userId)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;

            var result = await db.Collection("users").Document(userId).Get().ToAwaitableTask();
            User user = ConvertFirestoreResultToUser(result);

            CollectionReference gradings = db.Collection("users")
            .Document(user.Id)
                .Collection(user.TargetLanguage + "-library")
                .Document("library-data")
                .Collection("gradings");

            var gradingData = await gradings.Document("grading-data").Get().ToAwaitableTask();
            int availableGradings = ConvertFirestoreResultToInt(gradingData);
            user.CanGradeBooks = (availableGradings >= 1);

            return user;
        }

        public async Task CreateUserProfile(string username)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            DocumentReference dr = db.Collection("users").Document(App.CurrentUser.Id);
            JavaDictionary<string, object> user = new JavaDictionary<string, object>
            {
                { "username", username },
                { "date_created", DateTime.Now.ToString() }
            };
            await dr.Set(user);
        }

        public async Task SetTargetLanguage(string languageid)
        {
            FirebaseFirestore db = FirebaseFirestore.Instance;
            var result = await db.Collection("users")
                .Document(App.CurrentUser.Id)
                .Collection(languageid+"-library")
                .Document("library-data")
                .Get().ToAwaitableTask();
            var snapshot = (DocumentSnapshot) result;
            
            if (!snapshot.Exists()) 
            {
                //create library and grading metadata if doesn't exist
                JavaDictionary<string, object> libData = new JavaDictionary<string, object>
                {
                    { "owned_count", 0 },
                    { "read_count", 0 },
                    { "wishlist_count", 0 }
                };
                JavaDictionary<string, object> lib = new JavaDictionary<string, object>
                {
                    { "", "" }
                };
                
                libData.Add("books", lib);
                await db.Collection("users")
                    .Document(App.CurrentUser.Id)
                    .Collection(languageid + "-library")
                    .Document("library-data")
                    .Set(libData);

                JavaDictionary<string, object> grad = new JavaDictionary<string, object>
                {
                    { "available_gradings", 0 },
                    { "completed_gradings", 0 }
                };
                await db.Collection("users")
                    .Document(App.CurrentUser.Id)
                    .Collection(languageid + "-library")
                    .Document("library-data")
                    .Collection("gradings")
                    .Document("grading-data")
                    .Set(grad);
            }

            JavaDictionary<string, object> data = new JavaDictionary<string, object>
            {
                { "target_language", languageid }
            };
            await db.Collection("users").Document(App.CurrentUser.Id).Set(data, SetOptions.Merge());
        }

        private User ConvertFirestoreResultToUser(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            User user = new User();

            if (snapshot.Exists())
            {
                user.Id = snapshot.Id;
                user.Username = snapshot.Get("username").ToString();
                user.TargetLanguage = snapshot.Get("target_language").ToString();
            }

            return user;
        }

        private int ConvertFirestoreResultToInt(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            int resultInt = 0;
            if (snapshot.Exists())
            {
                resultInt = (int)snapshot.Get("available_gradings");
            }
            return resultInt;
        }
    }
}