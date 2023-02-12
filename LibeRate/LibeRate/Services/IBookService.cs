using LibeRate.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface IBookService
    {
        Task<Book> GetBook(string languageID, string ID);
        Task<List<Book>> GetBooks(string languageID, int pageNumber, Dictionary<string, object> filterSettings, bool previous);
        Task<List<Book>> GetLibraryBooks(string userID, string languageID, string status);
        void ResetService();
    }
}
