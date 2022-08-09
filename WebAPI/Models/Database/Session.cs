using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database
{
    [Table("session")]
    public class Session
    {
        
        [Column("id")]
        public string Id { set; get; }
        [Column("timestamp")]
        public DateTime Timestamp { set; get; }

        [Column("user_id")]
        public int UserId { set; get; }
        [ForeignKey("UserId")]
        public User User { set; get; }

        public Session() {}

        public Session(string id, DateTime timestamp, User user)
        {
            Id = id;
            Timestamp = timestamp;
            User = user;
        }

        public bool IsExpired()
        {
            TimeSpan timeSpan = DateTime.Now - Timestamp;
            if (timeSpan.TotalHours > 24) return true;
            return false;
        }
        
    }
}