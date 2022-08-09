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
using WebAPI.Models.API.Responses.Statistics;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = UserService.RoleUser)]
    [Route("[controller]")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public class StatisticsController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly StatisticService _statisticService;

        public StatisticsController(DatabaseContext context, StatisticService statisticService)
        {
            _context = context;
            _statisticService = statisticService;
        }

        [HttpGet("questionnaire/{id}")]
        [Produces("application/json")]
        public async Task<ActionResult<QuestionnaireStatistic>> ForQuestionnaire(int id)
        {
            return await _statisticService.ForQuestionnaire(id);
        }
        
        [HttpPost("history/{category}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<List<QuestionnaireStatistic>>> History(ECategory category, HistoryPayload data)
        {
            return await _statisticService.History(User.GetUserId(), category, data);
        }
        
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<UserStatistic>> ForUser()
        {
            return await _statisticService.ForUser(User.GetUserId());
        }
        
        [HttpGet("category/{category}/{type}")]
        [Produces("application/json")]
        public async Task<ActionResult<StatisticGroup>> ForCategoryType(ECategory category, ECourseType type)
        {
            return await _statisticService.ForCategory(category, type, User.GetUserId());
        }
        
        [HttpGet("category/{category}")]
        [Produces("application/json")]
        public async Task<ActionResult<StatisticGroup>> ForCategory(ECategory category)
        {
            return await _statisticService.ForCategory(category, null, User.GetUserId());
        }
        
    }
}