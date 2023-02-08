using LibeRate.Resx;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        public Command ChangeTargetLanguageCommand { get; }
        public Command SetDisplayLanguageCommand { get; }
        public SettingsViewModel() 
        {
            ChangeTargetLanguageCommand = new Command(async () => await ChangeTargetLanguage());
            SetDisplayLanguageCommand = new Command(SetDisplayLanguage);
        }

        private async Task ChangeTargetLanguage()
        {
            await Shell.Current.GoToAsync($"//{nameof(SelectTargetLanguagePage)}");
        }

        private void SetDisplayLanguage()
        {
        }
    }
}
