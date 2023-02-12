using LibeRate.Models;
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
        public User Profile { get; set; }
        public IAsyncCommand ViewLibraryCommand { get; }
        public ProfileViewModel() 
        {
            Profile = App.CurrentUser;

            ViewLibraryCommand = new AsyncCommand(ViewLibrary);
        }

        private async Task ViewLibrary()
        {
            await Shell.Current.GoToAsync(nameof(LibraryPage));
        }
    }
}
