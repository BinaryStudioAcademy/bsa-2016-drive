using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
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
