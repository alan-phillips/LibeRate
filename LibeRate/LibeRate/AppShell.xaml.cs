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
        IFirebaseAuthentication auth;
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
