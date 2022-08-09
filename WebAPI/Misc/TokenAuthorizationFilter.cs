using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Contexts;
using WebAPI.Helpers;

namespace WebAPI.Misc
{
    public sealed class TokenAuthorizationFilter : Attribute, IAuthorizationFilter
    {

        private DatabaseContext _context;

        public TokenAuthorizationFilter(DatabaseContext context)
        {
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            if (filterContext.HttpContext?.User == null)
            {
                filterContext.Result = new UnauthorizedResult();
                return;
            }
            
            string token = filterContext.HttpContext.User.GetToken();
            int userId = filterContext.HttpContext.User.GetUserId();
            var session = _context.Sessions.FirstOrDefault(s => s.Id == token);

            if (session == null || userId != session.UserId)
            {
                filterContext.Result = new UnauthorizedResult();
                return;
            }

            if (session.IsExpired())
            {
                filterContext.Result = new UnauthorizedResult();
                return;
            }

        }
        
    }
}