using System.Collections.Generic;

namespace WebAPI.Models.API.Requests.Questions
{
    public class CreateQuestionBulk
    {
        
        public List<CreateQuestion> Questions { set; get; }
        
    }
}