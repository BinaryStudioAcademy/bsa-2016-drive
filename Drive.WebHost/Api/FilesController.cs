using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services;
using Driver.Shared.Dto;

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
            var file = await _service.GetAllAsync();

            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        // GET: api/files/5
        [HttpGet]
        public async Task<IHttpActionResult> GetFileAsync(int id)
        {
            var file = await _service.GetAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        // POST: api/files
        [HttpPost]
        public async Task<IHttpActionResult> CreateFileAsync(FileUnitDto file)
        {
            var dto = await _service.CreateAsync(file);

            return Ok(dto);
        }

        // PUT: api/files/5
        [HttpPut]
        public async Task<IHttpActionResult> UpdateFileAsync(int id, FileUnitDto file)
        {
            var dto = await _service.UpdateAsync(id, file);

            return Ok(dto);
        }

        // DELETE: api/files/5
        [HttpDelete]
        public IHttpActionResult DeleteFileAsync(int id)
        {
            _service.DeleteAsync(id);

            return Ok();
        }
    }
}