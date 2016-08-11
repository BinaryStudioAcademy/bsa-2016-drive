using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Drive.Core.HttpClient;
using System.Net.Http;
using Newtonsoft.Json;

namespace Drive.WebHost.Services
{
    public class UsersProvider : IUsersProvider
    {
        private readonly IHttpClient _client;

        public UsersProvider(IHttpClient client)
        {
            _client = client;
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            return new UserDto();
        }

        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            var response = await _client.GetAsync("");
            var result = JsonConvert.DeserializeObject<List<UserDto>>(response);
            return result;
        }
    }
}