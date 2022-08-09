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
using WebAPI.Models.API.Requests.Texts;
using WebAPI.Models.API.Responses;
using WebAPI.Models.API.Responses.Questions;
using WebAPI.Models.API.Responses.Texts;
using WebAPI.Services;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = UserService.RoleAdmin)]
    [Route("admin/text")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public class TextAdminController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly IDataService _dataService;
        private readonly IImportService _importService;

        public TextAdminController(DatabaseContext context, IImportService importService, IDataService dataService)
        {
            _context = context;
            _importService = importService;
            _dataService = dataService;
        }

        [HttpGet("{id}/questions")]
        [Produces("application/json")]
        public async Task<ActionResult<List<TextQuestionApi>>> GetQuestions(int id)
        {

            var questions = await _context.TextQuestionView
                .Where(t => t.TextId == id)
                .ToListAsync();

            var all = await _context.TextQuestionView.ToListAsync();
            
            return questions.Select(q => new TextQuestionApi(q)).ToList();
        }
        
        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> EditText(int id, EditText data)
        {

            await _dataService.Edit(id, data);
             
            return Ok();
        }

        [HttpPatch("{id}/questions")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> SetQuestions(int id, SetTextQuestions data)
        { 
            await _dataService.SetQuestions(id, data);
             
            return Ok();
        }
        
        [HttpPost("search")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<List<TextApi>>> Search(TextFilter filter)
        {
            return await _context.Texts.Filter(filter).Select(t => new TextApi(t)).ToListAsync();
        }
        
        [HttpPost("import")]
        [Authorize(Roles = UserService.RoleAdmin)]
        [Produces("application/json")]
        public async Task<ActionResult<TextApi>> Import()
        {

            if (Request.Form.Files.Count == 0) return BadRequest(new GenericError("No file was given."));
            IFormFile file = Request.Form.Files[0];

            TextImportFilepond filepond = JsonConvert.DeserializeObject<TextImportFilepond>(Request.Form["filepond"]);

            try
            {
                var text = await _importService.ImportText(file.OpenReadStream());
                return new TextApi(text);
            }
            catch (ValidationException ve)
            {
                return StatusCode(422, new GenericError(ve.Message));
            }
            
        }
        
    }
}