using LibeRate.Services;
using LibeRate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IFirebaseAuthentication auth;
            auth = DependencyService.Get<IFirebaseAuthentication>();
            if (auth != null)
            {
                if(auth.IsSignedIn())
                {
                    App.CurrentUser.Id = auth.GetUserID();
                    await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
                }

            }
        }

    }
}