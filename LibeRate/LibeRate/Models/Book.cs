﻿using System;
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
        public float DifficultyRating { get; set; }
        public int UserCount { get; set; }

        public Book() { }
        public Book(string id, string title, string author, string imageURL, float difficultyRating, int userCount)
        {
            Id = id;
            Title = title;
            Author = author;
            ImageURL = imageURL;
            DifficultyRating = difficultyRating;
            UserCount = userCount;
        }
    }
}
