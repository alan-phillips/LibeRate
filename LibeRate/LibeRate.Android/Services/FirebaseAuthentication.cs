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
using Android.Gms.Extensions;

namespace LibeRate.Droid.Services
{
    public class FirebaseAuthentication : IFirebaseAuthentication
    {
        public bool IsSignedIn()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password);
                var token = await FirebaseAuth.Instance.CurrentUser.GetIdToken(true).AsAsync<GetTokenResult>();

                return token.Token;
            }
            catch (FirebaseAuthWeakPasswordException e) //thrown if password bad
            {
                e.PrintStackTrace();
                return "!Your password must be at least 6 characters.";
            }
            catch (FirebaseAuthInvalidCredentialsException e) //thrown if email is malformed
            {
                e.PrintStackTrace();
                return "!The email address you entered is invalid.";
            }
            catch (FirebaseAuthUserCollisionException e) //thrown if account with email exists
            {
                e.PrintStackTrace();
                return "!An account with this email already exists.";
            } 
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await FirebaseAuth.Instance.CurrentUser.GetIdToken(true).AsAsync<GetTokenResult>()  ;

                return token.Token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return "!An account with the specified email does not exist.";
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return "!The entered username or password is invalid.";
            }
        }

        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetUserID()
        {
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }
    }
}