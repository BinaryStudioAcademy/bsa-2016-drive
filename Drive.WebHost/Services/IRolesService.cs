using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface IRolesService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();

        Task<RoleDto> GetAsync(int id);

        Task<int> CreateAsync(RoleDto role);

        Task UpdateAsync(int id, RoleDto role);

        Task Delete(int id);

        void Dispose();
    }
}
