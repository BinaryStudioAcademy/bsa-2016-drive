using System.Security.Principal;

namespace Drive.Identity.Services.Abstract
{
    public abstract class GenericIdentityManager<T> where T : IIdentity
    {
        public IPrincipal Principal = System.Threading.Thread.CurrentPrincipal;
        public T Identity => (T)Principal.Identity;

        public void SetIdentity(T identity, string[] roles)
        {
            var principal = new GenericPrincipal(identity, roles);
            SetPrincipal(principal);
        }

        public void SetPrincipal(IPrincipal principal)
        {
            System.Threading.Thread.CurrentPrincipal = principal;
        }
    }
}
