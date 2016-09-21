using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.WebHost.Services
{
    public interface ISharedByLinkService
    {
        Task<ShareLinkDto> GetShareLinkAsync(int sLinkId);
        Task<string> SetShareLinkAsync(CopyMoveContentDto content);
    }
}
