using System.Security.Principal;

namespace Drive.Identity.Entities
{
    public class BSIdentity : IIdentity
    {
        public string Name { get; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string AuthenticationType { get; }
        public bool IsAuthenticated { get; }

        // TODO: Add fields for identity
    }
}
