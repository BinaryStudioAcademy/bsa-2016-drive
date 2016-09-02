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
        public int IssuedAt { get; set; }

        [JsonProperty("id")]
        public string UserId { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        public string Token { get; set; }

        public bool IsAuthenticated => !string.IsNullOrEmpty(UserId);

        public string AuthenticationType { get; }

        public bool IsExpired => Expired();

        private bool Expired()
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(IssuedAt).ToLocalTime();
            return dt.AddMinutes(1440) <= DateTime.Now;
        }
    }
}
