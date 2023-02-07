using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.Models
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageURL { get; set; }
        public int DifficultyRating { get; set; }

        public Book() { }
        public Book(string id, string title, string author, string imageURL, int difficultyRating)
        {
            Id = id;
            Title = title;
            Author = author;
            ImageURL = imageURL;
            DifficultyRating = difficultyRating;
        }
    }
}
