using LibeRate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooks(string languageID);
    }
}
