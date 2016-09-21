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
    [RoutePrefix("api/share")]
    public class SharedByLinkController : ApiController
    {
           
        private readonly ISharedByLinkService _sharedByLinkService;

        public SharedByLinkController(ISharedByLinkService sharedByLinkService)
        {
            _sharedByLinkService = sharedByLinkService;
        }

        // GET: api/share/getlink/5
        [HttpGet]
        [Route("getlink/{shareLinkId:int}")]
        public async Task<IHttpActionResult> GetShareLink(int shareLinkId)
        {
            var result = await _sharedByLinkService.GetShareLinkAsync(shareLinkId);
            return Ok(result);
        }

        //// PUT: api/share/getlinks
        //[HttpPut]
        //[Route("getlinks")]
        //public async Task<IHttpActionResult> GetShareLinks(CopyMoveContentDto content)
        //{
        //    var result = await _sharedByLinkService.GetShareLinksAsync(content);
        //    return Ok(result);
        //}

        // PUT: api/share/setlink
        [HttpPut]
        [Route("setlink")]
        public async Task<IHttpActionResult> SetShareLink(CopyMoveContentDto content)
        {
            string shareLink = await _sharedByLinkService.SetShareLinkAsync(content);
            return Ok(shareLink);
        }
    }
}
