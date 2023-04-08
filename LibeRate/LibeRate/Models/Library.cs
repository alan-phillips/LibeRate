using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LibeRate.Models
{
    public class Library : ObservableCollection<Book>
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Library() { }

        public Library(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
