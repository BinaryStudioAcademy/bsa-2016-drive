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
    public class TagsController : ApiController
    {
        private readonly ITagsService _tagsService;

        public TagsController(ITagsService tagsService)
        {
            _tagsService = tagsService;
        }

        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses = await _tagsService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCourceAsync(int id)
        {
            var result = await _tagsService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/tags
        [HttpPost]
        public async Task<IHttpActionResult> CreateFileAsync(TagDto data)
        {
            var result = await _tagsService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/tags/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateFileAsync(int id, TagDto data)
        {

            var result = await _tagsService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/tags/5
        [HttpDelete]
        public IHttpActionResult DeleteFileAsync(int id)
        {
            _tagsService.DeleteAsync(id);

            return Ok();
        }
    }
}
