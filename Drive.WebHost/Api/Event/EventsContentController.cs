using Drive.WebHost.Services.Events;
using Driver.Shared.Dto.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Drive.WebHost.Api.Events
{
    [RoutePrefix("api/eventscontent")]
    public class EventsContentController : ApiController
    {
        private readonly IEventContentService _eventContentService;
        public EventsContentController(IEventContentService eventContentService)
        {
            _eventContentService = eventContentService;
        }


        // GET: api/EventsContent
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var content = await _eventContentService.GetAllAsync();
            if (content == null)
                return NotFound();

            return Ok(content);
        }

        // GET: api/EventsContent/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _eventContentService.GetAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/EventsContent
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(EventContentDto data)
        {
            var result = await _eventContentService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/EventsContent/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, EventContentDto data)
        {

            var result = await _eventContentService.UpdateAsync(id, data);
            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/EventsContent{id}
        [HttpDelete]
        public IHttpActionResult DeleteAsync(int id)
        {
            _eventContentService.DeleteAsync(id);

            return Ok();
        }
    }
}
