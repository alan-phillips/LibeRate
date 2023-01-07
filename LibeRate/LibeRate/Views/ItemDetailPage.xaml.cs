using LibeRate.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LibeRate.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}