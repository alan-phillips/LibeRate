using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace LibeRate.Helpers
{
    public class DifficultyColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int difficulty = (int)value;
            Color result;

            if(difficulty < 11)
            {
                //Green
                result = Color.FromHex("00CC00");
            }
            else if (difficulty < 21)
            {
                result = Color.Blue;
            }
            else if (difficulty < 31)
            {
                //Orange
                result = Color.FromHex("FF8000");
            }
            else if (difficulty < 41)
            {
                result = Color.Maroon;
            } 
            else
            {
                result = Color.Purple;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
