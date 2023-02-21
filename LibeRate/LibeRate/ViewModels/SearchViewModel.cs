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
        public IAsyncCommand OpenRequestFormCommand { get; }

        public SearchViewModel()
        {
            OpenRequestFormCommand= new AsyncCommand(OpenRequestForm);  
        }

        public ICommand PerformSearchCommand => new AsyncCommand<string>(async (string query) =>
        {
            var route = $"///{nameof(BooksPage)}?query={query}";
            await Shell.Current.GoToAsync(route);
        });

        private async Task OpenRequestForm()
        {
            await Shell.Current.GoToAsync($"{nameof(BookRequestPage)}");
        }
    }
}
