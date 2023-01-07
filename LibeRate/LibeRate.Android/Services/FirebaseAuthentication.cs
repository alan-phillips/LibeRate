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
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> RegisterWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await Firebase.Auth.FirebaseAuth.Instance.CreateUserWithEmailAndPassword(email, password);
                var token = await (Firebase.Auth.FirebaseAuth.Instance.CurrentUser.GetIdToken(true).AsAsync<GetTokenResult>());

                return token.Token;
            } 
            catch (FirebaseAuthInvalidCredentialsException e) //thrown if email is malformed
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthUserCollisionException e) //thrown if account with email exists
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await (Firebase.Auth.FirebaseAuth.Instance.CurrentUser.GetIdToken(true).AsAsync<GetTokenResult>());

                return token.Token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}