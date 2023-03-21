using LibeRate.Models;
using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        public List<Library> Libraries { get; set; }
        Library ReadLibrary = new Library("Read");
        Library OwnedLibrary = new Library("Owned");
        Library WishlistLibrary = new Library("Wishlist");
        public IAsyncCommand<Book> ViewDetailCommand { get; }
        ILibraryService libraryService;
       
        public LibraryViewModel() 
        {
            libraryService = DependencyService.Get<ILibraryService>();
            Libraries = new List<Library>
            {
                ReadLibrary,
                OwnedLibrary,
                WishlistLibrary
            };
            ViewDetailCommand = new AsyncCommand<Book>(book => ViewDetail(book));

            Task.Run(async () => await LoadBooks());
        }

        private async Task LoadBooks()
        {
            List<Book> readResult = await libraryService.GetLibraryBooks(CurrentUser.Instance.Id, CurrentUser.Instance.TargetLanguage, ReadLibrary.Name);

            foreach(Book book in readResult) 
            { 
                ReadLibrary.Add(book);
            }

            List<Book> ownedResult = await libraryService.GetLibraryBooks(CurrentUser.Instance.Id, CurrentUser.Instance.TargetLanguage, OwnedLibrary.Name);

            foreach (Book book in ownedResult)
            {
                OwnedLibrary.Add(book);
            }
            
            List<Book> wishlistResult = await libraryService.GetLibraryBooks(CurrentUser.Instance.Id, CurrentUser.Instance.TargetLanguage, WishlistLibrary.Name);

            foreach (Book book in wishlistResult)
            {
                WishlistLibrary.Add(book);
            }
        }

        private async Task ViewDetail(Book book)
        {
            var route = $"{nameof(BookPage)}?BookId={book.Id}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
