using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database
{
    [Table("image")]
    public class Image
    {
        
        [Column("id")]
        public int Id { set; get; }
        
        [Column("location")]
        public string Location { set; get; }

        public Image()
        {
        }

        public Image(string location)
        {
            Location = location;
        }
    }
}