using System.Collections.Generic;
using System.Threading.Tasks;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;
using System.Web;
using Driver.Shared.Dto.Pro;
using Driver.Shared.Dto.Events;

namespace Drive.WebHost.Services
{
    public interface IFileService
    {
        Task<IEnumerable<FileUnitDto>> GetAllAsync();

        Task<IEnumerable<FileUnitDto>> GetAllByParentIdAsync(int spaceId, int? parentId);

        Task<FileUnitDto> GetAsync(int id);

        Task<FileUnitDto> GetDeletedAsync(int id);

        Task<FileUnitDto> CreateAsync(FileUnitDto dto);

        Task<FileUnitDto> UpdateAsync(int id, FileUnitDto dto);

        Task<FileUnitDto> UpdateDeletedAsync(int id, int? oldParentId, FileUnitDto dto);
        Task CreateCopyAsync(int id, FileUnitDto dto);
        Task DeleteAsync(int id);

        Task<ICollection<AppDto>> FilterApp(FileType fileType);
        Task<ICollection<AppDto>> SearchFiles(FileType fileType, string text);
        Task<string> UploadFile(HttpPostedFile file, AdditionalData fileData, int spaceId, int folderId);
        Task<DownloadFileDto> DownloadFile(string spaceId);
        Task<AcademyProCourseDto> SearchCourse(int fileId);
        Task<EventDto> SearchEvent(int fileId);
        void Dispose();
    }
}