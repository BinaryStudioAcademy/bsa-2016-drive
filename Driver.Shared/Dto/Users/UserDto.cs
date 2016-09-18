
using Newtonsoft.Json;

namespace Driver.Shared.Dto.Users
{
    public class UserDto
    {
        [JsonProperty(PropertyName="serverUserId")]
        public string id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }
}
