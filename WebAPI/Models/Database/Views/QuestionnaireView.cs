using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.Database.Views
{
    [Keyless]
    [Table("questionnaire_view")]
    public class QuestionnaireView
    {
        
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("course_id")]
        public int CourseId { get; set; }
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
        [Column("state_timestamp")]
        public DateTime StateTimestamp { get; set; }
        [Column("status")]
        public EQuestionnaireState State { get; set; }
        [Column("completion_time")]
        public int CompletionTime { get; set; }
        
        [Column("correct_answers")]
        public int CorrectAnswers { set; get; }
        [Column("wrong_answers")]
        public int WrongAnswers { set; get; }
        
        [Column("category")]
        public ECategory Category { get; set; }
        
    }
}