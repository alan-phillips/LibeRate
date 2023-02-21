using LibeRate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface ILibraryService
    {
        Task AddBookToLibrary(string bookId, string language, string status);
        Task<List<Book>> GetLibraryBooks(string language, string status);
        Task<List<Grading>> GetGradings(string language);
        Task CompleteGrading(string gradingId, string language, string status);
    }
}
