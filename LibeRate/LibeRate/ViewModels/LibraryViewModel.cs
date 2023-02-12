using LibeRate.Models;
using LibeRate.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        public List<Library> Libraries { get; set; }
        Library ReadLibrary = new Library("Read");
        Library OwnedLibrary = new Library("Owned");
        Library WishlistLibrary = new Library("Wishlist");
        IBookService bookService;
        public LibraryViewModel() 
        {
            bookService = DependencyService.Get<IBookService>();
            Libraries = new List<Library>
            {
                ReadLibrary,
                OwnedLibrary,
                WishlistLibrary
            };

            Task.Run(async () => await LoadBooks());
        }

        private async Task LoadBooks()
        {
            List<Book> readResult = await bookService.GetLibraryBooks(App.CurrentUser.Id, App.CurrentUser.TargetLanguage, ReadLibrary.Name);

            foreach(Book book in readResult) 
            { 
                ReadLibrary.Add(book);
            }

            List<Book> ownedResult = await bookService.GetLibraryBooks(App.CurrentUser.Id, App.CurrentUser.TargetLanguage, OwnedLibrary.Name);

            foreach (Book book in ownedResult)
            {
                OwnedLibrary.Add(book);
            }
            
            List<Book> wishlistResult = await bookService.GetLibraryBooks(App.CurrentUser.Id, App.CurrentUser.TargetLanguage, WishlistLibrary.Name);

            foreach (Book book in wishlistResult)
            {
                WishlistLibrary.Add(book);
            }
        }
    }
}
