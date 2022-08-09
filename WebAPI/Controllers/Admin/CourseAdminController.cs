using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPI.Contexts;
using WebAPI.Helpers;
using WebAPI.Misc;
using WebAPI.Models.API.Errors;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Requests.Courses;
using WebAPI.Models.API.Responses;
using WebAPI.Models.API.Responses.Questions;
using WebAPI.Models.API.Responses.Texts;
using WebAPI.Services;

namespace WebAPI.Controllers.Admin
 {
     [ApiController]
     [Authorize(Roles = UserService.RoleAdmin)]
     [Route("admin/course")]
     [TypeFilter(typeof(TokenAuthorizationFilter))]
     public class CourseAdminController : ControllerBase
     {
         
         private readonly DatabaseContext _context;
         private readonly IDataService _dataService;
         private readonly IImportService _importService;
 
         public CourseAdminController(DatabaseContext context, IDataService dataService, IImportService importService)
         {
             _context = context;
             _dataService = dataService;
             _importService = importService;
         }

         [HttpPost]
         [Consumes("application/json")]
         [Produces("application/json")]
         public async Task<ActionResult<CourseApi>> CreateCourse(CreateCourse data)
         {

             var course = await _dataService.Create(data);
             
             return new CourseApi(course);
         }

         [HttpPatch("{id}")]
         [Consumes("application/json")]
         [Produces("application/json")]
         public async Task<ActionResult> EditCourse(int id, EditCourse data)
         {

             await _dataService.Edit(id, data);
             
             return Ok();
         }

         [HttpGet("{id}/questions")]
         [Produces("application/json")]
         public async Task<List<CourseQuestionApi>> GetQuestions(int id)
         {
             return await _context.CourseQuestionView
                 .Where(v => v.CourseId == id)
                 .Select(v => new CourseQuestionApi(v)).ToListAsync();
         }

         [HttpPatch("{id}/questions")]
         [Consumes("application/json")]
         [Produces("application/json")]
         public async Task<ActionResult> SetQuestions(int id, SetQuestions data)
         { 
             await _dataService.SetQuestions(id, data);
             
             return Ok();
         }
         
         [HttpGet("{id}/texts")]
         [Produces("application/json")]
         public async Task<List<CourseTextApi>> GetTexts(int id)
         {
             return await _context.CourseTextView
                 .Where(v => v.CourseId == id)
                 .Select(v => new CourseTextApi(v)).ToListAsync();
         }
         
         [HttpPatch("{id}/texts")]
         [Consumes("application/json")]
         [Produces("application/json")]
         public async Task<ActionResult> SetTexts(int id, SetTexts data)
         {

             await _dataService.SetTexts(id, data);
             
             return Ok();
         }
         
         [HttpPost("search")]
         [Consumes("application/json")]
         [Produces("application/json")]
         public async Task<ActionResult<List<CourseApi>>> Search(CourseFilter filter)
         {
             return await _context.Courses.Filter(filter).Select(c => new CourseApi(c)).ToListAsync();
         }
         
         [HttpPost("import/memory")]
         [Authorize(Roles = UserService.RoleAdmin)]
         [Produces("application/json")]
         public async Task<ActionResult<CourseApi>> Import()
         {

             if (Request.Form.Files.Count == 0) return BadRequest(new GenericError("No file was given."));
             IFormFile file = Request.Form.Files[0];

             CourseFilepond filepond = JsonConvert.DeserializeObject<CourseFilepond>(Request.Form["filepond"]);

             try
             {
                 var course = await _importService.ImportMemoryCourse(file.OpenReadStream(), filepond);
                 return new CourseApi(course);
             }
             catch (ValidationException ve)
             {
                 return StatusCode(422, new GenericError(ve.Message));
             }
            
         }
         
     }
 }