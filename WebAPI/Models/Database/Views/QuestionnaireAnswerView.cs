using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models.Database.Views
{
    [Keyless]
    [Table("questionnaire_answer_view")]
    public class QuestionnaireAnswerView
    {
        
        [Column("questionnaire_id")]
        public int QuestionnaireId { get; set; }
        [Column("answer_id")]
        public int AnswerId { get; set; }
        
        [Column("choice")]
        public int Choice { get; set; }
        [Column("question_id")]
        public int QuestionId { get; set; }
        
        [Column("title")]
        public string Title { get; set; }
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
        [Column("text_id")]
        public int? TextId { get; set; }
        
    }
}