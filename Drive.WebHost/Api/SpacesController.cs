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


        // GET: api/spaces/(int)?folderId=(int?)&text=(string)&from=(int)&to=(int)
        [HttpGet]
        [Route("{spaceId:int}/search")]
        public async Task<IHttpActionResult> SearchFolderAndFile(int spaceId, string text, int page, int count, int? folderId = null)
        {
            var searchResultDto = await _spaceService.SearchFoldersAndFilesAsync(spaceId, folderId, text, page, count);

            if (searchResultDto == null || (searchResultDto.Files.Count == 0 && searchResultDto.Folders.Count == 0))
                return NotFound();
            return Ok(searchResultDto);
        }

        // GET: api/spaces/(int)?folderId=(int?)&text=(string)
        [HttpGet]
        [Route("{spaceId:int}/total")]
        public async Task<IHttpActionResult> NumberOfFoundFoldersAndFiles(int spaceId, string text, int? folderId = null)
        {
            int result = await _spaceService.NumberOfFoundFoldersAndFilesAsync(spaceId, folderId, text);
            if (result == 0)
                return NotFound();
            return Ok(result);
        }
    }
}