using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contexts;
using WebAPI.Misc;
using WebAPI.Services;

namespace WebAPI.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = UserService.RoleAdmin)]
    [Route("admin/image")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public class ImageAdminController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly IDataService _dataService;
        private readonly IImportService _importService;

        public ImageAdminController(DatabaseContext context, IDataService dataService, IImportService importService)
        {
            _context = context;
            _dataService = dataService;
            _importService = importService;
        }

        [HttpPost]
        [Authorize(Roles = UserService.RoleAdmin)]
        [Produces("application/json")]
        public async Task<ActionResult<string>> Post()
        {

            string name = await _importService.ImportImage(Request.Form.Files[0]);

            return name;
        }
        
    }
}