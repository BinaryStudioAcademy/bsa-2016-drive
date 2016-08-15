using System.Collections.Generic;
using System.Threading.Tasks;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public interface IFileService
    {
        Task<IEnumerable<FileUnitDto>> GetAllAsync();
        Task<FileUnitDto> GetAsync(int id);
        Task<FileUnitDto> CreateAsync(FileUnitDto dto);
        Task<FileUnitDto> UpdateAsync(int id, FileUnitDto dto);
        Task DeleteAsync(int id);
        void Dispose();
    }
}