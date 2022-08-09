using System.Collections.Generic;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Views;

namespace WebAPI.Models.API.Responses
{
    public class AnswerApi
    {
        
        public int AnswerId { get; set; }
        public int Choice { get; set; }
        
        
        public int QuestionId { get; set; }
        public string Title { set; get; }
        public int CorrectAnswer { set; get; }
        public string CommentText { set; get; }
        public string CommentImage { set; get; }
        public string Image { set; get; }
        public int? TextId { get; set; }
        
        public List<string> Options { set; get; }
        public List<string> Infos { set; get; }

        public AnswerApi()
        {
        }

        public AnswerApi(QuestionnaireAnswerView data)
        {

            AnswerId = data.AnswerId;
            Choice = data.Choice;
            
            QuestionId = data.QuestionId;
            Title = data.Title;
            CorrectAnswer = data.CorrectAnswer;
            CommentText = data.CommentText;
            CommentImage = data.CommentImage;
            Image = data.Image;
            TextId = data.TextId;

            Options = data.Options;
            Infos = data.Infos;

        }

        public AnswerApi(Answer data)
        {
            AnswerId = data.Id;
            Choice = data.Choice;
            
            QuestionId = data.QuestionId;
            Title = data.Question.Title;
            CorrectAnswer = data.Question.CorrectAnswer;
            CommentText = data.Question.CommentText;
            CommentImage = data.Question.CommentImage;
            Image = data.Question.Image;
            TextId = data.Question.TextId;

            Options = data.Question.Options;
            Infos = data.Question.Infos;
        }
    }
}