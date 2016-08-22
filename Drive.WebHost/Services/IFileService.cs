using System.Collections.Generic;
using System.Threading.Tasks;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;

namespace Drive.WebHost.Services
{
    public interface IFileService
    {
        Task<IEnumerable<FileUnitDto>> GetAllAsync();

        Task<IEnumerable<FileUnitDto>> GetAllByParentIdAsync(int spaceId, int? parentId);

        Task<FileUnitDto> GetAsync(int id);

        Task<FileUnitDto> CreateAsync(FileUnitDto dto);

        Task<FileUnitDto> UpdateAsync(int id, FileUnitDto dto);

        Task DeleteAsync(int id);

        Task<ICollection<AppDto>> FilterApp(FileType fileType);

        void Dispose();
    }
}