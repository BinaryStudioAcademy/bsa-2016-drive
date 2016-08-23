using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    [RoutePrefix("api/contentlinks")]
    public class ContentLinksController : ApiController
    {
        private readonly IContentLinkService _contentLinkService;

        public ContentLinksController(IContentLinkService contentLinkService)
        {
            _contentLinkService = contentLinkService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses = await _contentLinkService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _contentLinkService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/contentlinks
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(ContentLinkDto data)
        {
            var result = await _contentLinkService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/contentlinks/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, ContentLinkDto data)
        {

            var result = await _contentLinkService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/contentlinks/5
        [HttpDelete]
        public IHttpActionResult DeleteAsync(int id)
        {
            _contentLinkService.DeleteAsync(id);

            return Ok();
        }
    }
}
