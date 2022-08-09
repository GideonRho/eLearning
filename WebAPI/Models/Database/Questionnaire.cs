using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebAPI.Models.Data;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.Database
{
    [Table("questionnaire")]
    public class Questionnaire
    {
        
        [Column("id")]
        public int Id { set; get; }
        
        [Column("timestamp")]
        public DateTime Timestamp { set; get; }
        [Column("state_timestamp")]
        public DateTime StateTimestamp { set; get; }
        [Column("status")]
        public EQuestionnaireState State {private set; get; }
        [Column("completion_time")]
        public int CompletionTime { set; get; }
        
        [Column("correct_answers")]
        public int CorrectAnswers { set; get; }
        [Column("wrong_answers")]
        public int WrongAnswers { set; get; }
        
        [Column("user_id")]
        public int UserId { set; get; }
        [ForeignKey("UserId")]
        public User User { set; get; }

        [Column("course_id")]
        public int CourseId { set; get; }
        [ForeignKey("CourseId")]
        public Course Course { set; get; }

        public List<Answer> Answers { set; get; }
        
        public Questionnaire() {}

        public Questionnaire(int userId, Course course, IEnumerable<QuestionViewResult> questions)
        {
            UserId = userId;
            Course = course;
            Answers = questions.Select((q, index) => new Answer(q.QuestionId, index, this)).ToList();
            Timestamp = DateTime.Now;
            
            SetState(course.Category == ECategory.MemoryPerformance ? 
                EQuestionnaireState.Preparing : 
                EQuestionnaireState.Ongoing);
        }

        public void SetState(EQuestionnaireState state)
        {
            State = state;
            StateTimestamp = DateTime.Now;
        }
        
    }
}