using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Misc
{
    public class LocalHostFilter : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            string remoteIp = context.HttpContext.Request.Headers["X-Forwarded-For"];
            if (remoteIp != null)
            {
                context.Result = new UnauthorizedResult();
                return; 
            }
            
            if (!IPAddress.IsLoopback(context.HttpContext.Connection.RemoteIpAddress)) {
                context.Result = new UnauthorizedResult();
                return;
            }
            base.OnActionExecuting(context);
        }
        
    }
}