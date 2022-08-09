using System;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;
using WebAPI.Models.Database.Views;

namespace WebAPI.Models.API.Responses
{
    public class QuestionnaireApi
    {
        
        public int Id { get; set; }
        public int CourseId { get; set; }
        public DateTime Timestamp { get; set; }
        public EQuestionnaireState State { get; set; }
        public DateTime StateTimestamp { get; set; }
        public int StateTime { get; set; }
        public int CompletionTime { get; set; }
        
        public ECategory Category { get; set; }

        public QuestionnaireApi()
        {
        }

        public QuestionnaireApi(QuestionnaireView data)
        {
            Id = data.Id;
            CourseId = data.CourseId;
            Timestamp = data.Timestamp;
            State = data.State;
            StateTimestamp = data.StateTimestamp;
            CompletionTime = data.CompletionTime;
            Category = data.Category;
            StateTime = CalcStateTime();
        }

        public QuestionnaireApi(Questionnaire questionnaire, Course course)
        {
            Id = questionnaire.Id;
            CourseId = questionnaire.CourseId;
            Timestamp = questionnaire.Timestamp;
            State = questionnaire.State;
            StateTimestamp = questionnaire.StateTimestamp;
            CompletionTime = questionnaire.CompletionTime;
            Category = course.Category;
            StateTime = CalcStateTime();
        }
        
        private int CalcStateTime()
        {
            var timeDifference = DateTime.Now - StateTimestamp;
            return (int)timeDifference.TotalSeconds;
        }
        
    }
}