using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Identity
{
    public class GenericIdentityManager<T> where T : IIdentity
    {
        public T Identity => (T)System.Threading.Thread.CurrentPrincipal.Identity;

        public void SetIdentity(T identity, string[] roles)
        {
            var principal = new GenericPrincipal(identity, roles);
            System.Threading.Thread.CurrentPrincipal = principal;
        }
    }
}
