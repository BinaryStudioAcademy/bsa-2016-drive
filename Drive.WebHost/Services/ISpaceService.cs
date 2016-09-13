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
        Task<IList<SpaceDto>> GetAllAsync();

        Task<SpaceDto> GetAsync(int id);
        Task<SpaceDto> GetAsync(int id, int page, int count, string sotr);
        Task<int> GetSpaceByTypeAsync(SpaceType type);
        Task<int> GetTotalAsync(int id);

        Task<int> CreateAsync(SpaceDto space);

        Task UpdateAsync(int id, SpaceDto space);

        Task Delete(int id);
        Task DeleteWithStaff(int id);
        Task<SearchResultDto> SearchFoldersAndFilesAsync(int spaceId, int? folderId, string text, int page, int count);
        Task<int> NumberOfFoundFoldersAndFilesAsync(int spaceId, int? folderId, string text);

        Task CreateUserAndFirstSpaceAsync(string globalId);
        Task MoveContentAsync(CopyMoveContentDto content);
        Task CopyContentAsync(CopyMoveContentDto content);
        void Dispose();
    }
}
