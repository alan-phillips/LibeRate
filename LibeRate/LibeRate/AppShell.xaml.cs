using LibeRate.Services;
using LibeRate.ViewModels;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LibeRate
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(BookPage), typeof(BookPage));
            Routing.RegisterRoute(nameof(LibraryPage), typeof(LibraryPage));

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            IFirebaseAuthentication auth = DependencyService.Get<IFirebaseAuthentication>();
            if (auth.SignOut())
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
