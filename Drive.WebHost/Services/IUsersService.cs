using Driver.Shared.Dto.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetAsync(int id);
        Task CreateAsync(UserDto dto);
        Task<UserDto> GetCurrentUser();
        Task<IEnumerable<UserDto>> GetAllWithoutCurrentAsync();

        Task<IEnumerable<UserDto>> SyncWithRemoteUsers();
        void Dispose();
        string CurrentUserId { get; }
    }
}
