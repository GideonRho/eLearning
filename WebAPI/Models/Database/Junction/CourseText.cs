using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database.Junction
{
    [Table("course_text")]
    public class CourseText
    {
     
        [Column("id")]
        public int Id { set; get; }
        [Column("index")]
        public int Index { set; get; }
        
        [Column("course_id")]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { set; get; }
        
        
        [Column("text_id")]
        public int TextId { get; set; }
        [ForeignKey("TextId")]
        public Text Text { set; get; }

        public CourseText()
        {
        }

        public CourseText(int index, int courseId, int textId)
        {
            Index = index;
            CourseId = courseId;
            TextId = textId;
        }
    }
}