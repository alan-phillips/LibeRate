using System;
using System.Collections.Generic;
using System.Text;
using LibeRate.Models;

namespace LibeRate.ViewModels
{
    public class GradeBooksViewModel : BaseViewModel
    {
        public List<Grading> Gradings { get; set; }

        public GradeBooksViewModel() 
        { 
            
        }
    }
}
