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
            var result = await _spaceService?.GetAllAsync();

            if (result == null || result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSpace(int id)
        {
            var result = await _spaceService?.GetAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> CreateSpace(SpaceDto space)
        {
            int id = await _spaceService?.CreateAsync(space);
            return Ok(id);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteSpace(int id)
        {
            var result = await _spaceService?.GetAsync(id);

            if(result == null)
               return NotFound();

            await _spaceService?.Delete(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateSpace(int id, SpaceDto space)
        {
            await _spaceService?.UpdateAsync(id, space);
            return Ok();
        }


        // GET: api/spaces/(int)/search?folderId=(int?)&text=(string)&page=(int)&count=(int)
        [HttpGet]
        [Route("{spaceId:int}/search")]
        public async Task<IHttpActionResult> SearchFolderAndFile(int spaceId, string text = "", int page = 1, int count = 100, int? folderId = null)
        {
            text = text == null? string.Empty : text;
            var searchResultDto = await _spaceService?.SearchFoldersAndFilesAsync(spaceId, folderId, text, page, count);

            if (searchResultDto == null || (searchResultDto.Files.Count == 0 && searchResultDto.Folders.Count == 0))
                return NotFound();
            return Ok(searchResultDto);
        }

        // GET: api/spaces/(int)/total?folderId=(int?)&text=(string)
        [HttpGet]
        [Route("{spaceId:int}/total")]
        public async Task<IHttpActionResult> NumberOfFoundFoldersAndFiles(int spaceId, string text = "", int? folderId = null)
        {
            int result = await _spaceService?.NumberOfFoundFoldersAndFilesAsync(spaceId, folderId, text);
            if (result == 0)
                return NotFound();
            return Ok(result);
        }
    }
}