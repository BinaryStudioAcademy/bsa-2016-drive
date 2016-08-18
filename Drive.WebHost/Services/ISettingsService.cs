using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public interface ISettingsService
    {
        Task<SettingsDto> GetAsync(int id);

        Task<SettingsDto> UpdateAsync(int id, SettingsDto dto);

        void Dispose();
    }
}
