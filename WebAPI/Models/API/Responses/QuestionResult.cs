using WebAPI.Models.API.Responses.Questions;
using WebAPI.Models.Database;

namespace WebAPI.Models.API.Responses
{
    public class QuestionResult
    {
        
        /// <example>3</example>
        public int Id { set; get; }
        /// <example>1</example>
        public int UserChoice { set; get; }
        public QuestionApi Question { set; get; }

        public QuestionResult()
        {
        }

        public QuestionResult(Questionnaire questionnaire, Answer answer)
        {
            Question = new QuestionApi(answer.Question);
            UserChoice = answer.Choice;
            Id = answer.Id;
        }

    }
}