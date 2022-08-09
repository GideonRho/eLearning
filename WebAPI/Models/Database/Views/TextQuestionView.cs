using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models.Database.Views
{
    [Keyless]
    [Table("text_question_view")]
    public class TextQuestionView
    {
        
        [Column("text_id")]
        public int TextId { get; set; }
        [Column("question_id")]
        public int QuestionId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("key")]
        public string Key{ set; get; }
        [Column("correct_answer")]
        public int CorrectAnswer { get; set; }
        [Column("comment_image")]
        public string CommentImage { get; set; }    
        [Column("comment_text")]
        public string CommentText { get; set; }
        [Column("image")]
        public string Image { get; set; }
        [Column("options")]
        public List<string> Options { get; set; }
        [Column("infos")]
        public List<string> Infos { get; set; }

    }
}