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

        private IUnitOfWork _unitOfWork;

        public FoldersController(IFolderService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        //public FoldersController(IFolderService service)
        //{
        //    _service = service;
        //}


        // GET api/folders
        public async Task<IHttpActionResult> GetAllAsync()
        {
            //var data = await _service.GetAllAsync();

            return Ok();
        }

        // GET api/folders/1
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var folder = await _service.GetAsync(id);

            return Ok(folder);
        }

        // POST api/folders
        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync(FolderUnitDto folder)
        {
            await _service.CreateAsync(folder);

            return Ok();
        }

        //PUT api/folders/1
        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync(int id, FolderUnitDto folder)
        {
            await _service.UpdateAsync(folder);

            return Ok();
        }

        // DELETE api/folders/1
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
    }
}
