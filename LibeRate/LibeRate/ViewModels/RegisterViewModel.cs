using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _email;
        private string _username;
        private string _password;
        public Command RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await OnRegisterClicked());
        }

        private bool ValidateForm(object arg)
        {
            return  !String.IsNullOrWhiteSpace(_username)
                && !String.IsNullOrWhiteSpace(_password);
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }


        private async Task OnRegisterClicked()
        {
            IFirebaseAuthentication auth = DependencyService.Get<IFirebaseAuthentication>();
            string result = await auth.RegisterWithEmailAndPassword(Email, Password);
            if (result != string.Empty)
            {
                await auth.LoginWithEmailAndPassword(Email, Password);
                if (auth.IsSignedIn())
                {
                    IUserService userService = DependencyService.Get<IUserService>();
                    App.CurrentUser.Id = auth.GetUserID();
                    await userService.CreateUserProfile(App.CurrentUser.Id, Username);
                    await Shell.Current.GoToAsync($"//{nameof(SelectTargetLanguagePage)}");
                }
            }
        }
    }
}
