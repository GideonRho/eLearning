using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;

namespace WebAPI.Helpers
{
    public static class UserHelper
    {

        public static int GetUserId(this IPrincipal principal) => int.Parse(GetClaim(principal, ClaimTypes.NameIdentifier));

        public static string GetToken(this IPrincipal principal) => GetClaim(principal, ClaimTypes.Name);
        
        public static string GetClaim(this IPrincipal principal, string type)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity?.FindFirst(type);
            if (claim?.Value == null) throw new AuthenticationException($"Could not extract {type} from the given identity!");
            return claim.Value;
        }
        
    }
}