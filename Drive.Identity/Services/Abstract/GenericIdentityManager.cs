using System.Security.Principal;
using System.Web;

namespace Drive.Identity.Services.Abstract
{
    public abstract class GenericIdentityManager<T> where T : IIdentity
    {
        public IPrincipal Principal { get { return HttpContext.Current.User; }  }
        public T Identity => (T)Principal.Identity;

        public void SetIdentity(T identity, string[] roles)
        {
            var principal = new GenericPrincipal(identity, roles);
            SetPrincipal(principal);
        }

        public void SetPrincipal(IPrincipal principal)
        {
            HttpContext.Current.User = principal;
        }
    }
}
