using System;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Views;

namespace WebAPI.Models.API.Responses.Statistics
{
    public class QuestionnaireStatistic
    {
        
        public int QuestionnaireId { get; set; }
        public DateTime Date { get; set; }
        public int Correct { set; get; }
        public int Wrong { get; set; }
        public int Ranking { get; set; }

        public QuestionnaireStatistic()
        {
        }

        public QuestionnaireStatistic(Questionnaire questionnaire, int ranking)
        {
            QuestionnaireId = questionnaire.Id;
            Date = questionnaire.Timestamp;
            Correct = questionnaire.CorrectAnswers;
            Wrong = questionnaire.WrongAnswers;
            Ranking = ranking;
        }

    }
}