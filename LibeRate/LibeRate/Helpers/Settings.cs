﻿using Plugin.Settings.Abstractions;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using LibeRate.Resx;
using System.Globalization;

namespace LibeRate.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        private const string selectedLanguageKey = "selected_language";

        private static readonly string defaultLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        
        public static string SelectedLanguage
        {
            get
            {
                return AppSettings.GetValueOrDefault(selectedLanguageKey, defaultLanguage);
            }
            set
            {
                AppSettings.AddOrUpdateValue(selectedLanguageKey, value);
            }
        }
}
}
