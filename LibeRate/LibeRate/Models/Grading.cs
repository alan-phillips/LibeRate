using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.Models
{
    public class Grading
    {
        string Id { get; set; }
        public Book Book1 { get; set; }
        public Book Book2 { get; set; }

        public Grading() { }

        public Grading(string id, Book book1, Book book2)
        {
            Id = id;
            Book1 = book1;
            Book2 = book2;
        }
    }
}
