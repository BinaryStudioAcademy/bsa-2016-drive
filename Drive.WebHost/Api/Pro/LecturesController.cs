using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    [RoutePrefix("api/lectures")]
    public class LecturesController : ApiController
    {
        private readonly ILectureService _lectureService;

        public LecturesController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses = await _lectureService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _lectureService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/lectures
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(LectureDto data)
        {
            var result = await _lectureService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/lectures/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, LectureDto data)
        {

            var result = await _lectureService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/lectures/5
        [HttpDelete]
        public IHttpActionResult DeleteAsync(int id)
        {
            _lectureService.DeleteAsync(id);

            return Ok();
        }
    }
}
