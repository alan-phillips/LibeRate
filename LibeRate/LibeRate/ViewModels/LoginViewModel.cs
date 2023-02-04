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
            await auth.LoginWithEmailAndPassword(Username, Password);
            if (auth.IsSignedIn())
            {
                App.CurrentUser.Id = auth.GetUserID();
                await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
            }
        }

        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
