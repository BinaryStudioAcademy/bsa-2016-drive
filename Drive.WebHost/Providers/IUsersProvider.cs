using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Providers
{
    public interface IUsersProvider
    {
        Task<UserDto> GetByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetAsync();

        Task<UserDto> GetCurrentUser();

        string CurrentUserId { get; }
    }
}
