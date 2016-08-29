using Drive.WebHost.Services;
using Driver.Shared.Dto;
using Driver.Shared.Dto.Users;
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
            var result = await _rolesService.GetAllAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetRole(int id)
        {
            var result = await _rolesService.GetAsync(id);
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
            var result = await _rolesService.UpdateAsync(role.Id, role);
            if (result == true)
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}