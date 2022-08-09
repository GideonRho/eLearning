using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Database.Enums;
using WebAPI.Models.Excel;

namespace WebAPI.Models.Database.Junction
{
    [Table("text_question")]
    public class 
        TextQuestion
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("index")]
        public int Index { set; get; }
        
        [Column("text_id")]
        public int TextId { get; set; }
        [ForeignKey("TextId")]
        public Text Text { set; get; }
        
        [Column("question_id")]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { set; get; }
        
        public TextQuestion()
        {
        }
        
        public TextQuestion(Text text, int index, Question question)
        {
            Text = text;
            Index = index;

            Question = question;
            Question.Text = text;

        }
        
        public TextQuestion(Text text, int index, QuestionRow row)
        {
            Text = text;
            Index = index;

            Question = new Question(row, ECategory.TextComprehension);
            Question.Text = text;

        }
        
    }
}