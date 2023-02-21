using LibeRate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibeRate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookRequestPage : ContentPage
    {
        public BookRequestPage()
        {
            InitializeComponent();
            this.BindingContext = new BookRequestViewModel();
            pick.ItemsSource = ((BookRequestViewModel)this.BindingContext).Difficulties;
        }
    }
}