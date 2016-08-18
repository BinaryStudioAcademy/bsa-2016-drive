using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc.Filters;
using Drive.Identity.Entities;
using Drive.Identity.Services;
using Drive.Identity.Services.Abstract;
using FilterAttribute = System.Web.Mvc.FilterAttribute;
using RedirectResult = System.Web.Mvc.RedirectResult;

namespace Drive.WebHost.Filters
{
    public class JWTAuthenticationFilter : FilterAttribute, 
        System.Web.Mvc.Filters.IAuthenticationFilter, 
        System.Web.Http.Filters.IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var mockToken = bool.Parse(ConfigurationManager.AppSettings["MockToken"]);
            var token = mockToken ? ConfigurationManager.AppSettings["TestToken"] :
                filterContext.RequestContext.HttpContext.Request.Cookies["x-access-token"]?.Value;

            if (token != null)
            {
                var secret = ConfigurationManager.AppSettings["JWTSecret"];
                ITokenAuthenticationService authService = new JWTAuthService();
                var principal = authService.VerifyToken(token, secret);
                var checkExpiracy = bool.Parse(ConfigurationManager.AppSettings["CheckExpiracy"]);
                if (checkExpiracy && ((BSIdentity)principal.Identity).IsExpired)
                {
                    var clearCookie = new HttpCookie("x-access-token", "") { Expires = DateTime.Now.AddDays(-1) };
                    filterContext.RequestContext.HttpContext.Response.SetCookie(clearCookie);
                    return;
                }
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
                    var myCookie = new HttpCookie("referer", url);
                    filterContext.RequestContext.HttpContext.Response.SetCookie(myCookie);
                }
                if (authServer != null) filterContext.Result = new RedirectResult(authServer);
            }
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var mockToken = bool.Parse(ConfigurationManager.AppSettings["MockToken"]);
            var requestCookies = context.Request.Headers.GetCookies("x-access-token").SingleOrDefault();
            var token = mockToken ? ConfigurationManager.AppSettings["TestToken"] : requestCookies?["x-access-token"].Value;

            if (token != null)
            {
                var secret = ConfigurationManager.AppSettings["JWTSecret"];
                ITokenAuthenticationService authService = new JWTAuthService();
                var principal = authService.VerifyToken(token, secret);
                var checkExpiracy = bool.Parse(ConfigurationManager.AppSettings["CheckExpiracy"]);
                if (checkExpiracy && ((BSIdentity)principal.Identity).IsExpired)
                {
                    context.ErrorResult = new AuthenticationFailureResult(new
                    {
                        Error = true,
                        Message = "Token is invalid"
                    }, context.Request);
                }
                var idManager = new BSIdentityManager();
                idManager.SetPrincipal(principal);
                context.Principal = principal;
            }
            else
            {
                context.ErrorResult = new AuthenticationFailureResult(new
                {
                    Error = true,
                    Message = "No token provided"
                }, context.Request);
            }
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var needAuth = bool.Parse(ConfigurationManager.AppSettings["NeedAuth"]);
            if (needAuth)
            {
                var requestUrl = context.Request.RequestUri;
                string baseUrl = requestUrl.Scheme + "://" + requestUrl.Authority;
                context.Result = new System.Web.Http.Results.RedirectResult(new Uri(baseUrl), context.Request);
            }
            return Task.FromResult(0);
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(object jsonContent, HttpRequestMessage request)
        {
            JsonContent = jsonContent;
            Request = request;
        }

        public HttpRequestMessage Request { get; private set; }

        public object JsonContent { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = Request,
                Content = new ObjectContent(JsonContent.GetType(), JsonContent, new JsonMediaTypeFormatter())
            };
            return response;
        }
    }
}