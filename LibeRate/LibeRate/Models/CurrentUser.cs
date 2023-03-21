using LibeRate.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibeRate.Models
{
    public class CurrentUser : User
    {
        private CurrentUser()
        {
            IFirebaseAuthentication auth = DependencyService.Get<IFirebaseAuthentication>();
            if (auth.IsSignedIn())
            {
                Task.Run(async () => await LoadUser(auth.GetUserID()));
            }
        }

        private static CurrentUser _instance = new CurrentUser();
        public static CurrentUser Instance
        {
            get { return _instance; }
        }

        private async Task LoadUser(string id)
        {
            IUserService userService = DependencyService.Get<IUserService>();
            _instance = (CurrentUser)await userService.GetUser(id);
        }
    }
}
