using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        private bool canGradeBooks;
        public bool CanGradeBooks { get => canGradeBooks; set => SetProperty(ref canGradeBooks, value); }

        public ShellViewModel() 
        {
            CanGradeBooks = App.CurrentUser.CanGradeBooks;        
        }
    }
}
