using LibeRate.Models;
using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private User _profile;
        public User Profile {
            get => _profile;
            set
            {
                _profile = value;
                OnPropertyChanged(nameof(Profile));
            }
        }
        private string _targetLanguage;
        public string TargetLanguage
        {
            get => _targetLanguage;
            set
            {
                _targetLanguage = value;
                OnPropertyChanged(nameof(TargetLanguage));
            }
        }
        public IAsyncCommand ViewLibraryCommand { get; }

        public ProfileViewModel() 
        {
            Profile = CurrentUser.Instance;
            TargetLanguage = Language.languageTranslations[Profile.TargetLanguage].Localized;
            CurrentUser.Instance.UserReloaded += CurrentUser_UserReloaded;

            ViewLibraryCommand = new AsyncCommand(ViewLibrary);

        }

        private async Task ViewLibrary()
        {
            await Shell.Current.GoToAsync(nameof(LibraryPage));
        }

        void CurrentUser_UserReloaded(object sender, EventArgs e)
        {
            Profile = CurrentUser.Instance;
            TargetLanguage = Language.languageTranslations[Profile.TargetLanguage].Localized;
        }
    }
}
