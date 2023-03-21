using LibeRate.Models;
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
            CurrentUser.Instance.UserReloaded += CurrentUser_UserReloaded;
            CanGradeBooks = CurrentUser.Instance.CanGradeBooks;        
        }

        void CurrentUser_UserReloaded(object sender, EventArgs e)
        {
            CanGradeBooks = CurrentUser.Instance.CanGradeBooks;
        }
    }
}
