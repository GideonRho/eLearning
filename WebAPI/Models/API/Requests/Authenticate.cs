using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.API.Requests
{
    public class Authenticate
    {
        
        /// <example>Hans</example>
        [Required]
        public string Username { set; get; }
        /// <example>1234</example>
        [Required]
        public string Password { set; get; }

    }
}