using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibeRate.Services
{
    public interface IDialogService
    {
        Task<string> ShowActionSheetAsync(string title, string button1, string button2, string[] items);
    }
}
