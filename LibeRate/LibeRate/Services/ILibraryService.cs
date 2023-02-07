using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface ILibraryService
    {
        Task AddBookToLibrary(string bookId, string language, string status);
    }
}
