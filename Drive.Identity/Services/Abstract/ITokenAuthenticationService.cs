using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Identity.Services.Abstract
{
    public interface ITokenAuthenticationService
    {
        GenericPrincipal VerifyToken(string token, string secret);
    }
}
