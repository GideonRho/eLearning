using NUnit.Framework;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Tests
{
    public class AdminWorkflowTest : AbstractTest
    {

        [Test]
        public void QuestionImport()
        {
            Setup();
            
            var result = client.ImportQuestions("Questions/ImplicationRecognition.xlsx",
                new QuestionImportFilepond(ECategory.ImplicationRecognition));
            
            Assert.AreEqual(10, result.Count);
            
            Finish();
        }

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
                Duration = 60,
                IsActive = true,
            });
            
            
            Finish();
        }
        
    }
}