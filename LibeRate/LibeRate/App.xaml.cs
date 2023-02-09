using Autofac;
using LibeRate.Models;
using LibeRate.Resx;
using LibeRate.Services;
using LibeRate.Views;
using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibeRate
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public static bool LanguageChanged { get; set; }
        public App()
        {
            CurrentUser= new User();


            InitializeComponent();

            LocalizationResourceManager.Current.PropertyChanged += (sender, e) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

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
