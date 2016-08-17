using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    [RoutePrefix("api/academypro")]
    public class AcademyProApiController : ApiController
    {
        private readonly IAcademyProCourseService _academyProCourseService;

        public AcademyProApiController(IAcademyProCourseService academyProCourseService)
        {
            _academyProCourseService = academyProCourseService;
        }

        public async Task<IHttpActionResult> GetAllAsync()
        {
            var courses =  await _academyProCourseService.GetAllAsync();
            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetCourceAsync(int id)
        {
            var result = await _academyProCourseService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/academypro
        [HttpPost]
        public async Task<IHttpActionResult> CreateFileAsync(AcademyProCourseDto data)
        {
            var result = await _academyProCourseService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/academypro/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateFileAsync(int id, AcademyProCourseDto data)
        {

            var result = await _academyProCourseService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/academypro/5
        [HttpDelete]
        public IHttpActionResult DeleteFileAsync(int id)
        {
            _academyProCourseService.DeleteAsync(id);

            return Ok();
        }
    }
}