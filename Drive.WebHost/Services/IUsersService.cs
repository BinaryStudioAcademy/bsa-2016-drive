using Driver.Shared.Dto.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UsersDto>> GetAllAsync();
        Task<UserDto> GetAsync(int id);
        Task CreateAsync(UserDto dto);
        //Task UpdateAsync(int id, UserDto dto);
        //Task DeleteAsync(int id);
        Task<UserDto> GetCurrentUser();
        Task<IEnumerable<UsersDto>> GetAllWithoutCurrentAsync();
        void Dispose();
        string CurrentUserId { get; }
    }
}
