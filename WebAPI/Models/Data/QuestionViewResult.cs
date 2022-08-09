using System.Collections.Generic;
using WebAPI.Models.Database.Views;

namespace WebAPI.Models.Data
{
    public class QuestionViewResult
    {
        
        public int CourseId { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public int CorrectAnswer { get; set; }
        public string CommentImage { get; set; }
        public string CommentText { get; set; }
        public string Image { get; set; }
        public List<string> Options { get; set; }
        public List<string> Infos { get; set; }

        public QuestionViewResult(CourseQuestionByTagView data)
        {
            CourseId = data.CourseId;
            QuestionId = data.QuestionId;
            Title = data.Title;
            CorrectAnswer = data.CorrectAnswer;
            CommentImage = data.CommentImage;
            CommentText = data.CommentText;
            Image = data.Image;
            Options = data.Options;
            Infos = data.Infos;
        }
        
        public QuestionViewResult(CourseQuestionView data)
        {
            CourseId = data.CourseId;
            QuestionId = data.QuestionId;
            Title = data.Title;
            CorrectAnswer = data.CorrectAnswer;
            CommentImage = data.CommentImage;
            CommentText = data.CommentText;
            Image = data.Image;
            Options = data.Options;
            Infos = data.Infos;
        }
        
        public QuestionViewResult(CourseTextQuestionView data)
        {
            CourseId = data.CourseId;
            QuestionId = data.QuestionId;
            Title = data.Title;
            CorrectAnswer = data.CorrectAnswer;
            CommentImage = data.CommentImage;
            CommentText = data.CommentText;
            Image = data.Image;
            Options = data.Options;
            Infos = data.Infos;
        }
        
    }
}