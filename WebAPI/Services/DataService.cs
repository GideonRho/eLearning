using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.API.Requests.Questions;
using WebAPI.Models.API.Requests.Texts;
using WebAPI.Models.API.Responses.Questions;
using WebAPI.Models.Data;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;
using WebAPI.Models.Database.Junction;

namespace WebAPI.Services
{

    public interface IDataService
    {
        
        Task<Course> Create(CreateCourse data);
        Task<List<Question>> Create(CreateQuestionBulk data);
        Task SetQuestions(int courseId, SetQuestions data);
        Task SetQuestions(int courseId, SetTextQuestions data);
        Task SetTexts(int courseId, SetTexts data);
        Task Edit(int courseId, EditCourse data);
        Task Edit(int questionId, EditQuestion data);
        Task Edit(int textId, EditText data);

    }
    
    public class DataService : IDataService
    {
        
        private readonly DatabaseContext _context;

        public DataService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Course> Create(CreateCourse data)
        {
            
            var course = new Course(data);

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            
            return course;
        }

        public async Task<List<Question>> Create(CreateQuestionBulk data)
        {

            var questions = data.Questions.Select(d => new Question(d)).ToList();

            await _context.Questions.AddRangeAsync(questions);
            await _context.SaveChangesAsync();

            return questions;
        }

        public async Task SetQuestions(int courseId, SetQuestions data)
        {

            var courseQuestions =
                data.QuestionIds.Select((id, index) => new CourseQuestion(index, courseId, id));

            _context.CourseQuestions.RemoveRange(
                _context.CourseQuestions.Where(q => q.CourseId == courseId));
            await _context.CourseQuestions.AddRangeAsync(courseQuestions);
            await _context.SaveChangesAsync();

        }

        public async Task SetQuestions(int textId, SetTextQuestions data)
        {

            var text = await _context.Texts.FindAsync(textId);
            var questions = await _context.Questions
                .Where(q => data.QuestionIds.Contains(q.Id))
                .OrderBy(q => data.OrderIndex(q.Id))
                .ToListAsync();

            var textQuestions =
                questions.Select((q, index) => new TextQuestion(text, index, q));

            _context.TextQuestions.RemoveRange(
                _context.TextQuestions.Where(q => q.TextId == textId));
            await _context.TextQuestions.AddRangeAsync(textQuestions);
            await _context.SaveChangesAsync();

        }
        
        public async Task SetTexts(int courseId, SetTexts data)
        {

            var courseTexts = data.TextIds
                .Select((id, index) => new CourseText(index, courseId, id));

            _context.CourseTexts.RemoveRange(
                _context.CourseTexts.Where(t => t.CourseId == courseId));
            await _context.AddRangeAsync(courseTexts);
            await _context.SaveChangesAsync();

        }

        public async Task Edit(int courseId, EditCourse data)
        {

            var course = await _context.Courses.FindAsync(courseId);

            if (data.Name != null) course.Name = data.Name;
            if (data.IsActive.HasValue) course.IsActive = data.IsActive.Value;
            if (data.Duration.HasValue) course.Duration = data.Duration.Value;
            if (data.Images != null) course.Images = data.Images;
            if (data.Tags != null) course.Tags = data.Tags;
            if (data.IsDeleted.HasValue) course.IsDeleted = data.IsDeleted.Value;
            if (data.MemoryDelay.HasValue) course.MemoryDelay = data.MemoryDelay.Value;
            if (data.MemoryDuration.HasValue) course.MemoryDuration = data.MemoryDuration.Value;
            if (data.QuestionAmount.HasValue) course.QuestionAmount = data.QuestionAmount.Value;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

        }

        public async Task Edit(int questionId, EditQuestion data)
        {

            var question = await _context.Questions.FindAsync(questionId);

            if (data.Title != null) question.Title = data.Title;
            if (data.Key != null) question.Key = data.Key;
            if (data.Options != null) question.Options = data.Options;
            if (data.CorrectAnswer.HasValue) question.CorrectAnswer = data.CorrectAnswer.Value;
            if (data.CommentText != null) question.CommentText = data.CommentText;
            if (data.CommentImage != null) question.CommentImage = data.CommentImage;
            if (data.Image != null) question.Image = data.Image;
            if (data.Tags != null) question.Tags = data.Tags;
            if (data.Infos != null) question.Infos = data.Infos;
            if (data.IsDeleted.HasValue) question.IsDeleted = data.IsDeleted.Value;
            
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();

        }

        public async Task Edit(int textId, EditText data)
        {

            var text = await _context.Texts.FindAsync(textId);

            if (data.Content != null) text.Content = data.Content;
            if (data.Title != null) text.Title = data.Title;
            if (data.IsDeleted.HasValue) text.IsDeleted = data.IsDeleted.Value;

            _context.Texts.Update(text);
            await _context.SaveChangesAsync();
            
        }

    }
}