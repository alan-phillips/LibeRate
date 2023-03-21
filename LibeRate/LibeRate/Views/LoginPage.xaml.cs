using LibeRate.Models;
using LibeRate.Services;
using LibeRate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibeRate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.GoToAsync($"{nameof(InternetDisconnectedPage)}");
            } else
            {
                IFirebaseAuthentication auth;
                auth = DependencyService.Get<IFirebaseAuthentication>();
                if (auth != null)
                {
                    if (auth.IsSignedIn())
                    {
                        await CurrentUser.LoadUser(auth.GetUserID());
                        if (CurrentUser.Instance.TargetLanguage == "")
                        {
                            await Shell.Current.GoToAsync($"{nameof(SelectTargetLanguagePage)}");
                        }
                        else
                        {
                            await Shell.Current.GoToAsync($"///{nameof(SearchPage)}");
                        }
                    }

                }
            }
        }

    }
}