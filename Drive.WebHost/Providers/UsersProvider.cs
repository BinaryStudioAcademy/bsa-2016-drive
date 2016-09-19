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
            var remoteUser = (await _client.GetAsync<IEnumerable<RemoteUserDto>>("profile/user/getByCentralId/" + id)).FirstOrDefault();
            return remoteUser != null ? ConvertFromRemoteToLocalUser(remoteUser) : null;
        }

        public async Task<IEnumerable<UserDto>> GetAsync()
        {
            var remoteuserlist = await _client.GetAsync<List<RemoteUserDto>>("profile/user/filter");
            return remoteuserlist?.Select(x => ConvertFromRemoteToLocalUser(x));

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

        private UserDto ConvertFromRemoteToLocalUser(RemoteUserDto rUser)
        {
            var user = new UserDto { id = rUser.serverUserId, name = rUser.name, surname = rUser.surname };
            return user;
        }
    }
}