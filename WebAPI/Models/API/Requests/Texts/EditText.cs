using System.Collections.Generic;

namespace WebAPI.Models.API.Requests.Texts
{
    public class EditText
    {
        
        public string Content { get; set; }
        public string Title { get; set; }
        public bool? IsDeleted { get; set; }

    }
}