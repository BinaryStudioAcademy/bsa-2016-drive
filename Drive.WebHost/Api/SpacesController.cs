using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Drive.DataAccess.Entities;
using Drive.WebHost.Services;

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
        public IHttpActionResult GetAll()
        {
            var result = _spaceService.SpaceRepository().GetAll();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetSpace(int id)
        {
            var result = _spaceService.SpaceRepository().GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        
        [HttpPost]
        public IHttpActionResult AddSpace(Space entity)
        {
            if (!ModelState.IsValid)
               return BadRequest();

            _spaceService.SpaceRepository().Create(entity);
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteSpace(int id)
        {
            var result = _spaceService.SpaceRepository().GetById(id);

            if(result == null)
               return NotFound();

            _spaceService.SpaceRepository().Delete(id);
            return Ok();
        }

        [HttpPut]
        public IHttpActionResult UpdateSpace(Space entity)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _spaceService.SpaceRepository().Update(entity);
            return Ok();
        }
    }
}