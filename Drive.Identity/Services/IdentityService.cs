using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace Drive.WebHost.Services
{
    public class IdentityService
    {
        public static string GetUsername()
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            IIdentity identity = principal == null ? null : principal.Identity;
            return identity == null ? null : identity.Name;
        }
        public static bool IsInRole(string role)
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            return principal == null ? false : principal.IsInRole(role);
        }
    }
}