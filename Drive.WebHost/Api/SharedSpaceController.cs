using Drive.WebHost.Services;
using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/sharedspace")]
    public class SharedSpaceController : ApiController
    {
        private readonly ISharedSpaceService _sharedSpaceService;

        public SharedSpaceController(ISharedSpaceService sharedSpaceService)
        {
            _sharedSpaceService = sharedSpaceService;
        }

        // DELETE: api/sharedspace/?id=(int)
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteSharedData(int id)
        {
            await _sharedSpaceService.Delete(id);
            return Ok();
        }

        //GET: api/sharedspace/permission?id=(int)
        [HttpGet]
        [Route("permission")]
        public async Task<IHttpActionResult> GetPermissonsOfSharedData(int id)
        {
            var result = await _sharedSpaceService.GetPermissionsOfSharedDataAsync(id);
            if (result == null || result.Count() == 0)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/sharedspace/permission?users=(List<UserSharedSpaceDto>)&id=(int)
        [HttpPost]
        [Route("permission")]
        public async Task<IHttpActionResult> CreateOrUpdatePermissionsOfSharedData(List<UserSharedSpaceDto> users, int id)
        {
            await _sharedSpaceService.CreateOrUpdatePermissionsOfSharedDataAsync(users, id);
            return Ok();
        }

        // GET: api/sharedspace/?page=(int)&count=(int)&sort=(string)&folderId=(int?)&rootFolderId=(int?)
        [HttpGet]
        public async Task<IHttpActionResult> GetSharedData(int page = 1, int count = 100, string sort = null, int? folderId = null, int? rootFolderId = null)
        {
            var result = await _sharedSpaceService.GetAsync(page, count, sort, folderId, rootFolderId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        //GET: api/sharedspace/total?folderId=(int?)&rootFolderId=(int?)
        [HttpGet]
        [Route("total")]
        public async Task<IHttpActionResult> GetSharedSpaceTotal(int? folderId = null, int? rootFolderId = null)
        {
            var result = await _sharedSpaceService.GetTotalAsync(folderId, rootFolderId);
            if (result == 0)
                return NotFound();
            return Ok(result);
        }

        // GET: api/sharedspace/search?text=(string)&page=(int)&count=(int)&sort=(string)&folderId=(int?)&rootFolderId=(int?)
        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> SearchInSharedSpace(string text = "", int page = 1, int count = 100, string sort = null, int? folderId = null, int? rootFolderId = null)
        {
            var result = await _sharedSpaceService.SearchAsync(text, page, count, sort, folderId, rootFolderId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // GET: api/sharedspace/searchtotal?text=(string)&folderId=(int?)&rootFolderId=(int?)
        [HttpGet]
        [Route("searchtotal")]
        public async Task<IHttpActionResult> SearchTotalInSharedSpace(string text = "", int? folderId = null, int? rootFolderId = null)
        {
            var result = await _sharedSpaceService.SearchTotalAsync(text, folderId, rootFolderId);
            if (result == 0)
                return NotFound();
            return Ok(result);
        }


    }
}
