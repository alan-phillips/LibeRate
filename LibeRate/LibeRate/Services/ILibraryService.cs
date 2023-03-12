using LibeRate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface ILibraryService
    {
        Task AddBookToLibrary(string userId, string bookId, string language, string status);
        Task<List<Book>> GetLibraryBooks(string userId, string language, string status);
        Task<List<Grading>> GetGradings(string userId, string language);
        Task CompleteGrading(string userId, string gradingId, string language, string status);
    }
}
