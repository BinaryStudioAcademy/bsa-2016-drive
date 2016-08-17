using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    public class LectureController : ApiController
    {
        private readonly ILectureService _lectureService;

        public LectureController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses = await _lectureService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCourceAsync(int id)
        {
            var result = await _lectureService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/lecture
        [HttpPost]
        public async Task<IHttpActionResult> CreateFileAsync(LectureDto data)
        {
            var result = await _lectureService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/lecture/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateFileAsync(int id, LectureDto data)
        {

            var result = await _lectureService.UpdateAsync(id, data);

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
            _lectureService.DeleteAsync(id);

            return Ok();
        }
    }
}
