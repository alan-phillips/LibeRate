using LibeRate.Services;
using LibeRate.Views;
using LibeRate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LibeRate.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _errorMessage;
        private bool _errorMessageVisible;
        private string _username;
        private string _password;
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, ValidateForm);
            RegisterCommand = new Command(OnRegisterClicked);
            this.PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }

        private bool ValidateForm(object arg)
        {
            return !String.IsNullOrWhiteSpace(_username)
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
                if (_password!= value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private async void OnLoginClicked(object obj)
        {
            IFirebaseAuthentication auth = DependencyService.Get<IFirebaseAuthentication>();
            string result = await auth.LoginWithEmailAndPassword(Username, Password);
            if (auth.IsSignedIn())
            {
                App.CurrentUser.Id = auth.GetUserID();
                IUserService userService = DependencyService.Get<IUserService>();
                App.CurrentUser = await userService.GetUser(App.CurrentUser.Id);
                if(App.CurrentUser.TargetLanguage == "")
                {
                    await Shell.Current.GoToAsync($"{nameof(SelectTargetLanguagePage)}");
                } else
                {
                    await Shell.Current.GoToAsync($"///{nameof(SearchPage)}");
                }
            } else
            {
                ErrorMessage = result.Substring(1);
                ErrorMessageVisible = true;
            }
        }

        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
