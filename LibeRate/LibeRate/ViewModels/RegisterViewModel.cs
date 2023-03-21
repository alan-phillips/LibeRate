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
        private string _errorMessage;
        private bool _errorMessageVisible;
        private string _email;
        private string _username;
        private string _password;
        public Command RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked, ValidateForm);
            this.PropertyChanged +=
                (_, __) => RegisterCommand.ChangeCanExecute();
        }

        private bool ValidateForm(object arg)
        {
            return !String.IsNullOrWhiteSpace(_email)
                && !String.IsNullOrWhiteSpace(_username)
                && !String.IsNullOrWhiteSpace(_password);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public bool ErrorMessageVisible
        {
            get => _errorMessageVisible;
            set
            {
                if (_errorMessageVisible != value)
                {
                    _errorMessageVisible = value;
                    OnPropertyChanged(nameof(ErrorMessageVisible));
                }
            }
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


        private async void OnRegisterClicked(object obj)
        {
            IFirebaseAuthentication auth = DependencyService.Get<IFirebaseAuthentication>();
            string result = await auth.RegisterWithEmailAndPassword(Email, Password);
            if (!result.StartsWith("!"))
            {
                await auth.LoginWithEmailAndPassword(Email, Password);
                if (auth.IsSignedIn())
                {
                    IUserService userService = DependencyService.Get<IUserService>();
                    await userService.CreateUserProfile(auth.GetUserID(), Username);
                    await Shell.Current.GoToAsync($"{nameof(SelectTargetLanguagePage)}");
                }
            } else
            {
                ErrorMessage = result.Substring(1);
                ErrorMessageVisible = true;
            }
        }
    }
}
