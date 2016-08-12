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
        private readonly IAsyncHttpClient _client;

        public UsersProvider(IAsyncHttpClient client)
        {
            _client = client;
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            return await _client.GetAsync<UserDto>("profile/api /user/public/" + id);
        }

        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            return await _client.GetAsync<List<UserDto>>("profile/user/filter");
        }
    }
}