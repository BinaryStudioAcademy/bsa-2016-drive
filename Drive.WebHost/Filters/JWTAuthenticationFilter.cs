using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Drive.Identity.Services;
using Drive.WebHost.Services;
using Drive.Identity.Entities;

namespace Drive.WebHost.Filters
{
    public class JWTAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var mockToken = bool.Parse(ConfigurationManager.AppSettings["MockToken"]);
            var token = mockToken ? ConfigurationManager.AppSettings["TestToken"] :
                filterContext.RequestContext.HttpContext.Request.Cookies["x-access-token"]?.Value;

            if (string.IsNullOrWhiteSpace(token)) return;

            IPrincipal principal;
            try
            {
                principal = Authenticator.CreatePrincipal(token);
            }

            catch (TokenExpiredException)
            {
                var clearCookie = new HttpCookie("x-access-token", "") { Expires = DateTime.Now.AddDays(-1) };
                filterContext.RequestContext.HttpContext.Response.SetCookie(clearCookie);
                return;
            }

            var idManager = new BSIdentityManager();
            idManager.SetPrincipal(principal);
            filterContext.Principal = principal;
            filterContext.HttpContext.User = principal;

        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user != null && user.Identity.IsAuthenticated) return;
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (skipAuthorization)
                return;
            var needAuth = bool.Parse(ConfigurationManager.AppSettings["NeedAuth"]);
            if (!needAuth) return;
            var authServer = ConfigurationManager.AppSettings["AuthServer"];
            if (filterContext.HttpContext.Request.Url != null)
            {
                var url = HttpUtility.UrlDecode(filterContext.HttpContext.Request.Url.ToString());
                var myCookie = new HttpCookie("referer", url);
                filterContext.RequestContext.HttpContext.Response.SetCookie(myCookie);
            }
            if (authServer != null) filterContext.Result = new RedirectResult(authServer);
        }
    }
}