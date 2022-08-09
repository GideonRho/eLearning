using System.Collections.Generic;
using System.Threading;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.Database.Enums;
using WebAPI.Tests.Misc;

namespace WebAPI.Tests
{
    public abstract class AbstractTest
    {
        
        protected Client client;
        
        protected virtual void Setup(Handler.EServerMode mode = Handler.EServerMode.WithTestData)
        {
            Handler.StartServer(mode);
            client = new Client();
        }

        protected void Restart()
        {
            Startup.applicationLifetime.StopApplication();
            Thread.Sleep(100);
            Handler.StartServer(Handler.EServerMode.Restart);
            client = new Client();
        }
        
        protected virtual void Finish()
        {
            Startup.applicationLifetime.StopApplication();
        }

        protected void ImportAll()
        {
            ImportTexts();
            ImportQuestions();
            ImportCourses();
        }

        protected void ImportTexts()
        {
            client.ImportTexts("Texts/Biopolymere.xlsx", new TextImportFilepond());
            client.ImportTexts("Texts/DieAngstVorDoppelgangern.xlsx", new TextImportFilepond());
            client.ImportTexts("Texts/EigenstuhlTransplantation.xlsx", new TextImportFilepond());
            client.ImportTexts("Texts/HarakiriDerZellen.xlsx", new TextImportFilepond());
        }

        protected void ImportQuestions()
        {
            client.ImportQuestions("Questions/ImplicationRecognition.xlsx",
                new QuestionImportFilepond(ECategory.ImplicationRecognition));
        }

        protected void ImportCourses()
        {
            client.CreateCourse(new CreateCourse
            {
                Name = "Implikationen Erkennen mittel",
                Category = ECategory.ImplicationRecognition,
                Type = ECourseType.Simulation,
                Duration = 60,
                IsActive = true,
                Mode = ECourseMode.Randomized,
                QuestionAmount = 4,
                Tags = new List<string>{"einfach"}
            });
        }
        
    }
}