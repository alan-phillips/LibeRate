using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibeRate.ViewModels.Popups
{
    public class BrowserFiltersPopupViewModel : BaseViewModel
    {
        public Dictionary<string, object> Results= new Dictionary<string, object>();
        public List<string> Filters { get; set; }
        private string selectedFilter;
        public string SelectedFilter { get => selectedFilter; set { selectedFilter = value; OnPropertyChanged(nameof(SelectedFilter)); } }
        private int lowerDifficulty;
        public int LowerDifficulty { get => lowerDifficulty; set { lowerDifficulty = value; OnPropertyChanged(nameof(LowerDifficulty)); } }
        private int higherDifficulty;
        public int HigherDifficulty { get => higherDifficulty; set { higherDifficulty = value; OnPropertyChanged(nameof(HigherDifficulty)); } }
        public Command SubmitFormCommand { get; }
        public BrowserFiltersPopupViewModel() 
        {
            Filters = new List<string>
            {
                "Difficulty (Ascending)",
                "Difficulty (Descending)",
                "Popularity"
            };
            SelectedFilter= Filters[0];
            LowerDifficulty = 0;
            HigherDifficulty = 50;

            SubmitFormCommand= new Command(SubmitForm); 
        }
        
        private void SubmitForm()
        {
            Results.Add("filter", SelectedFilter);
            Results.Add("lower_difficulty", LowerDifficulty);
            Results.Add("higher_difficulty", HigherDifficulty);
        }
    }
}
