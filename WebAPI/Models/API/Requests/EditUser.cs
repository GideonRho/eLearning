using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.API.Requests
{
    public class EditUser
    {
        
        [Required]
        public string OldPassword { set; get; }
        
        public string Username { set; get; }
        public string Password { set; get; }
        public string Email { set; get; }
        
    }
}