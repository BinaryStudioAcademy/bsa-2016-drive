using Drive.WebHost.Services;
using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/roles")]
    public class RolesController : ApiController
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            //var result = await _rolesService.GetAllAsync();
            List<UserDto> users = new List<UserDto>();
            UserDto user1 = new UserDto { id = "23kh25", name = "Nikita Krasnov", department = "Backend Developer"};
            UserDto user2 = new UserDto { id = "235nk5", name = "Anton Kumpan", department = "Frontend Developer" };
            UserDto user3 = new UserDto { id = "2n2kk2", name = "Irina Antonenko", department = "HR" };
            users.Add(user1);
            users.Add(user2);
            List<RoleDto> roles = new List<RoleDto>();
            RoleDto role1 = new RoleDto { Id = 1, Name = "HR", Description = "Hi! We are HRs.", Users = users };
            users.Add(user3);
            RoleDto role2 = new RoleDto { Id = 2, Name = "Developers", Description = "Hi! We are developers.", Users = users };
            roles.Add(role1);
            roles.Add(role2);
            var result = roles;
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetRole(int id)
        {
            //var result = await _rolesService.GetAsync(id);
            List<UserDto> users = new List<UserDto>();
            UserDto user1 = new UserDto { id = "23kh25", name = "Nikita Krasnov", department = "Backend Developer" };
            UserDto user2 = new UserDto { id = "235nk5", name = "Anton Kumpan", department = "Frontend Developer" };
            UserDto user3 = new UserDto { id = "2n2kk2", name = "Irina Antonenko", department = "HR" };
            users.Add(user1);
            users.Add(user2);
            RoleDto role = new RoleDto();
            if (id == 1)
            {
               role = new RoleDto { Id = 1, Name = "HR", Description = "Hi! We are HRs.", Users = users };
            }
            else
            {
                users.Add(user3);
                role = new RoleDto { Id = 2, Name = "Developers", Description = "Hi! We are developers.", Users = users };
            }
            var result = role;
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> CreateRole(RoleDto role)
        {
            int id = await _rolesService.CreateAsync(role);
            return Ok(id);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRole(int id)
        {
            var result = await _rolesService.GetAsync(id);

            if (result == null)
                return NotFound();

            await _rolesService.Delete(id);
            return Ok();
        }


        [HttpPut]
        public async Task<IHttpActionResult> UpdateRole(RoleDto role)
        {
            await _rolesService.UpdateAsync(role.Id, role);
            return Ok();
        }
    }
}