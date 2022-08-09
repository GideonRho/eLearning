using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Responses;

namespace WebAPI.Tests
{
    public class StatisticsTest : AbstractTest
    {

        [Test]
        public void Test()
        {
            Setup();
            ImportAll();

            var course = client.GetCourses().First();

            int[] ids = new int[5];
            
            for (int i = 0; i <= 4; i++)
                ids[i] = Submit(course, i);
            
            var statistic = client.StatisticForQuestionnaire(ids[0]);
            Assert.AreEqual(0, statistic.Correct);
            Assert.AreEqual(20, statistic.Ranking);
            
            statistic = client.StatisticForQuestionnaire(ids[4]);
            Assert.AreEqual(4, statistic.Correct);
            Assert.AreEqual(100, statistic.Ranking);
            
            statistic = client.StatisticForQuestionnaire(ids[2]);
            Assert.AreEqual(2, statistic.Correct);
            Assert.AreEqual(60, statistic.Ranking);

            statistic = client.StatisticForQuestionnaire(ids[1]);
            Assert.AreEqual(1, statistic.Correct);
            Assert.AreEqual(40, statistic.Ranking);
            
            statistic = client.StatisticForQuestionnaire(ids[3]);
            Assert.AreEqual(3, statistic.Correct);
            Assert.AreEqual(80, statistic.Ranking);
            
            Finish();
        }

        private int Submit(CourseApi course, int correct)
        {
            client.StartQuestionnaire(course.Id);
            var questionnaire = client.CurrentQuestionnaire();
            var answers = client.QuestionnaireAnswers(questionnaire.Id);
            
            SubmitQuestionnaire data = new SubmitQuestionnaire();
            data.Choices = new List<SetChoice>();
            
            foreach (var answerApi in answers)
            {
                if (correct > 0)
                    data.Choices.Add(new SetChoice(answerApi.AnswerId, answerApi.CorrectAnswer));
                else
                    data.Choices.Add(new SetChoice(answerApi.AnswerId, answerApi.CorrectAnswer > 0 ? 0 : 1));
                
                correct--;
            }

            client.SubmitQuestionnaire(questionnaire.Id, data);

            return questionnaire.Id;
        }
        
    }
}