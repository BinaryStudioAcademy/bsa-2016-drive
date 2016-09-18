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

        // GET api/users/withoutcurrent
        [HttpGet]
        [Route("withoutcurrent")]
        public async Task<IHttpActionResult> GetAllWithoutCurrentAsync()
        {
            var users = await _usersService.GetAllWithoutCurrentAsync();

            if (users == null)
                return NotFound();
            return Ok(users);
        }

        // GET api/users/syncusers
        [HttpGet]
        [Route("syncusers")]

        public async Task<IHttpActionResult> SyncWithRemote()
        {
            var users = await _usersService.SyncWithRemoteUsers();

            if (users == null)
                return BadRequest();
            return Ok(users);
        }
        
    }
}
