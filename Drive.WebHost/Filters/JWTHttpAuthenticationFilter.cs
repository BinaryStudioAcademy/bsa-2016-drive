using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Drive.Identity.Services;
using Drive.WebHost.Services;
using Ninject;

namespace Drive.WebHost.Filters
{
    public class JWTHttpAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var mockToken = bool.Parse(ConfigurationManager.AppSettings["MockToken"]);
            var requestCookies = context.Request.Headers.GetCookies("x-access-token").SingleOrDefault();
            var token = mockToken ? ConfigurationManager.AppSettings["TestToken"] : requestCookies?["x-access-token"].Value;

            if (string.IsNullOrWhiteSpace(token))
            {
                context.ErrorResult = new AuthenticationFailureResult(new
                {
                    Error = true,
                    Message = "Token is not provided"
                }, context.Request);
                return Task.FromResult(0);
            }

            
            try
            {
                IPrincipal principal = Authenticator.CreatePrincipal(token);
                var idManager = new BSIdentityManager();
                idManager.SetPrincipal(principal);
                context.Principal = principal;
            }

            catch (TokenExpiredException)
            {
                context.ErrorResult = new AuthenticationFailureResult(new
                {
                    Error = true,
                    Message = "Token is invalid"
                }, context.Request);
                return Task.FromResult(0);
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}