using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Drive.WebHost.Services
{
    public interface ISharedSpaceService
    {
        Task CreateOrUpdatePermissionsOfSharedDataAsync(List<UserSharedSpaceDto> users, int id);
        Task<IEnumerable<UserSharedSpaceDto>> GetPermissionsOfSharedDataAsync(int id);
        Task<SharedSpaceDto> GetAsync(int page, int count, string sort, int? folderId, int? parentFolderId);
        Task<int> GetTotalAsync(int? folderId = null, int? rootFolderId = null);
        Task<SharedSpaceDto> SearchAsync(string text, int page, int count, string sort, int? folderId, int? parentFolderId);
        Task<int> SearchTotalAsync(string text, int? folderId = null, int? rootFolderId = null);
        Task Delete(int id);

        void Dispose();
    }
}