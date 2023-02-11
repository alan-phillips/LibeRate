using LibeRate.Models;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public SearchViewModel()
        {
        }

        public ICommand PerformSearchCommand => new AsyncCommand<string>(async (string query) =>
        {
            var route = $"///{nameof(BooksPage)}?query={query}";
            await Shell.Current.GoToAsync(route);
        });
    }
}
