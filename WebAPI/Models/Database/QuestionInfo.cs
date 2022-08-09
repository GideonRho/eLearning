using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database
{
    
    [Table("question_info")]
    public class QuestionInfo
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("text")]
        public string Text { set; get; }
        [Column("index")]
        public int Index { set; get; }
        
        [ForeignKey("question_id")]
        public Question Question { set; get; }

        public QuestionInfo()
        {
        }

        public QuestionInfo( Question question, int index, string text)
        {
            Text = text;
            Index = index;
            Question = question;
        }
    }
}