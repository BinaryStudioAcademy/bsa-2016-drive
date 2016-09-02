using Drive.WebHost.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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


        // GET: api/TrashBin
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var result = await _trashBinService.GetTrashBinContentAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
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
    }
}
