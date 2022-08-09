using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Excel;

namespace WebAPI.Models.Database.Junction
{
    [Table("course_question")]
    public class CourseQuestion
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("index")]
        public int Index { set; get; }
        
        [Column("course_id")]
        public int CourseId { get; set; }
        [Column("question_id")]
        public int QuestionId { get; set; }
        
        [ForeignKey("CourseId")]
        public Course Course { set; get; }
        [ForeignKey("QuestionId")]
        public Question Question { set; get; }

        public CourseQuestion()
        {
        }

        public CourseQuestion(int index, int courseId, int questionId)
        {
            Index = index;
            CourseId = courseId;
            QuestionId = questionId;
        }

        public CourseQuestion(Course course, int index, QuestionRow row)
        {
            Course = course;
            Index = index;
            Question = new Question(row, course.Category);
        }

    }
}