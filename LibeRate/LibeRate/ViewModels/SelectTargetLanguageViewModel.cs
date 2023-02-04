using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using LibeRate.Models;
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;
using LibeRate.Services;
using LibeRate.Views;

namespace LibeRate.ViewModels
{
    class SelectTargetLanguageViewModel : BaseViewModel
    {
        public ObservableCollection<LanguageMenuItem> Languages { get; set; }
        private LanguageMenuItem _selectedLanguage;
        public Command ConfirmSelectionCommand { get;}

        public SelectTargetLanguageViewModel()
        {
            Title = "Select Target Language";
            Languages = new ObservableCollection<LanguageMenuItem>();
            Languages.Add(new LanguageMenuItem("english", "English", ""));
            ConfirmSelectionCommand= new Command(ConfirmSelection);
        }

        public LanguageMenuItem SelectedLanguage 
        { 
            get => _selectedLanguage;
            set 
            { 
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }

        public async void ConfirmSelection()
        {
            IUserService userService = DependencyService.Get<IUserService>();

            await userService.SetTargetLanguage(SelectedLanguage.LanguageID);

            App.CurrentUser = await userService.GetUser(App.CurrentUser.Id);
            await Shell.Current.Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
        }

    }
}
