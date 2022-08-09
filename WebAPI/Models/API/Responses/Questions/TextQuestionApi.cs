using System.Collections.Generic;
using WebAPI.Models.Database.Views;

namespace WebAPI.Models.API.Responses.Questions
{
    public class TextQuestionApi
    {
        
        /// <example>3</example>
        public int Id { set; get; }
        /// <example>Alle Füchse sind Schafe. Alle Füchse sind Hasen.</example>
        public string Title { set; get; }
        public string Key { set; get; }
        /// <example>1</example>
        public int CorrectAnswer { set; get; }
        /// <example>Kommentar Text</example>
        public string CommentText { set; get; }
        public string CommentImage { set; get; }
        public string Image { set; get; }

        public List<string> Options { set; get; }
        public List<string> Infos { set; get; }
        
        public TextQuestionApi() {}

        public TextQuestionApi(TextQuestionView entry)
        {
            Id = entry.QuestionId;
            Title = entry.Title;
            Key = entry.Key;
            CorrectAnswer = entry.CorrectAnswer;
            CommentText = entry.CommentText;
            Image = entry.Image;
            CommentImage = entry.CommentImage;
            Infos = entry.Infos ?? new List<string>();
            Options = entry.Options;
        }
        
    }
}