using System.Collections.Generic;

namespace WebAPI.Models.API.Requests.Questions
{
    public class EditQuestion
    {
        
        public string Title { set; get; }
        public string Key { set; get; }
        public List<string> Options { set; get; }
        public int? CorrectAnswer { set; get; }
        public string CommentText { set; get; }
        public string CommentImage { set; get; }
        public string Image { set; get; }
        public List<string> Tags { set; get; }
        public List<string> Infos { set; get; }
        public bool? IsDeleted { get; set; }
        
    }
}