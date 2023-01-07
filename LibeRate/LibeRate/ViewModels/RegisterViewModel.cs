using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        public Command RegisterCommand { get; }

        public RegisterViewModel()
        {
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
            await auth.RegisterWithEmailAndPassword(Username, Password);
            if (auth.IsSignedIn())
            {
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
        }
    }
}
