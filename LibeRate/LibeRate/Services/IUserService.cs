using LibeRate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string userId);
        Task CreateUserProfile(string username);
        Task SetTargetLanguage(string languageid);
    }
}
