using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Helpers;
using WebAPI.Misc;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Responses;
using WebAPI.Models.Database.Enums;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = UserService.RoleUser)]
    [Route("[controller]")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public class QuestionnaireController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly QuestionnaireService _questionnaireService;

        public QuestionnaireController(DatabaseContext context, QuestionnaireService questionnaireService)
        {
            _context = context;
            _questionnaireService = questionnaireService;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<QuestionnaireApi>>> Get()
        {
            var userId = User.GetUserId();
            return await _context.QuestionnaireView
                .Where(q => q.UserId == userId)
                .Select(q => new QuestionnaireApi(q))
                .ToListAsync();
        }
        
        [HttpGet("current")]
        [Produces("application/json")]
        public async Task<ActionResult<QuestionnaireApi>> Current()
        {
            var userId = User.GetUserId();
            var user = await _context.Users
                .Include(u => u.CurrentQuestionnaire)
                .ThenInclude(q => q.Course)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user.CurrentQuestionnaire == null) return null;
            return new QuestionnaireApi(user.CurrentQuestionnaire, user.CurrentQuestionnaire.Course);
        }

        [HttpGet("active")]
        [Produces("application/json")]
        public async Task<ActionResult<List<QuestionnaireApi>>> Active()
        {
            var userId = User.GetUserId();
            var list = await _context.Questionnaires
                .Include(q => q.Course)
                .Where(q => q.UserId == userId && q.State != EQuestionnaireState.Done).ToListAsync();
            return list.Select(q => new QuestionnaireApi(q, q.Course)).ToList();
        }
        
        [HttpPost("{id}/setChoice")]
        [Consumes("application/json")]
        public async Task<ActionResult> SetChoice(int id, SetChoice data)
        {
            await _questionnaireService.SetChoice(id, data);
            return Ok();
        }

        [HttpPost("{id}/submit")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> Submit(int id, SubmitQuestionnaire data)
        {
            await _questionnaireService.Submit(id, User.GetUserId(), data);
            return Ok();
        }
        
        [HttpPost("{id}/advance")]
        [Produces("application/json")]
        public async Task<ActionResult> AdvanceState(int id)
        {
            await _questionnaireService.Advance(id);
            return Ok();
        }
        
        [HttpGet("{id}/answers")]
        [Produces("application/json")]
        public async Task<ActionResult<List<AnswerApi>>> Answers(int id)
        {
            return await _context.QuestionnaireAnswerView
                .Where(a => a.QuestionnaireId == id)
                .Select(a => new AnswerApi(a))
                .ToListAsync();
        }
        
        [HttpPost("start/{courseId}")]
        [Produces("application/json")]
        public async Task<ActionResult> Start(int courseId)
        {
            var userId = User.GetUserId();
            await _questionnaireService.Start(userId, courseId);
            return Ok();
        }
        
    }
}