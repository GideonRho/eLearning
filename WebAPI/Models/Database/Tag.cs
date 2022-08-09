using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models.Database
{
    [Table("tag")]
    public class Tag
    {
        
        [Column("id")]
        public int Id { set; get; }
        [Column("name")]
        public string Name { set; get; }

        public Tag()
        {
        }

        public Tag(string name)
        {
            Name = name;
        }
    }
}