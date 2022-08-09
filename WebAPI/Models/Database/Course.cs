using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.Database.Enums;
using WebAPI.Models.Database.Junction;
using WebAPI.Models.Excel;

namespace WebAPI.Models.Database
{
    [Table("course")]
    public class Course
    {
        
        [Column("id")]
        public int Id { set; get; }
        
        [Column("name")]
        public string Name { set; get; }
        [Column("type")]
        public ECourseType Type { set; get; }
        [Column("category")]
        public ECategory Category { set; get; }
        [Column("mode")]
        public ECourseMode Mode { get; set; }
        [Column("active")]
        public bool IsActive { set; get; }
        [Column("duration")]
        public int Duration { set; get; }
        [Column("images")]
        public List<string> Images { set; get; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [Column("memory_duration")]
        public int MemoryDuration { set; get; }
        [Column("memory_delay")]
        public int MemoryDelay { set; get; }
        
        [Column("question_amount")]
        public int QuestionAmount { set; get; }
        [Column("tags")]
        public List<string> Tags { set; get; }
        
        public List<CourseQuestion> Questions { set; get; }
        
        public Course() {}

        public Course(CreateCourse data)
        {
            Name = data.Name;
            IsActive = data.IsActive;
            Category = data.Category;
            Mode = data.Mode;
            Type = data.Type;
            if (data.Images != null) Images = new List<string>(data.Images);
            
            Duration = data.Duration;

            MemoryDuration = data.MemoryDuration;
            MemoryDelay = data.MemoryDelay;

            QuestionAmount = data.QuestionAmount;
            if (data.Tags != null) Tags = new List<string>(data.Tags);
        }

        public Course(ECourseType type, MemoryCourseRow row, IEnumerable<QuestionRow> questions)
        {
            Name = row.name;
            Duration = row.Duration;
            MemoryDelay = row.Delay;
            MemoryDuration = row.MemoryDuration;
            Type = type;
            Tags = row.Tags.ToList();
            Category = ECategory.MemoryPerformance;
            Mode = ECourseMode.Selective;
            Questions = questions.Select((row, index) => new CourseQuestion(this, index, row)).ToList();
        }

    }
}