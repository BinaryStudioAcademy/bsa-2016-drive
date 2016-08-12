using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface IUsersProvider
    {
        Task<UserDto> GetByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAsync();
    }
}
