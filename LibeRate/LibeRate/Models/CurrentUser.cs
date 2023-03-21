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
        private static CurrentUser _instance = new CurrentUser();
        public static CurrentUser Instance
        {
            get { return _instance; }
        }

        public static async Task LoadUser(string id)
        {
            IUserService userService = DependencyService.Get<IUserService>();
            User u = await userService.GetUser(id);
            if (u != null)
            {
                _instance.Id = u.Id;
                _instance.AccountCreated = u.AccountCreated;
                _instance.Username = u.Username;
                _instance.TargetLanguage = u.TargetLanguage;    
                _instance.CanGradeBooks = u.CanGradeBooks;

                Instance.UserReloaded?.Invoke(Instance, EventArgs.Empty);
            }
        }

        public event EventHandler UserReloaded;
    }
}
