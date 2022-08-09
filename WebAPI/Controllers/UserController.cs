using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Helpers;
using WebAPI.Misc;
using WebAPI.Models.API.Requests;
using WebAPI.Models.API.Responses;
using WebAPI.Models.Database;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = UserService.RoleUser)]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;
        private readonly ProductService _productService;
        private readonly MailService _mailService;

        public UserController(DatabaseContext context, IUserService userService, ProductService productService, MailService mailService)
        {
            _context = context;
            _userService = userService;
            _productService = productService;
            _mailService = mailService;
        }
        
        /// <summary>
        /// Performs a login with the given username and password.
        /// </summary>
        /// <param name="data"></param>
        /// <response code="200">Authentication was successful.</response>
        /// <response code="400">Username or password was incorrect.</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult<UserApi>> Authenticate(Authenticate data)
        {
            var auth = await _userService.Authenticate(data.Username, data.Password);

            if (auth == null)
                return BadRequest("Username or password was incorrect!");

            User user = auth.Value.Item1;
            ClaimsPrincipal principal = auth.Value.Item2;

            // sign-in
            await HttpContext.SignInAsync(
                scheme: "AuthSecurityScheme",
                principal: principal);
            
            return Ok(new UserApi(user.Id, user.Name, user.EmailConfirmed));
        }

        /// <summary>
        /// Creates a new user with the given data.
        /// </summary>
        /// <param name="data"></param>
        /// <response code="200">The user was successfully created.</response>
        /// <response code="400">The user could not be created!</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> Register(CreateUser data)
        {
            User user = await _userService.Create(data.Username, data.Password, data.Email);

            if (user == null)
                return BadRequest("The user could not be created!");

            await _mailService.SendConfirmationMail(user);
            
            return Ok();
        }
        
        /// <summary>
        /// Edits the profile of the current user.
        /// </summary>
        /// <param name="data"></param>
        /// <response code="200">The profile was successfully edited.</response>
        /// <response code="400">Profile could not be edited, check the password!</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("edit")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> EditProfile(EditUser data)
        {
            var userId = User.GetUserId();
            
            var result = await _userService.Edit(userId, data);

            if (!result)
                return BadRequest("Profile could not be edited, check the password!");
            return Ok();
        }

        /// <summary>
        /// Ends the current session for the user.
        /// </summary>
        /// <response code="200">The session was ended successfully.</response>
        /// <response code="404">The current session token does not exist!</response>
        /// <returns></returns>
        [HttpPost("logout")]
        [Produces("application/json")]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<ActionResult> Logout()
        {

            var userId = User.GetUserId();
            var token = User.GetToken();

            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == token);
            if (session == null) return NotFound();
            if (session.UserId != userId) return Unauthorized();

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
        
        /// <summary>
        /// Ends all sessions for the current user, will effectively log him out of all devices.
        /// </summary>
        /// <response code="200">All sessions where ended successfully</response>
        /// <returns></returns>
        [HttpPost("logoutAll")]
        [Produces("application/json")]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<ActionResult> LogoutAll()
        {

            var userId = User.GetUserId();

            var sessions = await _context.Sessions.Where(s => s.UserId == userId).ToListAsync();
            
            _context.Sessions.RemoveRange(sessions);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        /// <summary>
        /// Tries to register the given product key to the current user.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("registerKey")]
        [Consumes("application/json")]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<ActionResult> RegisterKey(RegisterKey key)
        {
         
            var userId = User.GetUserId();

            var result = await _productService.RegisterKey(userId, key.Key);
            if (result == false)
                return BadRequest("Invalid product key.");

            return Ok();
        }

        /// <summary>
        /// Sets the current questionnaire for the current user to the given questionnaire by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("currentQuestionnaire/{id}")]
        [Consumes("application/json")]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<ActionResult> SetCurrentQuestionnaire(int id)
        {
            var userId = User.GetUserId();

            if (!await _userService.SetCurrentQuestionnaire(userId, id))
                return BadRequest();
                
            return Ok();
        }

        /// <summary>
        /// Confirms the email address for the current user with the given code.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// <response code="200">Email has been confirmed successfully</response>
        /// <response code="400">Invalid confirmation code was given.</response>
        /// </returns>
        [HttpPost("confirmMail")]
        [Consumes("application/json")]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<ActionResult> ConfirmMail(CodeApi data)
        {
            var userId = User.GetUserId();

            if (!await _userService.ConfirmMail(userId, data.Code))
                return BadRequest();
            
            return Ok();
        }

        /// <summary>
        /// Indicates that the server should send a new confirmation mail to the current user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("sendConfirmationMail")]
        [Consumes("application/json")]
        [TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<ActionResult> RequestConfirmationMail()
        {
            var userId = User.GetUserId();

            if (!await _mailService.SendConfirmationMail(await _userService.Get(userId)))
                return BadRequest();

            return Ok();
        }
        
        /// <summary>
        /// Refreshes the current session, granting a new session token.
        /// </summary>
        /// <response code="200">Session was successfully refreshed</response>
        /// <response code="400">Current session is invalid.</response>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refreshSession")]
        [Produces("application/json")]
        public async Task<ActionResult<UserApi>> RefreshSession()
        {
            var userId = User.GetUserId();
            var auth = await _userService.RefreshSession(userId, User.GetToken());

            if (auth == null)
                return BadRequest();

            User user = auth.Value.Item1;
            ClaimsPrincipal principal = auth.Value.Item2;

            // sign-in
            await HttpContext.SignInAsync(
                scheme: "AuthSecurityScheme",
                principal: principal);
            
            return Ok(new UserApi(user.Id, user.Name, user.EmailConfirmed));
        }
        
    }
}