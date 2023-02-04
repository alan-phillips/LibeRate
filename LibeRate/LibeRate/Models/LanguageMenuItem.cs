using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.Models
{
    public class LanguageMenuItem
    {
        public string LanguageID { get; set; }
        public string LanguageName { get; set; }
        public string ImageURL { get; set; }
        public LanguageMenuItem(string id, string language, string image) 
        {
            LanguageID = id;
            LanguageName = language;
            ImageURL = image;
        }
    }
}
