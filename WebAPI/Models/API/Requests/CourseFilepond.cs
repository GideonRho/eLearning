using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Requests
{
    public class CourseFilepond
    {
        
        public ECourseType Type { get; set; }
        public bool IsActive { get; set; }
        
    }
}