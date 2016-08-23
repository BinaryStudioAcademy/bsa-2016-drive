using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    [RoutePrefix("api/academies")]
    public class AcademiesController : ApiController
    {
        private readonly IAcademyProCourseService _academyProCourseService;

        public AcademiesController(IAcademyProCourseService academyProCourseService)
        {
            _academyProCourseService = academyProCourseService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses = await _academyProCourseService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _academyProCourseService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/academies
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(AcademyProCourseDto data)
        {
            var result = await _academyProCourseService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/academies/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, AcademyProCourseDto data)
        {

            var result = await _academyProCourseService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/academies/5
        [HttpDelete]
        public IHttpActionResult DeleteAsync(int id)
        {
            _academyProCourseService.DeleteAsync(id);

            return Ok();
        }
    }
}