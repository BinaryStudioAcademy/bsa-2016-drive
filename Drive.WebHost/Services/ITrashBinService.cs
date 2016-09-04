using Driver.Shared.Dto;
using Driver.Shared.Dto.TrashBin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface ITrashBinService
    {
        Task<IEnumerable<TrashBinDto>> GetTrashBinContentAsync();
        Task<IEnumerable<TrashBinDto>> SearchTrashBinAsync(string text);
        Task RestoreFileAsync(int id);
        Task DeleteFileAsync(int id);
        Task RestoreFolderAsync(int id);
        Task DeleteFolderAsync(int id);
        Task RestoreAllFromSpacesAsync(IEnumerable<int> spaces);
        Task ClearAllFromSpaceAsync(int spaceId);
        Task ClearTrashBinAsync();
    }
}