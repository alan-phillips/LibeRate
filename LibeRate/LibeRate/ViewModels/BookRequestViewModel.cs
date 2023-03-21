using LibeRate.Models;
using LibeRate.Services;
using LibeRate.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class BookRequestViewModel : BaseViewModel
    {
        public List<string> Difficulties;
        private string selectedDifficulty;
        public string SelectedDifficulty { get => selectedDifficulty; set { selectedDifficulty = value; OnPropertyChanged(nameof(SelectedDifficulty)); } }

        public AsyncCommand SubmitFormCommand { get; }

        public BookRequestViewModel() 
        {
            Difficulties = new List<string>
            {
                { "0 (Beginner)" },
                { "5 (Beginner)" },
                { "10 (Beginner)" },
                { "15 (Novice)" },
                { "20 (Novice)" },
                { "25 (Intermediate)" },
                { "30 (Intermediate)" },
                { "35 (Advanced)" },
                { "40 (Advanced)" },
                { "45 (Advanced+)" }
            };
            SubmitFormCommand = new AsyncCommand(SubmitForm, ValidateForm);
            this.PropertyChanged +=
                (_, __) => SubmitFormCommand.ChangeCanExecute();
        }

        private bool ValidateForm(object arg)
        {
            return !String.IsNullOrWhiteSpace(bookUrl)
                && (bookUrl.Contains("amazon.com")
                || bookUrl.Contains("amazon.co.uk")
                || bookUrl.Contains("amazon.co.jp")
                || bookUrl.Contains("amazon.de"))
                && SelectedDifficulty!= null;
        }

        private string bookUrl;
        public string BookUrl
        {
            get => bookUrl;
            set
            {
                if (bookUrl != value)
                {
                    bookUrl = value;
                    OnPropertyChanged(nameof(BookUrl));
                }
            }
        }

        private async Task SubmitForm()
        {
            IBookService bs = DependencyService.Get<IBookService>();

            int difficulty = Int32.Parse(new string(SelectedDifficulty.Trim().TakeWhile(c => char.IsDigit(c)).ToArray()));

            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "amazon_url", BookUrl },
                { "estimated_difficulty", difficulty },
                { "request_user", CurrentUser.Instance.Id }
            };

            await bs.CreateBookRequest(CurrentUser.Instance.TargetLanguage, data);
            await Shell.Current.GoToAsync($"///{nameof(SearchPage)}");
        }
    }
}
