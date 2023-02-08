using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using LibeRate.Models;
using LibeRate.Services;
using System.Web;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LibeRate.ViewModels
{ 
    public class BookViewModel : BaseViewModel, IQueryAttributable
    {
        private Book displayBook;
        private readonly IDialogService _dialogService;
        IBookService bookService;
        public Command AddToLibraryCommand { get; }
        public BookViewModel()
        {

            bookService = DependencyService.Get<IBookService>();
            DisplayBook = new Book();
            _dialogService = new DialogService();   
            AddToLibraryCommand= new Command(async () => await AddToLibrary());
        }

        public Book DisplayBook { get { return displayBook; } set { displayBook = value; OnPropertyChanged(nameof(DisplayBook)); } }


        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            Book selectedBook = new Book(HttpUtility.UrlDecode(query["BookId"]),
                HttpUtility.UrlDecode(query["Title"]),
                HttpUtility.UrlDecode(query["Author"]),
                HttpUtility.UrlDecode(query["Img"]),
                int.Parse(HttpUtility.UrlDecode(query["Diff"])));

            DisplayBook= selectedBook;
        }


        private async Task AddToLibrary()
        {
            string[] strings = { "Wishlist", "Owned", "Read" };
            string selection = await _dialogService.ShowActionSheetAsync(null, "Cancel", null, strings);
            if (selection != "Cancel")
            {
                ILibraryService libraryService = DependencyService.Get<ILibraryService>();
                await libraryService.AddBookToLibrary(displayBook.Id, App.CurrentUser.TargetLanguage, selection);
            }
        }
    }
}

