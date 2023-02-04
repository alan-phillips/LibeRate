using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using LibeRate.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibeRate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectTargetLanguagePage : ContentPage
    {

        public SelectTargetLanguagePage()
        {
            InitializeComponent();
            this.BindingContext = new SelectTargetLanguageViewModel();
        }

    }
}
