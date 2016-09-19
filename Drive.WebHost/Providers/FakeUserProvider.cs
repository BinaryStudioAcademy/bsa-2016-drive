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
            var user = new UserDto { id = _bsIdentityManager.UserId, name = "Tester", surname="C" };
            TaskCompletionSource<UserDto> tcs = new TaskCompletionSource<UserDto>();
            tcs.SetResult(user);
            return tcs.Task;
        }
        public Task<IEnumerable<UserDto>> GetAsync()
        {
            var userList = new List<UserDto>();
            userList.Add(new UserDto { id = "577a16659829fe050adb3f5c", name = "TestName A" });
            userList.Add(new UserDto { id = "577a171c9829fe050adb3f5d", name = "Tester B" });
            userList.Add(new UserDto { id = "577a17669829fe050adb3f5e", name = "Tester C" });
            userList.Add(new UserDto { id = "577a17a99829fe050adb3f5f", name = "Tester D" });
            userList.Add(new UserDto { id = "577a17df9829fe050adb3f60", name = "Tester E" });
            userList.Add(new UserDto { id = "577a18349829fe050adb3f61", name = "Tester F" });
            TaskCompletionSource<IEnumerable<UserDto>> tcs = new TaskCompletionSource<IEnumerable<UserDto>>();
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