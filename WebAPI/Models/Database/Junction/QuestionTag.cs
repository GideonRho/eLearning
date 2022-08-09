using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database.Junction
{
    [Table("question_tag")]
    public class QuestionTag
    {
        
        [Column("id")]
        public int Id { set; get; }
        [ForeignKey("question_id")]
        public Question Question { set; get; }
        [ForeignKey("tag_id")]
        public Tag Tag { set; get; }

        public QuestionTag()
        {
        }

        public QuestionTag(Question question, Tag tag)
        {
            Question = question;
            Tag = tag;
        }
    }
}