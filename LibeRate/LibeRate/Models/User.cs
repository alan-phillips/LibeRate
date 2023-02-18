using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.Models
{
    public class User 
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string TargetLanguage { get; set; }
        public DateTime AccountCreated { get; set; }
        public bool CanGradeBooks { get; set; }

        public User() { }
        public User(string id)
        {
            Id = id;
        } 
        public User(string id, string username, string targetLanguage)
        {
            Id = id; 
            Username = username; 
            TargetLanguage = targetLanguage;
        }
    }
}
