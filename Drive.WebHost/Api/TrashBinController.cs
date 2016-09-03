using Drive.WebHost.Services;
using Driver.Shared.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/TrashBin")]
    public class TrashBinController : ApiController
    {
        private readonly ITrashBinService _trashBinService;
        public TrashBinController(ITrashBinService trashBinService)
        {
            _trashBinService = trashBinService;
        }

        //// GET: api/TrashBin
        //[HttpGet]
        //public async Task<IHttpActionResult> Get()
        //{
        //    var result = await _trashBinService.GetTrashBinContentAsync();

        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}

        // GET: api/TrashBin
        [HttpGet]
        public async Task<IHttpActionResult> SearchTrashBin(string text = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                var result = await _trashBinService.GetTrashBinContentAsync();
                return Ok(result);
            }
            else
            {
                var result = await _trashBinService.SearchTrashBinAsync(text);
                return Ok(result);
            }
        }

        // DELETE: api/TrashBin/File/{id}
        [HttpDelete]
        [Route("file/{id}")]
        public async Task<IHttpActionResult> DeleteFile(int id)
        {
            await _trashBinService.DeleteFileAsync(id);
            return Ok();
        }

        // DELETE: api/TrashBin/Folder/{id}
        [HttpDelete]
        [Route("folder/{id}")]
        public async Task<IHttpActionResult> DeleteFolder(int id)
        {
            await _trashBinService.DeleteFolderAsync(id);
            return Ok();
        }

        // PUT: api/TrashBin/File/{id}
        [HttpPut]
        [Route("file/{id}")]
        public async Task<IHttpActionResult> RestoreFile(int id)
        {
            await _trashBinService.RestoreFileAsync(id);
            return Ok();
        }

        // PUT: api/TrashBin/Folder/{id}
        [HttpPut]
        [Route("folder/{id}")]
        public async Task<IHttpActionResult> RestoreFolder(int id)
        {
            await _trashBinService.RestoreFolderAsync(id);
            return Ok();
        }

        // PUT: api.TrashBin/Spaces
        [HttpPut]
        [Route("spaces")]
        public async Task<IHttpActionResult> RestoreSpaces(List<TrashBinDto> spaces)
        {
            await _trashBinService.RestoreAllFromSpacesAsync(spaces);
            return Ok();
        }
    }
}
