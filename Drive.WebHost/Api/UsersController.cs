using Drive.DataAccess.Entities;
using Drive.WebHost.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var users = await _usersService?.GetAllAsync();

            if (users == null)
                return NotFound();

            return Ok(users);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUserAsync(int id)
        {
            var user = await _usersService?.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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
