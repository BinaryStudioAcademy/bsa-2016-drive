using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Drive.Identity.Entities;
using Drive.Identity.Services;
using Drive.Identity.Services.Abstract;

namespace Drive.WebHost.Filters
{
    public class Authenticator
    {
        public static IPrincipal CreatePrincipal(string token)
        {
            var secret = ConfigurationManager.AppSettings["JWTSecret"];
            ITokenAuthenticationService authService = new JWTAuthService();
            var principal = authService.VerifyToken(token, secret);
            var checkExpiracy = bool.Parse(ConfigurationManager.AppSettings["CheckExpiracy"]);
            if (checkExpiracy && ((BSIdentity)principal.Identity).IsExpired)
            {
                throw new TokenExpiredException("token expired");
            }
            return principal;
        }
    }
}