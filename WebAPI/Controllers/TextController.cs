using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contexts;
using WebAPI.Misc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = UserService.RoleUser)]
    [Route("[controller]")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public class TextController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly IDataService _dataService;
        private readonly IImportService _importService;

        public TextController(DatabaseContext context, IDataService dataService, IImportService importService)
        {
            _context = context;
            _dataService = dataService;
            _importService = importService;
        }

    }
}