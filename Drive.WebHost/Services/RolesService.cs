using Drive.Core.HttpClient;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;
using Driver.Shared.Dto.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Drive.WebHost.Services
{
    public class RolesService : IRolesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncHttpClient _httpClient;
        private readonly IUsersService _userService;

        public RolesService(IUnitOfWork unitOfWork, IAsyncHttpClient httpClient, IUsersService userService)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _userService = userService;
        }

        public async Task<RoleDto> GetAsync(int id)
        {
            var users = await _userService.GetAllAsync();
            var data = await _unitOfWork?.Roles?.Query.Include(x => x.Users).Where(x => x.Id == id).Select(s => new RoleDto
            {
                Id = s.Id,
                Description = s.Description,
                Name = s.Name,
                Users = s.Users
            }).SingleOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            var dto = await _unitOfWork.Roles.Query.Include(x => x.Users).Select(s => new RoleDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Users = s.Users
            }).ToListAsync();
            return dto;
        }

        public async Task<int> CreateAsync(RoleDto dto)
        {
            List<User> permittedUsers = new List<User>();
            foreach (var item in dto.Users)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                if (user == null)
                {
                    UserDto userdto = new UserDto();
                    userdto.serverUserId = item.GlobalId;
                    await _userService.CreateAsync(userdto);
                    var suser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                    permittedUsers.Add(suser);
                }
                else
                {
                    permittedUsers.Add(user);
                }
            }

            var role = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                IsDeleted = false,
                Users = permittedUsers
            };
            _unitOfWork.Roles.Create(role);
            await _unitOfWork.SaveChangesAsync();
            return role.Id;
        }

        public async Task UpdateAsync(int id, RoleDto dto)
        {
            List<User> permittedUsers = new List<User>();
            foreach (var item in dto.Users)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                if (user == null)
                {
                    UserDto userdto = new UserDto();
                    userdto.serverUserId = item.GlobalId;
                    await _userService.CreateAsync(userdto);
                    var suser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                    permittedUsers.Add(suser);
                }
                else
                {
                    permittedUsers.Add(user);
                }
            }
            var role = await _unitOfWork?.Roles?.Query.Include(x => x.Users).SingleOrDefaultAsync(x => x.Id == id);
            role.Name = dto.Name;
            role.Description = dto.Description;
            role.Users = permittedUsers;
            await _unitOfWork?.SaveChangesAsync();
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