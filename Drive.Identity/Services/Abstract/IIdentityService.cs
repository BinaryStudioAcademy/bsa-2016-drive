using System.Security.Principal;
using System.Threading.Tasks;

namespace Drive.Identity.Services.Abstract
{
    public interface IIdentityService
    {
        Task<IIdentity> GetIdentityAsync(string login, string password);
    }
}
