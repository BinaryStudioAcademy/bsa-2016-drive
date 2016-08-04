using Drive.Identity.Entities;
using Drive.Identity.Services.Abstract;

namespace Drive.Identity.Services
{
    public class BSIdentityManager : GenericIdentityManager<BSIdentity>
    {
        public string UserId => Identity.UserId;
        public string Token => Identity.Token;
        public string Name => Identity.Name;
        public bool IsAuthenticated => Identity.IsAuthenticated;
    }
}
