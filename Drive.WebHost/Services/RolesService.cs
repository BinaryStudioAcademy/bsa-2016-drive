using Drive.Core.HttpClient;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Drive.WebHost.Services
{
    public class RolesService : IRolesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncHttpClient _httpClient;

        public RolesService(IUnitOfWork unitOfWork, IAsyncHttpClient httpClient)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
        }

        public async Task<RoleDto> GetAsync(int id)
        {
            var data = await _unitOfWork.Roles.GetByIdAsync(id);

            return new RoleDto
            {
                Name = data.Name,
                Description = data.Description,
                Id = data.Id,
                Users = from d in data.Users
                        select new UserDto
                        {
                            LocalId = d.Id,
                            id = d.GlobalId,
                        }
            };
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var data = await _unitOfWork.Roles.GetAllAsync();

            var dto = from d in data
                      select new RoleDto
                      {
                          Name = d.Name,
                          Description = d.Description,
                          Id = d.Id,
                          Users = from user in d.Users
                                  select new UserDto
                                  {
                                      LocalId = user.Id,
                                      id = user.GlobalId,
                                  }
                      };
            return dto;
        }

        public async Task<int> CreateAsync(RoleDto dto)
        {
            var role = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                IsDeleted = false
            };
            _unitOfWork.Roles.Create(role);
            await _unitOfWork.SaveChangesAsync();
            return role.Id;
        }

        public async Task UpdateAsync(int id, RoleDto dto)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);

            role.Name = dto.Name;
            role.Description = dto.Description;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Roles.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}