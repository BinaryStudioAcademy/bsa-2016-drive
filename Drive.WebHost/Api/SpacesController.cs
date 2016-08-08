using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Drive.DataAccess.Entities;
using Drive.WebHost.Services;

namespace Drive.WebHost.Api
{
    public class SpacesController : ApiController
    {

        private readonly ISpaceService _spaceService;

        public SpacesController(ISpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [HttpGet]
        public IEnumerable<Space> GetAll()
        {
           return _spaceService.SpaceRepository().GetAll();
        }

        [HttpGet]
        public Space Get(int id)
        {
            return _spaceService.SpaceRepository().GetById(id);
        }
        
        [HttpPost]
        public void Add(Space entity)
        {
            _spaceService.SpaceRepository().Create(entity);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            _spaceService.SpaceRepository().Delete(id);
        }

        [HttpPut]
        public void Update(Space entity)
        {
            _spaceService.SpaceRepository().Update(entity);
        }
    }
}