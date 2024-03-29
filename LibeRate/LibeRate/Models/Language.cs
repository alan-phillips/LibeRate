﻿using LibeRate.Resx;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.Helpers;

namespace LibeRate.Models
{
    public static class Language
    {
        public static readonly Dictionary<string, LocalizedString> languageTranslations = new Dictionary<string, LocalizedString>()
        {
            { "english", new LocalizedString(() => string.Format(AppResources.English)) },
            { "french", new LocalizedString(() => string.Format(AppResources.French)) },
            { "spanish", new LocalizedString(() => string.Format(AppResources.Spanish)) },
            { "german", new LocalizedString(() => string.Format(AppResources.German)) },
            { "japanese", new LocalizedString(() => string.Format(AppResources.Japanese)) }
        };

        public static readonly Dictionary<string, LocalizedString> libraryTranslations = new Dictionary<string, LocalizedString>()
        {
            { "read", new LocalizedString(() => string.Format(AppResources.LibraryRead)) },
            { "owned", new LocalizedString(() => string.Format(AppResources.LibraryOwned)) },
            { "wishlist", new LocalizedString(() => string.Format(AppResources.LibraryWishlist)) }
        };

    }
}
