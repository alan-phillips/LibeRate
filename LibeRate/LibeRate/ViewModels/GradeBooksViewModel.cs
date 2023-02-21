using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LibeRate.Models;
using LibeRate.Services;
using LibeRate.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using System.Security.Cryptography;
using LibeRate.Views;

namespace LibeRate.ViewModels
{
    public class GradeBooksViewModel : BaseViewModel
    {
        ILibraryService libraryService;
        public ObservableCollection<Grading> Gradings { get; set; }

        public Command<string> CompleteGradingCommand { get; }
        public ICommand PreviousGradingCommand { get; }
        public IAsyncCommand SubmitGradingsCommand { get; }

        public GradeBooksViewModel() 
        { 
            libraryService = DependencyService.Get<ILibraryService>();
            Position= 0;
            FirstGrading = true;
            FinalGrading = false;

            CompleteGradingCommand = new Command<string>(result => CompleteGrading(result));
            PreviousGradingCommand = new Command(PreviousGrading);
            SubmitGradingsCommand = new AsyncCommand(SubmitGradings);

            Gradings= new ObservableCollection<Grading>();
            Task.Run(async () => await LoadGradings());
        }

        private bool firstGrading;
        public bool FirstGrading
        {
            get { return finalGrading; }
            set
            {
                firstGrading = value;
                OnPropertyChanged(nameof(FirstGrading));
            }
        }

        private bool finalGrading;
        public bool FinalGrading
        {
            get { return finalGrading; }
            set
            {
                finalGrading = value;
                OnPropertyChanged(nameof(FinalGrading));
            }
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

        private void CompleteGrading(string result)
        {
            Gradings[Position].Result= result;
            if(Position < Gradings.Count-1)
            {
                FirstGrading = false;
                Position++;
            } else
            {
                FinalGrading=true;
            }
            
        }

        private void PreviousGrading()
        {
            if(Position > 0)
            {
                Position--;
                if(Position == 0) { FirstGrading = true; }
            }
        }

        private async Task SubmitGradings()
        {
            IBookService bs = DependencyService.Get<IBookService>();
            foreach (Grading grading in Gradings)
            {
                if(grading.Result != "skipped")
                {
                    float rating1 = grading.Book1.DifficultyRating;
                    float rating2 = grading.Book2.DifficultyRating;

                    float[] newRatings = EloCalculator.CalculateElo(rating1, rating2, grading.Result);

                    await bs.SetDifficultyRating(App.CurrentUser.TargetLanguage, grading.Book1.Id, newRatings[0]);
                    await bs.SetDifficultyRating(App.CurrentUser.TargetLanguage, grading.Book2.Id, newRatings[1]);
                }

                await libraryService.CompleteGrading(grading.Id, App.CurrentUser.TargetLanguage, grading.Result);
                
            }
            App.CurrentUser.CanGradeBooks = false;
            await Shell.Current.GoToAsync($"//{nameof(SearchPage)}");
        }
    }
}
