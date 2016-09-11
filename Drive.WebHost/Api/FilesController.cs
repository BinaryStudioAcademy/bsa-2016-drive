using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Drive.WebHost.Services;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;
using System;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

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
            await _service.CreateCopyAsync(id, file);

            //if (id != file.Id)
            //    return BadRequest();

            return Ok();

        }

        // DELETE: api/files/5
        [HttpDelete]
        public IHttpActionResult DeleteFileAsync(int id)
        {
            _service?.DeleteAsync(id);

            return Ok();
        }

        // GET: api/files/apps/fileType
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


        // GET: api/files/apps/fileType/search
        [HttpGet]
        [Route("apps/{fileType:alpha}/search")]
        public async Task<IHttpActionResult> SearchFiles(string fileType, string text = "")
        {
            FileType fileTypeEnum;
            if (!Enum.TryParse(fileType, true, out fileTypeEnum))
            {
                return NotFound();
            }
            text = text == null ? string.Empty : text;

            var result = await _service.SearchFiles(fileTypeEnum, text);
            if (result == null || result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/files/apps/findcourse/5
        [HttpGet]
        [Route("apps/findcourse/{id:int}")]
        public async Task<IHttpActionResult> SearchCourse(int id)
        {
            int result = await _service.SearchCourse(id);
            if (result == 0)
            {
                return NotFound();
            }
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

        // POST: api/files/spaceId=(int)&folderId=(int?)
        [HttpPost]
        [Route("~/api/files/upload")]
        public async Task<IHttpActionResult> UploadFile(int spaceId, int? parentId )
        {
            int parent = parentId ?? 0;
            string result = "";
            HttpRequest request = HttpContext.Current.Request;
            try
            {
                foreach (string file in request.Files)
                {
                    var fileContent = request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        result = await _service?.UploadFile(fileContent, spaceId, parent);
                    }
                }
                return Ok(result);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }

        // GET: api/files/dowload&fileId=(string)
        [HttpGet]
        [Route("~/api/files/download")]
        public async Task<HttpResponseMessage> DownloadFile(string fileId)
        {
            HttpResponseMessage response;
            try
            {
                var dto = await _service?.DownloadFile(fileId);
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(dto.Content);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(dto.Type);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = dto.Name
                };

                return response;
            }
            catch (Exception)
            {
                return response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}