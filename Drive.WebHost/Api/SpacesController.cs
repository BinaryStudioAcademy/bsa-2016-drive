using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Drive.DataAccess.Entities;
using Drive.WebHost.Services;
using Driver.Shared.Dto;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/spaces")]
    public class SpacesController : ApiController
    {

        private readonly ISpaceService _spaceService;

        public SpacesController(ISpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var result = await _spaceService.GetAllAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSpace(int id)
        {
            var result = await _spaceService.GetAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> CreateSpace(SpaceDto space)
        {
            if (!ModelState.IsValid)
               return BadRequest();

            await _spaceService.CreateAsync(space);
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteSpace(int id)
        {
            var result = await _spaceService.GetAsync(id);

            if(result == null)
               return NotFound();

            await _spaceService.Delete(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateSpace(int id, SpaceDto space)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _spaceService.UpdateAsync(id, space);
            return Ok();
        }
    }
}