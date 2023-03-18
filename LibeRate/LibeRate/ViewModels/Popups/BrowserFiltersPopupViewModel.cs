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
        
        public List<int> ItemsPerPage { get; set; }
        private int selectedItemsPerPage;
        public int SelectedItemsPerPage { get => selectedItemsPerPage; set { selectedItemsPerPage = value; OnPropertyChanged(nameof(SelectedItemsPerPage)); } }
        
        private int lowerDifficulty;
        public int LowerDifficulty { get => lowerDifficulty; set { lowerDifficulty = value; OnPropertyChanged(nameof(LowerDifficulty)); } }
        
        private int higherDifficulty;
        public int HigherDifficulty { get => higherDifficulty; set { higherDifficulty = value; OnPropertyChanged(nameof(HigherDifficulty)); } }
        
        public Command SubmitFormCommand { get; }
        
        public BrowserFiltersPopupViewModel(Dictionary<string, object> CurrentFilterSettings)
        {
            Filters = new List<string>
            {
                "Difficulty (Ascending)",
                "Difficulty (Descending)",
                "Popularity"
            };
            ItemsPerPage = new List<int>
            {
                5,
                10,
                25
            };
            
            SelectedFilter = (string)CurrentFilterSettings["filter"];
            SelectedItemsPerPage = (int)CurrentFilterSettings["items_per_page"];
            LowerDifficulty = (int)CurrentFilterSettings["lower_difficulty"];
            HigherDifficulty = (int)CurrentFilterSettings["higher_difficulty"];

            SubmitFormCommand= new Command(SubmitForm); 
        }
        
        private void SubmitForm()
        {
            Results.Add("search_query", "");
            Results.Add("items_per_page", SelectedItemsPerPage);
            Results.Add("filter", SelectedFilter);
            Results.Add("lower_difficulty", LowerDifficulty);
            Results.Add("higher_difficulty", HigherDifficulty);
        }
    }
}
