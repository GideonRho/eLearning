using WebAPI.Models.Database;
using WebAPI.Models.Database.Views;

namespace WebAPI.Models.API.Responses.Texts
{
    public class CourseTextApi
    {
        
        public int CourseId { get; set; }
        /// <example>3</example>
        public int TextId { get; set; }
        /// <example>Some Content</example>
        public string Content { get; set; }
        /// <example>Some Title</example>
        public string Title { get; set; }

        public CourseTextApi()
        {
        }

        public CourseTextApi(CourseTextView data)
        {
            CourseId = data.CourseId;
            TextId = data.TextId;
            Content = data.Content;
            Title = data.Title;
        }
        
    }
}