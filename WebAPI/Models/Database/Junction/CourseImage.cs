using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database.Junction
{
    [Table("course_image")]
    public class CourseImage
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("index")]
        public int Index { set; get; }
        
        [ForeignKey("course_id")]
        public Course Course { set; get; }
        [ForeignKey("image_id")]
        public Image Image { set; get; }

        public CourseImage()
        {
        }

        public CourseImage(int index, Course course, Image image)
        {
            Index = index;
            Course = course;
            Image = image;
        }
    }
}