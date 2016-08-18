using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services;
using Driver.Shared.Dto;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/settings")]
    public class SettingsController : ApiController
    {
        private readonly ISettingsService _service;

        public SettingsController(ISettingsService service)
        {
            _service = service;
        }

        // GET api/settings/1
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var space = await _service?.GetAsync(id);

            if (space == null)
                return NotFound();

            return Ok(space);
        }

        //PUT api/settings/1
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, SettingsDto settings)
        {
            var dto = await _service?.UpdateAsync(id, settings);

            if (id != settings?.Id)
            {
                return BadRequest();
            }

            return Ok(dto);
        }
    }
}
