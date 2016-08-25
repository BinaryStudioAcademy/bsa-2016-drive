using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;
using System;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        private readonly IFileService _service;

        public FilesController(IFileService service)
        {
            _service = service;
        }

        // GET: api/files
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var file = await _service?.GetAllAsync();

            if (file == null || !file.Any())
                return NotFound();

            return Ok(file);
        }

        // GET: api/files/5
        [HttpGet]
        public async Task<IHttpActionResult> GetFileAsync(int id)
        {
            var file = await _service?.GetAsync(id);

            if (file == null)
                return NotFound();

            return Ok(file);
        }

        // GET: api/files/deleted/5
        [HttpGet]
        [Route("deleted/{id:int}")]
        public async Task<IHttpActionResult> GetFileDeletedAsync(int id)
        {
            var file = await _service?.GetDeletedAsync(id);

            if (file == null)
                return NotFound();

            return Ok(file);
        }

        // POST: api/files
        [HttpPost]
        public async Task<IHttpActionResult> CreateFileAsync(FileUnitDto file)
        {
            var dto = await _service?.CreateAsync(file);
            if (dto == null)
            {
                return BadRequest();
            }
            return Ok(dto);
        }

        // PUT: api/files/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateFileAsync(int id, FileUnitDto file)
        {
            var dto = await _service?.UpdateAsync(id, file);

            if (id != file.Id)
                return BadRequest();

            return Ok(dto);

        }

        // PUT: api/files/deleted/5?oldParentId=(int)
        [HttpPut]
        [Route("deleted/{id:int}")]
        public async Task<IHttpActionResult> UpdateDeletedFileAsync(int id, int? oldParentId, FileUnitDto file)
        {
            var dto = await _service.UpdateDeletedAsync(id, oldParentId, file);

            if (id != file.Id)
                return BadRequest();

            return Ok(dto);
        }

        // PUT: api/files/copied/5
        [HttpPut]
        [Route("copied/{id:int}")]
        public async Task<IHttpActionResult> CreateCopyFileAsync(int id, FileUnitDto file)
        {
            var dto = await _service.CreateCopyAsync(id, file);

            if (id != file.Id)
                return BadRequest();

            return Ok(dto);

        }

        // DELETE: api/files/5
        [HttpDelete]
        public IHttpActionResult DeleteFileAsync(int id)
        {
            _service?.DeleteAsync(id);

            return Ok();
        }

        // GET: api/files/app/fileType
        [HttpGet]
        [Route("apps/{fileType:alpha}")]
        public async Task<IHttpActionResult> FilterApp(string fileType)
        {
            FileType fileTypeEnum;
            if (!Enum.TryParse(fileType, true, out fileTypeEnum))
                return NotFound();
            var result = await _service.FilterApp(fileTypeEnum);
            if (result == null || result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        // GET: api/files?spaceId=(int)&parentId=(int)
        [Route("~/api/files/parent")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllByParentIdAsync(int spaceId, int? parentId)
        {
            var result = await _service.GetAllByParentIdAsync(spaceId, parentId);

            return Ok(result);
        }
    }
}