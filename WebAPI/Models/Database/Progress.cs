using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.Database
{
    [Table("progress")]
    public class Progress
    {
        
        [Column("id")]
        public int Id { set; get; }

        [Column("training")]
        public EProgressState Training { set; get; }
        [Column("simulation")]
        public EProgressState Simulation { set; get; }

        [Column("user_id")]
        public int UserId { set; get; }
        [ForeignKey("UserId")]
        public User User { set; get; }
        [Column("question_id")]
        public int QuestionId { set; get; }
        [ForeignKey("QuestionId")]
        public Question Question { set; get; }

        public Progress()
        {
        }

        public Progress(int userId, int questionId)
        {
            UserId = userId;
            QuestionId = questionId;
        }
        
        public void UpdateState(ECourseType type, EProgressState state)
        {

            var oldState = GetState(type);

            if (state == EProgressState.Wrong && oldState == EProgressState.Correct) return;

            if (type == ECourseType.Simulation) Simulation = state;
            if (type == ECourseType.Training) Training = state;

        }

        public EProgressState GetState(ECourseType? type)
        {
            if (!type.HasValue) return HighestState();
            if (type == ECourseType.Simulation) return Simulation;
            return Training;
        }

        public EProgressState HighestState()
        {
            if (Training == EProgressState.Correct || Simulation == EProgressState.Correct) return EProgressState.Correct;
            if (Training == EProgressState.Wrong || Simulation == EProgressState.Wrong) return EProgressState.Wrong;
            return EProgressState.None;
        }
        
    }
}