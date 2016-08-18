using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Drive.Identity.Entities;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersProvider _userProvider;

        public UsersService(IUnitOfWork unitOfWork, IUsersProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await _unitOfWork?.Users?.GetByIdAsync(id);
            return await _userProvider?.GetByIdAsync(user?.GlobalId);
        }

        public async Task<IEnumerable<UsersDto>> GetAllAsync()
        {
            return await _userProvider?.GetAsync();
        }

        public async Task CreateAsync(UserDto dto)
        {
            var user = await _unitOfWork.Users.Query.FirstOrDefaultAsync(u => u.GlobalId == dto.serverUserId);
            if (user == null)
            {
                _unitOfWork.Users.Create(new User() {GlobalId = dto.serverUserId, IsDeleted = false});
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<UserDto> GetCurrentUser()
        {
            return await _userProvider.GetCurrentUser();
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}