using System.Collections.Generic;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Requests
{
    public class CourseFilter
    {
        
        /// <example>0</example>
        public ECategory? Category { get; set; }
        /// <example>0</example>
        public ECourseType? Type { get; set; }
        public List<string> Tags { get; set; }
        public List<string> TitleKeywords { get; set; }
        /// <example>true</example>
        public bool? IsActive { get; set; }

    }
}