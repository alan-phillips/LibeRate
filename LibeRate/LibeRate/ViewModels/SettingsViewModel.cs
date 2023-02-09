using LibeRate.Resx;
using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        Dictionary<string, string> languages = new Dictionary<string, string>
            {
                { "English", "en" },
                { "日本語", "ja" }
            };
        string[] languageNames = { "English", "日本語" };
        public Command ChangeTargetLanguageCommand { get; }
        public Command SetDisplayLanguageCommand { get; }
        private readonly IDialogService _dialogService;
        public SettingsViewModel() 
        {
            _dialogService = new DialogService();
            ChangeTargetLanguageCommand = new Command(async () => await ChangeTargetLanguage());
            SetDisplayLanguageCommand = new Command(async () => await SetDisplayLanguage());
        }

        private async Task ChangeTargetLanguage()
        {
            await Shell.Current.GoToAsync($"//{nameof(SelectTargetLanguagePage)}");
        }

        private async Task SetDisplayLanguage()
        {
            string selection = await _dialogService.ShowActionSheetAsync(null, "Cancel", null, languageNames);
            if (selection != "Cancel")
            {
                LocalizationResourceManager.Current.CurrentCulture = CultureInfo.GetCultureInfo(languages[selection]);
            }
        }
    }
}
