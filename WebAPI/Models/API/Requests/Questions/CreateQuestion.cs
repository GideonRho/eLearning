using System.Collections.Generic;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Requests.Questions
{
    public class CreateQuestion
    {
        public string Title { set; get; }
        public string Key { set; get; }
        public ECategory Category { set; get; }
        public int CorrectAnswer { set; get; }
        public string CommentText { set; get; }
        public string CommentImage { set; get; }
        public string Image { set; get; }
        public List<string> Tags { set; get; }
        public List<string> Infos { set; get; }
        public List<string> Options { set; get; }
    }
}