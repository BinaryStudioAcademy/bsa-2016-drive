using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;

namespace Drive.WebHost.Services
{
    public interface IFolderService
    {
        Task<IEnumerable<FolderUnit>> GetAllAsync();

        Task<FolderUnit> GetAsync(int id);

        Task CreateAsync(FolderUnit folder);

        Task UpdateAsync(FolderUnit folder);

        Task DeleteAsync(int id);

        void Dispose();
    }
}
