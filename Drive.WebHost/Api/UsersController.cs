using Drive.DataAccess.Entities;
using System.Collections.Generic;
using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<User> users = new List<User>();

            if (users == null)
                return NotFound();

            return Ok(users);
        }

        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        [HttpDelete]
        public void Delete(int id)
        {
            
        }

        [HttpPut]
        public void Put(int id)
        {

        }

        [HttpPost]
        public void Post(int id)
        {

        }

        
    }
}
