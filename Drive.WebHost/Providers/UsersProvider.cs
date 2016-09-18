using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drive.Core.HttpClient;
using Drive.Identity.Services;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Providers
{
    public class UsersProvider : IUsersProvider
    {
        private readonly IAsyncHttpClient _client;
        private readonly BSIdentityManager _bsIdentityManager;
        public UsersProvider(IAsyncHttpClient client, BSIdentityManager bsIdentityManager)
        {
            _client = client;
            _bsIdentityManager = bsIdentityManager;
        }

        public async Task<UserDto> GetByIdAsync(string id)
        {
            return (await _client.GetAsync<IEnumerable<UserDto>>("profile/user/getByCentralId/" + id)).FirstOrDefault();
        }

        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            return await _client.GetAsync<List<UserDto>>("profile/user/filter");
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var userId = _bsIdentityManager.UserId;

            return await GetByIdAsync(userId);
        }

        public string CurrentUserId
        {
            get { return _bsIdentityManager.UserId; }
        }
    }
}