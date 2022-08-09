using System.Collections.Generic;
using NUnit.Framework;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Tests
{
    public class ImportTest : AbstractTest
    {
        
        [Test]
        public void General()
        {
            Setup();

            var text = client.ImportTexts("Texts/Biopolymere.xlsx",
                new TextImportFilepond());

            var questions = client.GetTextQuestions(text.Id);
            Assert.AreEqual(3, questions.Count);
            
            client.ImportQuestions("Questions/ImplicationRecognition.xlsx",
                new QuestionImportFilepond(ECategory.ImplicationRecognition));

            client.CreateCourse(new CreateCourse
            {
                Name = "Implikationen Erkennen mittel",
                Category = ECategory.ImplicationRecognition,
                Type = ECourseType.Simulation,
                Duration = 60,
                IsActive = true,
            });
            
            Finish();
        }

    }
}