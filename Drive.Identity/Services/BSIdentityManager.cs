using Drive.Identity.Entities;
using Drive.Identity.Services.Abstract;

namespace Drive.Identity.Services
{
    public class BSIdentityManager : GenericIdentityManager<BSIdentity>
    {
        public string UserId => "577a16659829fe050adb3f5c";//Identity.UserId;

        public string Token
            =>
                "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6IjU3N2ExNjY1OTgyOWZlMDUwYWRiM2Y1YyIsImVtYWlsIjoidGVzdGVyX2FAZXhhbXBsZS5jb20iLCJyb2xlIjoiREVWRUxPUEVSIiwiaWF0IjoxNDcwOTA1MjczfQ.2I_Ml5jEfSG0W5czpC5mwoedrQm-uIiy5aFiqW38gRE"
            ; // Identity.Token;
        public string Name => Identity.Name;
        public string Email => Identity.Email;
        public string Role => Identity.Role;
        public bool IsAuthenticated => Identity.IsAuthenticated;
    }
}
