using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface IFirebaseAuthentication
    {
        Task<string> RegisterWithEmailAndPassword(string email, string password);
        Task<string> LoginWithEmailAndPassword(string email, string password);
        bool SignOut();
        bool IsSignedIn();
    }
}
