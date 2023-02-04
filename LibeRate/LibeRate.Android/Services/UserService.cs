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

namespace LibeRate.Droid.Services
{
    public class UserService : Java.Lang.Object, IUserService, IOnCompleteListener
    {
        User user = new User(); 
        bool hasReadUser = false;
        public async Task<User> GetUser(string userId)
        {
            user.Id= userId;
            FirebaseFirestore db = FirebaseFirestore.Instance;
            db.Collection("users").Document(userId).Get().AddOnCompleteListener(this);
            for (var i = 0; i < 25; i++)
            {
                if (hasReadUser == true) break;
                await System.Threading.Tasks.Task.Delay(100);
            }
            return null;
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
            JavaDictionary<string, object> data = new JavaDictionary<string, object>
            {
                { "target_language", languageid }
            };
            await db.Collection("users").Document(App.CurrentUser.Id).Set(data, SetOptions.Merge());
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            var snapshot = (DocumentSnapshot)task.Result;
            if (snapshot.Exists())
            {
                user.Username = snapshot.Get("username").ToString();
                user.TargetLanguage = snapshot.Get("target_language").ToString();
                user.AccountCreated = DateTime.Parse(snapshot.Get("date_created").ToString());

                hasReadUser = true;
            }
        }
    }
}