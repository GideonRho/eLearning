using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Junction;
using WebAPI.Models.Database.Views;

namespace WebAPI.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Course> Courses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<ProductKey> ProductKeys { get; set; }
        
        public DbSet<CourseText> CourseTexts { get; set; }
        public DbSet<CourseQuestion> CourseQuestions { get; set; }
        public DbSet<TextQuestion> TextQuestions { get; set; }
        
        public DbSet<CourseQuestionByTagView> CourseQuestionByTagView { get; set; }
        public DbSet<CourseQuestionView> CourseQuestionView { get; set; }
        public DbSet<CourseTextQuestionView> CourseTextQuestionView { get; set; }
        public DbSet<CourseTextView> CourseTextView { get; set; }
        public DbSet<TextQuestionView> TextQuestionView { get; set; }
        public DbSet<CourseTagView> CourseTagView { get; set; }
        public DbSet<QuestionTagView> QuestionTagView { get; set; }
        public DbSet<QuestionnaireView> QuestionnaireView { get; set; }
        public DbSet<QuestionnaireAnswerView> QuestionnaireAnswerView { get; set; }

    }
}