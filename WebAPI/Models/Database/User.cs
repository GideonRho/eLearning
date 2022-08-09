using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.Database
{
    [Table("user")]
    public class User
    {
        
        [JsonIgnore]
        [Column("id")]
        public int Id { set; get; }
        [JsonIgnore]
        [Column("name")]
        public string Name { set; get; }
        [JsonIgnore]
        [Column("email")]
        public string Email { set; get; }
        [JsonIgnore]
        [Column("password")]
        public string Password { set; get; }
        [JsonIgnore]
        [Column("salt")]
        public byte[] Salt { set; get; }
        [JsonIgnore]
        [Column("role")]
        public ERole Role { set; get; }
        [JsonIgnore]
        [Column("verification_code")]
        public string VerificationCode { set; get; }
        [JsonIgnore]
        [Column("email_confirmed")]
        public bool EmailConfirmed { set; get; }
        
        [JsonIgnore]
        [ForeignKey("questionnaire_id")]
        public Questionnaire CurrentQuestionnaire { set; get; }

        public User()
        {
        }

        public User(string name, string password, string email, byte[] salt)
        {
            Name = name;
            Password = password;
            Email = email;
            Salt = salt;
        }
    }
}