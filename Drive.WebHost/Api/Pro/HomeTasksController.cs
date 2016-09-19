using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Api.Pro
{
    [RoutePrefix("api/hometasks")]
    public class HomeTasksController : ApiController
    {
        private readonly IHomeTasksService _homeTasksService;

        public HomeTasksController(IHomeTasksService homeTasksService)
        {
            _homeTasksService = homeTasksService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var tasks = await _homeTasksService.GetAllAsync();
            if (tasks == null)
                return NotFound();

            return Ok(tasks);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _homeTasksService.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/hometasks
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(HomeTaskDto data)
        {
            var result = await _homeTasksService.CreateAsync(data);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }

        // PUT: api/hometasks/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, HomeTaskDto data)
        {

            var result = await _homeTasksService.UpdateAsync(id, data);

            if (id != result.Id)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        // DELETE: api/hometasks/5
        [HttpDelete]
        public IHttpActionResult DeleteAsync(int id)
        {
            _homeTasksService.DeleteAsync(id);

            return Ok();
        }
    }
}
