using WebAPI.Models.API.Enums;

namespace WebAPI.Models.API.Requests
{
    public class HistoryPayload
    {
        
        public CourseTypeFilter Type { get; set; }
        public int Offset { get; set; }
        public int Amount { get; set; }
        
    }
}