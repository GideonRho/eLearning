using WebAPI.Models.Database;

namespace WebAPI.Models.API.Responses.Texts
{
    public class TextApi
    {
        
        /// <example>3</example>
        public int Id { get; set; }
        /// <example>Some Content</example>
        public string Content { get; set; }
        /// <example>Some Title</example>
        public string Title { get; set; }

        public TextApi()
        {
        }

        public TextApi(Text dbText)
        {
            Id = dbText.Id;
            Content = dbText.Content;
            Title = dbText.Title;
        }
        
    }
}