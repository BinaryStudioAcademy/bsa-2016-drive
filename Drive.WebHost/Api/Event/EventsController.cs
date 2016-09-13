using Drive.WebHost.Services.Events;
using Driver.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/events")]
    public class EventsController : ApiController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }


        // GET: api/events
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var events = await _eventService.GetAllAsync();
            if (events == null)
                return NotFound();

            return Ok(events);
        }

        // GET: api/events/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _eventService.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/events
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(EventDto data)
        {
            var result = await _eventService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // PUT: api/events/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, EventDto data)
        {
            var result = await _eventService.UpdateAsync(id, data);
            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/events/{id}
        [HttpDelete]
        public IHttpActionResult DeleteAsync(int id)
        {
            _eventService.DeleteAsync(id);

            return Ok();
        }
    }
}
