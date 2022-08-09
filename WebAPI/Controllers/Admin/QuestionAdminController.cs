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
using WebAPI.Models.API.Requests.Questions;
using WebAPI.Models.API.Responses;
using WebAPI.Models.API.Responses.Questions;
using WebAPI.Models.Database.Enums;
using WebAPI.Services;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = UserService.RoleAdmin)]
    [Route("admin/question")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public class QuestionAdminController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly IDataService _dataService;
        private readonly IImportService _importService;

        public QuestionAdminController(DatabaseContext context, IImportService importService, IDataService dataService)
        {
            _context = context;
            _importService = importService;
            _dataService = dataService;
        }

        [HttpPost("import")]
        [Produces("application/json")]
        public async Task<ActionResult<List<QuestionApi>>> Import()
        {

            if (Request.Form.Files.Count == 0) return BadRequest(new GenericError("No file was given."));
            IFormFile file = Request.Form.Files[0];

            ECategory category = JsonConvert.DeserializeObject<QuestionImportFilepond>(Request.Form["filepond"]).Category;

            try
            {
                var dbQuestions = await _importService.ImportQuestions(file.OpenReadStream(), category);
                var result = dbQuestions.Select(q => new QuestionApi(q)).ToList();
                return result;
            }
            catch (ValidationException ve)
            {
                return StatusCode(422, new GenericError(ve.Message));
            }
            
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<List<QuestionApi>>> Post(CreateQuestionBulk data)
        {

            var questions = await _dataService.Create(data);

            return questions.Select(q => new QuestionApi(q)).ToList();
        }
        
        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> EditQuestion(int id, EditQuestion data)
        {

            await _dataService.Edit(id, data);
             
            return Ok();
        }

        [HttpPost("search")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<List<QuestionApi>>> Search(QuestionFilter filter)
        {
            var list = await _context.Questions.Filter(filter).ToListAsync();
            return list.FilterTags(filter).Select(q => new QuestionApi(q)).ToList();
        }

    }
}