using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.Database
{
    [Table("product_key")]
    public class ProductKey
    {
        
        [Key]
        [Column("key")]
        public string Key { set; get; }
        [Column("type")]
        public EProductKeyType Type { set; get; }
        [Column("activation_date")]
        public DateTime ActivationDate { set; get; }
        [Column("expiration_date")]
        public DateTime ExpirationDate { set; get; }
        /// <summary>
        /// The duration in days the key will be valid for after activation. 
        /// </summary>
        [Column("duration")]
        public int Duration { set; get; }

        [ForeignKey("user_id")]
        public User User { set; get; }

        public ProductKey()
        {
        }

        public ProductKey(string key, int duration)
        {
            Type = EProductKeyType.Duration;
            Key = key;
            Duration = duration;
        }

        public ProductKey(string key, DateTime expirationDate)
        {
            Type = EProductKeyType.Date;
            Key = key;
            ExpirationDate = expirationDate;
        }

        public void Activate()
        {
            if (Type == EProductKeyType.Date)
            {
                ActivationDate = DateTime.Now;
                ExpirationDate = DateTime.Now + TimeSpan.FromDays(Duration);
            }
        }

        public bool IsValid()
        {
            return ExpirationDate >= DateTime.Now;
        }
        
    }
}