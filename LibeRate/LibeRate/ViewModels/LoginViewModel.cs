using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
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
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }
    }
}
