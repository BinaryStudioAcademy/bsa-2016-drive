using Drive.Identity.Services;
using Drive.WebHost.Services;
using Driver.Shared.Dto.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drive.WebHost.Providers
{
    public class FakeUserProvider : IUsersProvider
    {
        private readonly BSIdentityManager _bsIdentityManager;
        public FakeUserProvider( BSIdentityManager bsIdentityManager)
        {
            _bsIdentityManager = bsIdentityManager;
        }
        public Task<UserDto> GetByIdAsync(string id)
        {
            var user = new UserDto() { serverUserId = _bsIdentityManager.UserId, name = "TestName", surname="B" };
            TaskCompletionSource<UserDto> tcs = new TaskCompletionSource<UserDto>();
            tcs.SetResult(user);
            return tcs.Task;
        }
        public Task<IEnumerable<UsersDto>> GetAsync()
        {
            var userList = new List<UsersDto>();
            userList.Add(new UsersDto() { id = "56780659ea7a3b626282103d", name = "Eduard Dolynskyi" });
            userList.Add(new UsersDto() { id = "577a177413eb94e209af1ee4", name = "Tester C" });
            userList.Add(new UsersDto() { id = "567921371560298f766909a7", name = "Tester A" });
            TaskCompletionSource<IEnumerable<UsersDto>> tcs = new TaskCompletionSource<IEnumerable<UsersDto>>();
            tcs.SetResult(userList);
            return tcs.Task;
        }

        public Task<UserDto> GetCurrentUser()
        {
            return GetByIdAsync(_bsIdentityManager.UserId);
        }

        public string CurrentUserId
        {
            get { return _bsIdentityManager.UserId; }
        }
    }
}