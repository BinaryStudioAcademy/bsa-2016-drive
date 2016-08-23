using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    [RoutePrefix("api/codesamples")]
    public class CodeSamplesController : ApiController
    {
        private readonly ICodeSamplesService _codeSamplesService;

        public CodeSamplesController(ICodeSamplesService codeSamplesService)
        {
            _codeSamplesService = codeSamplesService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses = await _codeSamplesService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _codeSamplesService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/codesamples
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(CodeSampleDto data)
        {
            var result = await _codeSamplesService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/codesamples/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, CodeSampleDto data)
        {

            var result = await _codeSamplesService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/codesamples/5
        [HttpDelete]
        public IHttpActionResult DeleteAsync(int id)
        {
            _codeSamplesService.DeleteAsync(id);

            return Ok();
        }
    }
}
