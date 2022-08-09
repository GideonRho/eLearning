using System.Collections.Generic;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Responses.Questions
{
    public class QuestionApi
    {
        
        /// <example>3</example>
        public int Id { set; get; }
        /// <example>Alle Füchse sind Schafe. Alle Füchse sind Hasen.</example>
        public string Title { set; get; }
        public string Key { set; get; }
        /// <example>1</example>
        public int CorrectAnswer { set; get; }
        public ECategory Category { set; get; }
        /// <example>Kommentar Text</example>
        public string CommentText { set; get; }
        public string CommentImage { set; get; }
        public string Image { set; get; }
        
        public List<string> Options { set; get; }
        public List<string> Tags { set; get; }
        public List<string> Infos { set; get; }
        
        public QuestionApi() {}

        public QuestionApi(Database.Question entry)
        {
            Id = entry.Id;
            Title = entry.Title;
            Key = entry.Key;
            CorrectAnswer = entry.CorrectAnswer;
            Category = entry.Category;
            CommentText = entry.CommentText;
            Image = entry.Image;
            CommentImage = entry.CommentImage;
            Infos = entry.Infos ?? new List<string>();
            Tags = entry.Tags ?? new List<string>();
            Options = entry.Options;
        }

    }
}