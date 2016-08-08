using System.Security.Principal;
using Drive.Identity.Entities;
using Drive.Identity.Services.Abstract;
using JWT;

namespace Drive.Identity.Services
{
    public class JWTAuthService : ITokenAuthenticationService
    {
        public GenericPrincipal VerifyToken(string token, string secret)
        {
            try
            {
                JsonWebToken.JsonSerializer = new BSIdentitiySerializer();
                var identity = JsonWebToken.DecodeToObject<BSIdentity>(token, secret);
                identity.Token = token;
                GenericPrincipal principal = new GenericPrincipal(identity, new[] { identity.Role });
                return principal;
            }
            catch (SignatureVerificationException)
            {
                // TODO: Catch error
                return null;
            }
        }
    }
}
