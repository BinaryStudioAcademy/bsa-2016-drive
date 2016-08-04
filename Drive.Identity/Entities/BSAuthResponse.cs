using System.Runtime.Serialization;

namespace Drive.Identity.Entities
{
    [DataContract]
    internal class BSAuthResponse
    {
        [DataMember(EmitDefaultValue = false, Name = "success")]
        public bool Success { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "message")]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "token")]
        public string Token { get; set; }
    }
}
