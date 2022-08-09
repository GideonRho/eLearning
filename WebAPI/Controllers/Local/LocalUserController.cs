using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contexts;
using WebAPI.Misc;
using WebAPI.Models.API.Requests;
using WebAPI.Services;

namespace WebAPI.Controllers.Local
{
    [ApiController]
    [LocalHostFilter]
    [Route("local/user")]
    public class LocalUserController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;

        public LocalUserController(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("admin")]
        public async Task<ActionResult> CreateAdmin(CreateUser data)
        {

            await _userService.Create(data.Username, data.Password, "", true);
            
            return Ok();
        }

    }
}