using System.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Drive.Identity.Entities;
using Drive.Identity.Services;
using Drive.Identity.Services.Abstract;

namespace Drive.WebHost.Filters
{
    public class JWTAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var httpCookie = filterContext.RequestContext.HttpContext.Request.Cookies["x-access-token"];
            var token = httpCookie?.Value;
            if (token != null)
            {
                var secret = ConfigurationManager.AppSettings["JWTSecret"];
                ITokenAuthenticationService authService = new JWTAuthService();
                var principal = authService.VerifyToken(token, secret);
                if (((BSIdentity) principal.Identity).IsExpired) return;
                var idManager = new BSIdentityManager();
                idManager.SetPrincipal(principal);
                filterContext.Principal = principal;
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}