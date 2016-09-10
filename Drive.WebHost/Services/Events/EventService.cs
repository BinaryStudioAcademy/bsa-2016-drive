using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Driver.Shared.Dto.Events;

namespace Drive.WebHost.Services.Events
{
    public class EventService : IEventService
    {
        public Task<EventDto> CreateAsync(EventDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<EventDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<EventDto> UpdateAsync(int id, EventDto dto)
        {
            throw new NotImplementedException();
        }
    }
}