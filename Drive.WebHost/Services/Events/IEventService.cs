using Driver.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.WebHost.Services.Events
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllAsync();
        Task<EventDto> GetAsync(int id);
        Task<EventDto> CreateAsync(EventDto dto);
        Task<EventDto> UpdateAsync(int id, EventDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<AppsEventDto>> SearchEvents(string text);
    }
}
