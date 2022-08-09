using WebAPI.Models.Database;

namespace WebAPI.Services.Statistics
{
    public class QuestionnaireRanking
    {
        
        public Questionnaire Questionnaire { get; set; }
        public int Points { get; set; }

        public QuestionnaireRanking(Questionnaire questionnaire)
        {
            Questionnaire = questionnaire;
            int total = questionnaire.CorrectAnswers + questionnaire.WrongAnswers;
            Points = (int)(questionnaire.CorrectAnswers / (double)total * 100.0);
        }
    }
}