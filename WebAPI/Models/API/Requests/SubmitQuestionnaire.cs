using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.API.Requests
{
    public class SubmitQuestionnaire
    {
        
        [Required]
        public List<SetChoice> Choices { set; get; }
        
    }
}