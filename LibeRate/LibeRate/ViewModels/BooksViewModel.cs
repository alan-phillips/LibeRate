using LibeRate.Models;
using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class BooksViewModel : BaseViewModel
    {
        IBookService bookService;
        public ObservableCollection<Book> Books { get; set; }
        private int _pageNumber;
        private bool _previousButtonVisible;
        private bool _nextButtonVisible;
        public int ItemsPerPage { get; set; }
        public Command RefreshCommand { get; }
        public Command<Book> ViewDetailCommand { get; }
        public Command NextPageCommand { get; }
        public Command PreviousPageCommand { get; }

        public BooksViewModel()
        {

            PageNumber= 1;
            ItemsPerPage = 25;
            PreviousButtonVisible = false;
            NextButtonVisible = true;
            
            RefreshCommand = new Command(Refresh);
            ViewDetailCommand = new Command<Book>(ViewDetail);
            NextPageCommand = new Command(NextPage);
            PreviousPageCommand = new Command(PreviousPage);

            bookService = DependencyService.Get<IBookService>();
            Books = new ObservableCollection<Book>();
            LoadBooks();
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
            IsBusy = true;
            Books.Clear();
            if(App.LanguageChanged==true) 
            {
                bookService.ResetService();
                PageNumber = 1;
                App.LanguageChanged = false;
            }
            LoadBooks();
            IsBusy = false;
        }

        private async void LoadBooks()
        {
            Books.Clear();
            List<Book> result = await bookService.GetBooks(App.CurrentUser.TargetLanguage, PageNumber, ItemsPerPage);
            foreach(Book book in result)
            {
                Books.Add(book);
            }
            if(result.Count < ItemsPerPage)
            {
                NextButtonVisible = false;
            } else
            {
                NextButtonVisible = true;
            }
        }


        private async void ViewDetail(Book book)
        {
            var route = $"{nameof(BookPage)}?BookId={book.Id}&Title={book.Title}&Author={book.Author}&Img={book.ImageURL}&Diff={book.DifficultyRating}";
            await Shell.Current.GoToAsync(route);
        }

        private void NextPage()
        {
            Books.Clear();
            PageNumber++;
            PreviousButtonVisible = true;
            LoadBooks();

        }

        private void PreviousPage()
        {
            Books.Clear();
            PageNumber--;
            if(PageNumber == 1)
            {
                PreviousButtonVisible= false;
            }
            NextButtonVisible = true;
            LoadBooks();

        }
    }
}
