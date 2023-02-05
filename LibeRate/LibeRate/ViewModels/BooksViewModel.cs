using LibeRate.Models;
using LibeRate.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public int ItemsPerPage { get; set; }
        public Command RefreshCommand { get; }
        public Command ViewDetailCommand { get; }
        public Command NextPageCommand { get; }
        public Command PreviousPageCommand { get; }

        public BooksViewModel()
        {
            Title = "Browse";

            PageNumber= 1;
            ItemsPerPage = 25;
            PreviousButtonVisible = false;

            RefreshCommand = new Command(Refresh);
            ViewDetailCommand = new Command(ViewDetail);
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

        public bool PreviousButtonVisible
        {
            get => _previousButtonVisible;
            set
            {
                _previousButtonVisible = value;
                OnPropertyChanged(nameof(PreviousButtonVisible));
            }
        }

        private async void LoadBooks()
        {
            Books.Clear();
            List<Book> result = await bookService.GetBooks("english", PageNumber, ItemsPerPage);
            foreach(Book book in result)
            {
                Books.Add(book);
            }
        }

        private async void Refresh()
        {
        }

        private async void ViewDetail()
        {

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
            LoadBooks();

        }
    }
}
