using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database
{
    [Table("answer")]
    public class Answer
    {
        
        [Column("id")]
        public int Id { set; get; }
        
        [Column("choice")]
        public int Choice { set; get; }
        [Column("index")]
        public int Index { set; get; }
        
        [Column("question_id")]
        public int QuestionId { set; get; }
        [ForeignKey("QuestionId")]
        public Question Question { set; get; }
        
        [Column("questionnaire_id")]
        public int QuestionnaireId { get; set; }
        [ForeignKey("QuestionnaireId")]
        public Questionnaire Questionnaire { set; get; }

        public Answer() {}
        
        public Answer(int questionId, int index, Questionnaire questionnaire)
        {
            Choice = -1;
            Index = index;
            QuestionId = questionId;
            Questionnaire = questionnaire;
        }
        
        public Answer(Question question, int index, Questionnaire questionnaire)
        {
            Choice = -1;
            Index = index;
            Question = question;
            Questionnaire = questionnaire;
        }
        
    }
}