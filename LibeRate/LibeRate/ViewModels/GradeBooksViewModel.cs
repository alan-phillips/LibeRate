using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LibeRate.Models;
using LibeRate.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace LibeRate.ViewModels
{
    public class GradeBooksViewModel : BaseViewModel
    {
        ILibraryService libraryService;
        public ObservableCollection<Grading> Gradings { get; set; }

        public IAsyncCommand<string> CompleteGradingCommand { get; }

        public GradeBooksViewModel() 
        { 
            libraryService = DependencyService.Get<ILibraryService>();
            Position= 0;

            CompleteGradingCommand = new AsyncCommand<string>(result => CompleteGrading(result));
            Gradings= new ObservableCollection<Grading>();
            Task.Run(async () => await LoadGradings());
        }

        private int position;
        public int Position
        {
            get { return position; }
            set { position = value;
                OnPropertyChanged(nameof(Position)); }
        }

        private async Task LoadGradings()
        {
            List<Grading> gradings = await libraryService.GetGradings(App.CurrentUser.TargetLanguage);
            foreach(Grading grading in gradings)
            {
                Gradings.Add(grading); 
            }
        }

        private async Task CompleteGrading(string result)
        {
            switch(result)
            {
                case ("Book1"):
                    break;
                case ("Book2"):
                    break;
                case ("Equal"):
                    break;
                case ("Skipped"):
                    break;
            }
            if(Position < Gradings.Count-1)
            {
                Position++;
            }
            
        }
    }
}
