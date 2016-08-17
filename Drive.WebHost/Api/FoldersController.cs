using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Drive.WebHost.Services;
using Driver.Shared.Dto;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/folders")]
    public class FoldersController : ApiController
    {
        private readonly IFolderService _service;

        public FoldersController(IFolderService service)
        {
            _service = service;
        }

        // GET api/folders
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var data = await _service?.GetAllAsync();

            if (data == null || !data.Any())
                return NotFound();

            return Ok(data);
        }

        // GET api/folders/1
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var folder = await _service?.GetAsync(id);

            if (folder == null)
                return NotFound();

            return Ok(folder);
        }

        // POST api/folders
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(FolderUnitDto folder)
        {
            var dto = await _service?.CreateAsync(folder);

            if (dto == null)
            {
                return BadRequest();
            }
            return Ok(dto);
        }

        //PUT api/folders/1
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, FolderUnitDto folder)
        {
            var dto = await _service?.UpdateAsync(id, folder);

            if (id != folder?.Id)
            {
                return BadRequest();
            }

            return Ok(dto);
        }

        // DELETE api/folders/1
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(int id)
        {
            await _service?.DeleteAsync(id);

            return Ok();
        }

        [Route("~/api/content/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetContentAsync(int id)
        {
            var dto = await _service.GetContentAsync(id);

            return Ok(dto);
        }
    }
}
