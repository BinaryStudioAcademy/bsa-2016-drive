using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Driver.Shared.Dto.Events;

namespace Drive.WebHost.Services.Events
{
    public class EventContentService : IEventContentService
    {
        public Task<EventContentDto> CreateAsync(EventContentDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventContentDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<EventContentDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<EventContentDto> UpdateAsync(int id, EventContentDto dto)
        {
            throw new NotImplementedException();
        }
    }
}