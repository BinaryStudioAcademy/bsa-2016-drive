using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Services
{
    public interface IUsersProvider
    {
        Task<UserDto> GetByIdAsync(string id);
        Task<IEnumerable<UsersDto>> GetAsync();

        Task<UserDto> GetCurrentUser();
    }
}
