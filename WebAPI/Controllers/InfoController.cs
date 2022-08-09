using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {

        [HttpGet("utcOffset")]
        public async Task<ActionResult<TimeSpan>> Get()
        {
            return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
        }
        
    }
}