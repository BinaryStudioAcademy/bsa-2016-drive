using System.Collections.Generic;
using System.Threading.Tasks;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public interface IFileService
    {
        Task<IEnumerable<FileUnitDto>> GetAllAsync();
        Task<FileUnitDto> GetAsync(int id);
        Task<int> CreateAsync(FileUnitDto dto);
        Task UpdateAsync(int id, FileUnitDto dto);
        Task DeleteAsync(int id);
        void Dispose();
    }
}