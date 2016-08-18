using Drive.DataAccess.Entities;
using Driver.Shared.Dto;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetAsync(int id);
        Task CreateAsync(UserDto dto);
        //Task UpdateAsync(int id, UserDto dto);
        //Task DeleteAsync(int id);
        Task<UserDto> GetLocalUser();
        void Dispose();
    }
}
