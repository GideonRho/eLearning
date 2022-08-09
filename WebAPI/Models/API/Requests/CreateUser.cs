using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.API.Requests
{
    public class CreateUser
    {
        
        /// <example>Peter</example>
        [Required]
        public string Username { set; get; }
        /// <example>1234</example>
        [Required]
        public string Password { set; get; }

        /// <example>hans@gmail.com</example>
        [Required]
        public string Email { set; get; }
        
    }
}