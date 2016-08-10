using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public interface ISpaceService
    {
        Task<IEnumerable<SpaceDto>> GetAllAsync();

        Task<SpaceDto> GetAsync(int id);

        Task<int> CreateAsync(SpaceDto space);

        Task UpdateAsync(int id, SpaceDto space);

        Task Delete(int id);

        void Dispose();
    }
}
