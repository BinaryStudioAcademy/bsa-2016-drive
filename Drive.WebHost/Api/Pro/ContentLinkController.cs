using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    public class ContentLinkController : ApiController
    {
        private readonly IContentLinkService _contentLinkService;

        public ContentLinkController(IContentLinkService contentLinkService)
        {
            _contentLinkService = contentLinkService;
        }

        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses = await _contentLinkService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCourceAsync(int id)
        {
            var result = await _contentLinkService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/lecture
        [HttpPost]
        public async Task<IHttpActionResult> CreateFileAsync(ContentLinkDto data)
        {
            var result = await _contentLinkService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/lecture/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateFileAsync(int id, ContentLinkDto data)
        {

            var result = await _contentLinkService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/lecture/5
        [HttpDelete]
        public IHttpActionResult DeleteFileAsync(int id)
        {
            _contentLinkService.DeleteAsync(id);

            return Ok();
        }
    }
}
