using System.Security.Principal;

namespace Drive.Identity.Services.Abstract
{
    public abstract class GenericIdentityManager<T> where T : IIdentity
    {
        public T Identity => (T)System.Threading.Thread.CurrentPrincipal.Identity;

        public void SetIdentity(T identity, string[] roles)
        {
            var principal = new GenericPrincipal(identity, roles);
            System.Threading.Thread.CurrentPrincipal = principal;
        }
    }
}
