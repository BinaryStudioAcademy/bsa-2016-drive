using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public interface IFolderService
    {
        Task<IEnumerable<FolderUnitDto>> GetAllAsync();

        Task<FolderUnitDto> GetAsync(int id);

        Task<int> CreateAsync(FolderUnitDto folder);

        Task UpdateAsync(int id, FolderUnitDto folder);

        Task DeleteAsync(int id);

        void Dispose();
    }
}
