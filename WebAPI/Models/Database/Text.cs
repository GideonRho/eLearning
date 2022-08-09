using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebAPI.Models.Database.Junction;
using WebAPI.Models.Excel;

namespace WebAPI.Models.Database
{
    [Table("text")]
    public class Text
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("title")]
        public string Title { set; get; }
        [Column("content")]
        public string Content { set; get; }
        [Column("is_deleted")]
        public bool IsDeleted { set; get; }

        public List<TextQuestion> Questions { set; get; }

        public Text()
        {
        }

        public Text(TextRow header, IEnumerable<TextRow> questions)
        {
            Title = header.textTitle;
            Content = header.textContent;
            Questions = questions.Select((row, index) => new TextQuestion(this, index, row)).ToList();
        }
        
    }
}