using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.Models
{
    public class Grading
    {
        public Book Book1 { get; set; }
        public Book Book2 { get; set; }

        public Grading() { }

        public Grading(Book book1, Book book2)
        {
            Book1 = book1;
            Book2 = book2;
        }
    }
}
