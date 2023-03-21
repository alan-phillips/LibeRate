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
            string bookId = HttpUtility.UrlDecode(query["BookId"]);

            Task.Run(async () => await LoadBook(bookId) );
        }

        private async Task LoadBook(string bookId)
        {
            DisplayBook = await bookService.GetBook(CurrentUser.Instance.TargetLanguage, bookId);
        }

        private async Task AddToLibrary()
        {
            string[] strings = { "Wishlist", "Owned", "Read" };
            string selection = await _dialogService.ShowActionSheetAsync(null, "Cancel", null, strings);
            if (selection != "Cancel")
            {
                ILibraryService libraryService = DependencyService.Get<ILibraryService>();
                await libraryService.AddBookToLibrary(CurrentUser.Instance.Id, displayBook.Id, CurrentUser.Instance.TargetLanguage, selection);
            }
        }
    }
}

