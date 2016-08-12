using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Web;
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
                if (((BSIdentity)principal.Identity).IsExpired) return;
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
                var needAuth = bool.Parse(ConfigurationManager.AppSettings["NeedAuth"]);
                if (!needAuth) return;
                var authServer = ConfigurationManager.AppSettings["AuthServer"];
                if (filterContext.HttpContext.Request.Url != null)
                {
                    var url = HttpUtility.UrlDecode(filterContext.HttpContext.Request.Url.ToString());
                    var cookie = new HttpCookie("referer", url + "auth");
                    //filterContext.RequestContext.HttpContext.Response.Cookies.Add(cookie);
                    var myCookie = new CookieHeaderValue("referer", url + "auth");
                    filterContext.RequestContext.HttpContext.Response.Headers.Add("Set-Cookie", myCookie.ToString());
                }
                if (authServer != null) filterContext.Result = new RedirectResult(authServer);
            }
        }
    }
}