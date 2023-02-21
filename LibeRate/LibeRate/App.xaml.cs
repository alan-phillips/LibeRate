using Autofac;
using LibeRate.Helpers;
using LibeRate.Models;
using LibeRate.Resx;
using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
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
            LocalizationResourceManager.Current.CurrentCulture = CultureInfo.GetCultureInfo(Settings.SelectedLanguage);

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        protected override void OnSleep()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        protected override void OnResume()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if(e.NetworkAccess != NetworkAccess.Internet)
            {
                Shell.Current.GoToAsync($"{nameof(InternetDisconnectedPage)}");
            } else
            {
                Shell.Current.GoToAsync("..");
            }
        }
    }
}
