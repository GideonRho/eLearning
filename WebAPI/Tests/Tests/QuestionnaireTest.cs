using System.Collections.Generic;
using NUnit.Framework;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.API.Responses;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Tests
{
    public class QuestionnaireTest : AbstractTest
    {
        
        [Test]
        public void ImplicationRecognitionRandomized()
        {
            Setup();
            
            var questions = client.ImportQuestions("Questions/ImplicationRecognition.xlsx",
                new QuestionImportFilepond(ECategory.ImplicationRecognition));

            var course = client.CreateCourse(new CreateCourse
            {
                Name = "Implikationen Erkennen mittel",
                Category = ECategory.ImplicationRecognition,
                Type = ECourseType.Simulation,
                Mode = ECourseMode.Randomized,
                QuestionAmount = 3,
                Tags = new List<string>{"mittel"},
                Duration = 60,
                IsActive = true,
            });

            
            client.StartQuestionnaire(course.Id);
            var questionnaire = client.CurrentQuestionnaire();
            var answers = client.QuestionnaireAnswers(questionnaire.Id);

            Assert.AreEqual(3, answers.Count);

            Finish();
        }
        
    }
}