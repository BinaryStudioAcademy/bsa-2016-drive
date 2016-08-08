using System;
using System.Runtime.Serialization;
using System.Security.Principal;
using Newtonsoft.Json;

namespace Drive.Identity.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BSIdentity : IIdentity
    {
        public string Name { get; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("iat")]
        public string IssuedAt { get; set; }

        [JsonProperty("id")]
        public string UserId { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        public string Token { get; set; }

        public bool IsAuthenticated { get; }
        public string AuthenticationType { get; }

        // TODO: Add fields for identity
    }
}
