using System.Collections.Generic;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Requests.Courses
{
    public class CreateCourse
    {
        
        public string Name { get; set; }
        public bool IsActive { get; set; } 
        public ECategory Category { get; set; }
        public ECourseType Type { get; set; }
        public ECourseMode Mode { get; set; }
        public List<string> Images { get; set; }
        
        public int Duration { get; set; }
        
        public int MemoryDuration { get; set; }
        public int MemoryDelay { get; set; }
        
        public int QuestionAmount { get; set; }
        public List<string> Tags { get; set; } 

    }
}