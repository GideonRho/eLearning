using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models.Database.Views
{
    [Keyless]
    [Table("question_tag_view")]
    public class QuestionTagView
    {
        [Column("tag")]
        public string Tag { set; get; }
    }
}