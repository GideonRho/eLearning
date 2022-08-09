using System.Collections.Generic;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Requests
{
    public class QuestionFilter
    {
        
        public ECategory? Category { get; set; }
        public List<string> Tags { get; set; }
        public List<string> TitleKeywords { get; set; }

    }
}