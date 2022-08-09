using System.Collections.Generic;

namespace WebAPI.Models.API.Requests.Courses
{
    public class EditCourse
    {
        
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public List<string> Images { get; set; }
        public bool? IsDeleted { get; set; }
        
        public int? Duration { get; set; }
        
        public int? MemoryDuration { get; set; }
        public int? MemoryDelay { get; set; }
        
        public int? QuestionAmount { get; set; }
        public List<string> Tags { get; set; }
        
    }
}