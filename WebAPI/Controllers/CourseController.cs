using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Helpers;
using WebAPI.Misc;
using WebAPI.Models.API.Responses;
using WebAPI.Models.API.Responses.Texts;
using WebAPI.Services;

namespace WebAPI.Controllers
 {
     [ApiController]
     [Authorize(Roles = UserService.RoleUser)]
     [Route("[controller]")]
     [TypeFilter(typeof(TokenAuthorizationFilter))]
     public class CourseController : ControllerBase
     {
         
         private readonly DatabaseContext _context;

         public CourseController(DatabaseContext context)
         {
             _context = context;
         }
         
         [HttpGet]
         [Produces("application/json")]
         public async Task<List<CourseApi>> Get()
         {

             if (!User.IsInRole(UserService.RolePremium))
                 return new List<CourseApi>();

             return await _context.Courses
                 .Where(c => c.IsActive && !c.IsDeleted)
                 .Select(c => new CourseApi(c))
                 .ToListAsync();
         }
         
         [HttpGet("{id}/texts")]
         [Produces("application/json")]
         public async Task<List<CourseTextApi>> GetTexts(int id)
         {
             return await _context.CourseTextView
                 .Where(v => v.CourseId == id)
                 .Select(v => new CourseTextApi(v)).ToListAsync();
         }
         
     }
 }