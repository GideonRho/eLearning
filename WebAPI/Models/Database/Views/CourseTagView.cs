using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models.Database.Views
{
    [Keyless]
    [Table("course_tag_view")]
    public class CourseTagView
    {
        [Column("tag")]
        public string Tag { set; get; }
    }
}