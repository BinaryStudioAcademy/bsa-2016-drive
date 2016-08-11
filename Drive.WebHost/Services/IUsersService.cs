using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface IUsersService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetAsync(int id);
        //Task<int> CreateAsync(UserDto dto);
        //Task UpdateAsync(int id, UserDto dto);
        //Task DeleteAsync(int id);
        void Dispose();
    }
}
