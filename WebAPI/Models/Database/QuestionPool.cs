using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebAPI.Models.Database.Junction;

namespace WebAPI.Models.Database
{
    [Table("question_pool")]
    public class QuestionPool
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("count")]
        public int Count { set; get; }

        public Course Course { set; get; }
        public List<QuestionPoolTag> Tags { set; get; }

        public QuestionPool()
        {
        }

        public QuestionPool(IEnumerable<Tag> tags, int count)
        {
            Count = count;
            Tags = new List<QuestionPoolTag>();
            Tags.AddRange(tags.Select(t => new QuestionPoolTag(this, t)));
        }

        public void UpdateTags(IEnumerable<Tag> tags)
        {
            Tags = tags.Select(t => new QuestionPoolTag(this, t)).ToList();
        }

        public IEnumerable<Question> Filter(IEnumerable<Question> questions) 
            => questions.Where(q => q.Category == Course.Category);

    }
}