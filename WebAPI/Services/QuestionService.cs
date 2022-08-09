using WebAPI.Contexts;

namespace WebAPI.Services
{

    public interface IQuestionService
    {
    }
    
    public class QuestionService : IQuestionService
    {

        private readonly DatabaseContext _context;

        public QuestionService(DatabaseContext context)
        {
            _context = context;
        }

    }
}