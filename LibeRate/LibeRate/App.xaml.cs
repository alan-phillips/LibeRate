using LibeRate.Models;
using LibeRate.Services;
using LibeRate.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibeRate
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
