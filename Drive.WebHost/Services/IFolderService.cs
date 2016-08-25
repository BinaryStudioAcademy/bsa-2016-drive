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

        Task<IEnumerable<FolderUnitDto>> GetAllByParentIdAsync(int spaceId, int? parentId);

        Task<FolderUnitDto> GetAsync(int id);

        Task<FolderUnitDto> GetDeletedAsync(int id);

        Task<FolderUnitDto> CreateAsync(FolderUnitDto folder);

        Task<FolderUnitDto> UpdateAsync(int id, FolderUnitDto folder);

        Task<FolderUnitDto> UpdateDeletedAsync(int id, int? oldParentId, FolderUnitDto dto);

        Task DeleteAsync(int id);

        Task<FolderContentDto> GetContentAsync(int id, int page, int count, string sort);

        Task<FolderContentDto> GetContentAsync(int id);

        Task<int> GetContentTotalAsync(int id);

        void Dispose();
    }
}
