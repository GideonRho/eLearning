using WebAPI.Models.Database.Enums;

namespace WebAPI.Models.API.Requests
{
    public class QuestionImportFilepond
    {
        
        public ECategory Category { get; set; }

        public QuestionImportFilepond()
        {
        }

        public QuestionImportFilepond(ECategory category)
        {
            Category = category;
        }
    }
}