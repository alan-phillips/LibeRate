using Acr.UserDialogs;
using LibeRate.Models;
using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class BooksViewModel : BaseViewModel, IQueryAttributable
    {
        IBookService bookService;
        public ObservableCollection<Book> Books { get; set; }
        private Dictionary<string, object> filterSettings;
        private int _pageNumber;
        private bool _previousButtonVisible;
        private bool _nextButtonVisible;
        public ICommand RefreshCommand { get; }
        public IAsyncCommand<Book> ViewDetailCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand OpenFiltersMenuCommand { get; }

        public BooksViewModel()
        {
            filterSettings = new Dictionary<string, object>
            {
                { "search_query", "" },
                { "items_per_page", 5 },
                { "filter", "Popularity" },
                { "lower_difficulty", 0 },
                { "higher_difficulty", 50 }
            };

            PageNumber = 1;
            PreviousButtonVisible = false;
            NextButtonVisible = true;
            
            RefreshCommand = new Command(Refresh);
            ViewDetailCommand = new AsyncCommand<Book>(book => ViewDetail(book));
            NextPageCommand = new Command(NextPage);
            PreviousPageCommand = new Command(PreviousPage);
            OpenFiltersMenuCommand = new Command(async () => await OpenFiltersMenu());

            CurrentUser.Instance.UserReloaded += CurrentUser_UserReloaded;

            bookService = DependencyService.Get<IBookService>();
            Books = new ObservableCollection<Book>();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if(query.Count > 0) 
            {
                string searchQuery = HttpUtility.UrlDecode(query["query"]);
                filterSettings["search_query"] = searchQuery;
                PageNumber = 1;
                PreviousButtonVisible = false;
            } else
            {
                filterSettings["search_query"] = "";
            }
            Task.Run(async () => await LoadBooks(false, true));
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                _pageNumber = value;
                OnPropertyChanged(nameof(PageNumber));
            }
        }

        public bool NextButtonVisible
        {
            get=> _nextButtonVisible;
            set
            {
                _nextButtonVisible = value;
                OnPropertyChanged(nameof(NextButtonVisible));
            }
        }

        public bool PreviousButtonVisible
        {
            get => _previousButtonVisible;
            set
            {
                _previousButtonVisible = value;
                OnPropertyChanged(nameof(PreviousButtonVisible));
            }
        }

        public void Refresh()
        {
            Books.Clear();
            Task.Run(async () => await LoadBooks(false, true));
        }

        private async Task LoadBooks(bool previous, bool reloaded)
        {
            if(IsBusy) { return; }

            IsBusy= true;
            Books.Clear();

            List<Book> result = await bookService.GetBooks(CurrentUser.Instance.TargetLanguage, PageNumber, filterSettings, previous, reloaded);
            
            foreach(Book book in result)
            {
                Books.Add(book);
            }
            
            if(result.Count < (int)filterSettings["items_per_page"])
            {
                NextButtonVisible = false;
            } else
            {
                NextButtonVisible = true;
            }
            
            IsBusy= false;
        }


        private async Task ViewDetail(Book book)
        {
            var route = $"{nameof(BookPage)}?BookId={book.Id}";
            await Shell.Current.GoToAsync(route);
        }

        private void NextPage()
        {
            if(IsBusy) { return; }
            Books.Clear();
            PageNumber++;
            PreviousButtonVisible = true;
            Task.Run(async () => await LoadBooks(false, false));

        }

        private void PreviousPage()
        {
            if (IsBusy) { return; }
            Books.Clear();
            PageNumber--;
            if(PageNumber == 1)
            {
                PreviousButtonVisible= false;
            }
            NextButtonVisible = true;
            Task.Run(async () => await LoadBooks(true, false));
        }

        private async Task OpenFiltersMenu()
        {
            var popup = new Views.Popups.BrowserFiltersPopup(filterSettings);
            var popupResult = await Shell.Current.Navigation.ShowPopupAsync(popup);
            if(popupResult != null)
            {
                filterSettings = (Dictionary<string, object>)popupResult;
                PageNumber= 1;
                PreviousButtonVisible = false;
                bookService.ResetService();
                Refresh();
            }
            
        }

        void CurrentUser_UserReloaded(object sender, EventArgs e)
        {
            filterSettings = new Dictionary<string, object>
            {
                { "search_query", "" },
                { "items_per_page", 5 },
                { "filter", "Popularity" },
                { "lower_difficulty", 0 },
                { "higher_difficulty", 50 }
            };

            bookService.ResetService();
            PageNumber = 1;
        }

    }
}
