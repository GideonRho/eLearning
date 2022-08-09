using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database
{
    [Table("ranking")]
    public class Ranking
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("course_id")]
        public int CourseId { set; get; }
        
        [Column("points")]
        public int Points { set; get; }
        [Column("percentile")]
        public int Percentile { set; get; }

        public Ranking()
        {
        }

        public Ranking(int courseId, int points, int percentile)
        {
            CourseId = courseId;
            Points = points;
            Percentile = percentile;
        }
        
    }
}