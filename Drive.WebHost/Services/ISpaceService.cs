using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;

namespace Drive.WebHost.Services
{
    public interface ISpaceService
    {
        Task<IEnumerable<Space>> GetAllAsync();

        Task<Space> GetAsync(int id);

        Task CreateAsync(Space space);

        Task UpdateAsync(Space space);

        Task Delete(int id);

        void Dispose();
    }
}
