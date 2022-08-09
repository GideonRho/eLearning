using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Models.API.Requests;
using WebAPI.Models.Data;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Services
{
    public class QuestionnaireService
    {
        
        private readonly DatabaseContext _context;

        public QuestionnaireService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SetChoice(int id, SetChoice data)
        {

            var answer = await _context.Answers.FindAsync(data.Id);

            answer.Choice = data.Choice;

            _context.Update(answer);
            await _context.SaveChangesAsync();
        }

        public async Task Submit(int id, int userId, SubmitQuestionnaire data)
        {

            var questionnaire = await _context.Questionnaires
                .Include(q => q.Course)
                .FirstAsync(q => q.Id == id);
            var list = await _context.Answers
                .Include(a => a.Question)
                .Where(a => a.QuestionnaireId == id)
                .ToListAsync();
            var questionIds = list.Select(a => a.QuestionId);
            var progresses = await _context.Progresses
                .Where(p => p.UserId == userId 
                            && questionIds.Contains(p.QuestionId))
                .ToListAsync();
            var newProgress = new List<Progress>();

            int correct = 0;
            int wrong = 0;
            
            foreach (var answer in list)
            {

                Progress progress = progresses
                    .FirstOrDefault(p => p.QuestionId == answer.QuestionId);
                if (progress == null)
                {
                    progress = new Progress(userId, answer.QuestionId);
                    newProgress.Add(progress);
                }

                foreach (var choice in data.Choices)
                {
                    if (choice.Id == answer.Id)
                    {
                        answer.Choice = choice.Choice;
                        if (answer.Choice == answer.Question.CorrectAnswer)
                        {
                            progress.UpdateState(questionnaire.Course.Type, EProgressState.Correct);
                            correct++;
                        }
                        else
                        {
                            progress.UpdateState(questionnaire.Course.Type, EProgressState.Wrong);
                            wrong++;
                        }
                    }
                }


            }

            questionnaire.CompletionTime = (int)(DateTime.Now - questionnaire.StateTimestamp).TotalSeconds; 
            questionnaire.SetState(EQuestionnaireState.Done);
            questionnaire.CorrectAnswers = correct;
            questionnaire.WrongAnswers = wrong;

            await _context.AddRangeAsync(newProgress);
            _context.UpdateRange(progresses);
            _context.UpdateRange(list);
            _context.Update(questionnaire);
            await _context.SaveChangesAsync();

        }

        public async Task Advance(int id)
        {

            var questionnaire = await _context.Questionnaires.
                Include(q => q.Course).
                FirstOrDefaultAsync(q => q.Id == id);

            if (questionnaire.Course.Category == ECategory.MemoryPerformance)
            {
                switch (questionnaire.State)
                {
                    case EQuestionnaireState.Preparing:
                        questionnaire.SetState(EQuestionnaireState.Paused);
                        break;
                    case EQuestionnaireState.Paused:
                        questionnaire.SetState(EQuestionnaireState.Ongoing);
                        break;
                }
            }

            _context.Questionnaires.Update(questionnaire);
            await _context.SaveChangesAsync();
            
        }

        public async Task Start(int userId, int courseId)
        {
            var user = await _context.Users.FindAsync(userId);
            var course = await _context.Courses.FindAsync(courseId);
            var questions = await QuestionsForQuestionnaire(course);
            
            var questionnaire = new Questionnaire(userId, course, questions);
            user.CurrentQuestionnaire = questionnaire;

            _context.Users.Update(user);
            await _context.Questionnaires.AddAsync(questionnaire);
            await _context.SaveChangesAsync();
        }
        
        private async Task<List<QuestionViewResult>> QuestionsForQuestionnaire(Course course)
        {
            return await QuestionsForCourse(course);
        }
        
        private async Task<List<QuestionViewResult>> QuestionsForCourse(Course course)
        {
            if (course.Category == ECategory.TextComprehension) return await QuestionsForTextCourse(course);
            if (course.Mode == ECourseMode.Randomized) 
                return TakeRandom(await QuestionsForRandomizedCourse(course), course.QuestionAmount);
            if (course.Mode == ECourseMode.Selective) return await QuestionsForSelectiveCourse(course);
            throw new NotImplementedException();
        }

        private async Task<List<QuestionViewResult>> QuestionsForTextCourse(Course course)
        {
            var data = _context.CourseTextQuestionView
                .Where(d => d.CourseId == course.Id);
            return await data.Select(d => new QuestionViewResult(d)).ToListAsync();
        }
        
        private async Task<List<QuestionViewResult>> QuestionsForRandomizedCourse(Course course)
        {
            var data = _context.CourseQuestionByTagView
                .Where(d => d.CourseId == course.Id);
            return await data.Select(d => new QuestionViewResult(d))
                .ToListAsync();
        }

        private List<QuestionViewResult> TakeRandom(IEnumerable<QuestionViewResult> questions, int amount)
        {
            var list = new List<QuestionViewResult>(questions);
            var result = new List<QuestionViewResult>();

            if (amount >= list.Count) return list;
            
            Random random = new Random();
            for (int i = 0; i < amount; i++)
            {
                int index = random.Next(list.Count);
                result.Add(list[index]);
                list.RemoveAt(index);
            }
            
            return result;
        }
        
        private async Task<List<QuestionViewResult>> QuestionsForSelectiveCourse(Course course)
        {
            var data = _context.CourseQuestionView
                .Where(d => d.CourseId == course.Id);
            return await data.Select(d => new QuestionViewResult(d)).ToListAsync();
        }
        
    }
}