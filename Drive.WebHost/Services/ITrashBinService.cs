using Drive.DataAccess.Entities;
using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Drive.WebHost.Services
{
    public interface ITrashBinService
    {
        Task<IEnumerable<TrashBinDto>> GetTrashBinContentAsync();
        Task RestoreFileAsync(int id);
        Task DeleteFileAsync(int id);
    }
}