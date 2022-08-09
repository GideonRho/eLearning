using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Misc;
using WebAPI.Models.API.Responses;
using WebAPI.Services;

namespace WebAPI.Controllers.Admin
{
    
    [ApiController]
    [Authorize(Roles = UserService.RoleAdmin)]
    [Route("admin/tag")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    
    public class TagController : ControllerBase
    {
        
        private readonly DatabaseContext _context;

        public TagController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<TagApi>>> Get()
        {

            var courseTags = await _context.CourseTagView
                .Select(v => v.Tag).ToListAsync();
            var questionTags = await _context.QuestionTagView
                .Select(v => v.Tag).ToListAsync();

            return courseTags.Union(questionTags).Distinct().Select(s => new TagApi(s)).ToList();
        }
        
    }
}