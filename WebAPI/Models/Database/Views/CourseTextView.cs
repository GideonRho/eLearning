using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models.Database.Views
{
    [Keyless]
    [Table("course_text_view")]
    public class CourseTextView
    {
        
        [Column("course_id")]
        public int CourseId { set; get; }
        [Column("text_id")]
        public int TextId { set; get; }
        [Column("title")]
        public string Title { set; get; }
        [Column("content")]
        public string Content { set; get; }
        
    }
}