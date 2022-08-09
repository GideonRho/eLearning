using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database.Junction
{
    [Table("question_pool_tag")]
    public class QuestionPoolTag
    {
        
        [Column("id")]
        public int Id { set; get; }
        [ForeignKey("question_pool_id")]
        public QuestionPool QuestionPool { set; get; }
        [ForeignKey("tag_id")]
        public Tag Tag { set; get; }

        public QuestionPoolTag()
        {
        }

        public QuestionPoolTag(QuestionPool questionPool, Tag tag)
        {
            QuestionPool = questionPool;
            Tag = tag;
        }
    }
}