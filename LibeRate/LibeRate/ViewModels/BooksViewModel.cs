using LibeRate.Models;
using LibeRate.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class BooksViewModel : BaseViewModel
    {
        public ObservableCollection<Book> Books { get; set; }
        public Command RefreshCommand { get; }
        public Command ViewDetailCommand { get; }

        public BooksViewModel()
        {
            Title = "Browse";

            RefreshCommand = new Command(Refresh);
            ViewDetailCommand = new Command(ViewDetail);
            Books = new ObservableCollection<Book>();
            LoadBooks();
        }

        private async void LoadBooks()
        {
            IBookService bookService = DependencyService.Get<IBookService>();
            List<Book> books = await bookService.GetBooks("english");
            foreach(Book book in books)
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
    }
}
