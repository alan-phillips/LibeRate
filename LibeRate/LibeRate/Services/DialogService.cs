using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibeRate.Services
{
    public class DialogService : IDialogService
    {
        public async Task<string> ShowActionSheetAsync(string title, string button1, string button2, string[] items)
        {
                string result = await UserDialogs.Instance.ActionSheetAsync(title, button1, button2, null, items);
                return result;
        }
    }
}
