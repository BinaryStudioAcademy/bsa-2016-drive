using Driver.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.WebHost.Services.Events
{
    public interface IEventContentService
    {
        Task<IEnumerable<EventContentDto>> GetAllAsync();
        Task<EventContentDto> GetAsync(int id);
        Task<EventContentDto> CreateAsync(EventContentDto dto);
        Task<EventContentDto> UpdateAsync(int id, EventContentDto dto);
        Task DeleteAsync(int id);
    }
}
