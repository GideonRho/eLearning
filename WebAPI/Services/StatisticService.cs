using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Helpers;
using WebAPI.Models.API.Enums;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Responses.Statistics;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;
using WebAPI.Services.Statistics;

namespace WebAPI.Services
{
    public class StatisticService
    {

        private readonly DatabaseContext _context;

        public StatisticService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<QuestionnaireStatistic> ForQuestionnaire(int id)
        {

            var questionnaire = await _context.Questionnaires.FindAsync(id);
            var result = new QuestionnaireStatistic(questionnaire, await QuestionnairePercentile(questionnaire));

            return result;
        }

        public async Task<List<QuestionnaireStatistic>> History(int userId, ECategory category, HistoryPayload data)
        {
            var questionnaires = await _context.Questionnaires
                .Include(q => q.Course)
                .Where(q => q.UserId == userId && q.Course.Category == category && q.State == EQuestionnaireState.Done)
                .FilterCourseType(data.Type)
                .OrderByDescending(q => q.Timestamp)
                .Skip(data.Offset)
                .Take(data.Amount)
                .ToListAsync();
            
            List<QuestionnaireStatistic> list = new List<QuestionnaireStatistic>();
            foreach (var questionnaire in questionnaires)
                list.Add(new QuestionnaireStatistic(questionnaire, await QuestionnairePercentile(questionnaire)));
            
            return list;
        }

        public async Task<UserStatistic> ForUser(int id)
        {

            StatisticGroup simulation = await ForUser(id, ECourseType.Simulation);
            StatisticGroup training = await ForUser(id, ECourseType.Training);
            StatisticGroup global = await ForUser(id, null);
            
            return new UserStatistic(training, simulation, global);
        }

        private async Task<StatisticGroup> ForUser(int id, ECourseType? type)
        {
            var total = await _context.Questions
                .Where(q => !q.IsDeleted)
                .CountAsync();

            var progressList = await _context.Progresses
                .Where(p => p.UserId == id)
                .ToListAsync();

            int correct = progressList.Count(p => p.GetState(type) == EProgressState.Correct);
            int wrong = progressList.Count(p => p.GetState(type) == EProgressState.Wrong);

            return new StatisticGroup(correct, wrong, total);
        }
        
        public async Task<StatisticGroup> ForCategory(ECategory category, ECourseType? type, int userId)
        {

            var total = await _context.Questions
                .Where(q => !q.IsDeleted && q.Category == category)
                .CountAsync();

            var progressList = await _context.Progresses
                .Include(p => p.Question)
                .Where(p => p.Question.Category == category && p.UserId == userId)
                .ToListAsync();

            int correct = progressList.Count(p => p.GetState(type) == EProgressState.Correct);
            int wrong = progressList.Count(p => p.GetState(type) == EProgressState.Wrong);

            return new StatisticGroup(correct, wrong, total);
        }

        private async Task<int> QuestionnairePercentile(Questionnaire questionnaire)
        {
            await UpdateCourseRanking(questionnaire.CourseId);

            var questionnaireRanking = new QuestionnaireRanking(questionnaire);

            var ranking = await _context.Rankings
                .Where(r => r.CourseId == questionnaire.CourseId && r.Points == questionnaireRanking.Points)
                .FirstOrDefaultAsync();

            if (ranking == null) return 0;

            return ranking.Percentile;
        }
        
        private async Task UpdateCourseRanking(int courseId)
        {

            var questionnaires = await _context.Questionnaires
                .Where(q => q.CourseId == courseId && q.State == EQuestionnaireState.Done)
                .ToListAsync();

            var questionnaireRankings = questionnaires
                .Select(q => new QuestionnaireRanking(q))
                .OrderBy(q => q.Points)
                .ToList();

            var rankings = new List<Ranking>();
            
            for (int p = 0; p <= 100; p++) 
                rankings.Add(new Ranking(courseId, p, GetPercentile(questionnaireRankings, p)));
            
            _context.Rankings.RemoveRange(_context.Rankings.Where(r => r.CourseId == courseId));
            await _context.Rankings.AddRangeAsync(rankings);
            await _context.SaveChangesAsync();

        }

        private int GetPercentile(IReadOnlyList<QuestionnaireRanking> ranking, int points)
        {
            for (int i = 0; i < ranking.Count; i++)
            {
                if (ranking[i].Points > points)
                    return (int)(i * 100.0 / (double)ranking.Count);
            }

            return 100;
        }
        
    }
}