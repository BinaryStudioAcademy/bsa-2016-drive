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
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return await _userProvider.GetByIdAsync(user.GlobalId);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _userProvider.GetAsync();
        }

        public async Task CreateAsync(UserDto dto)
        {
            var user = await _unitOfWork.Users.Query.FirstOrDefaultAsync(u => u.GlobalId == dto.id);
            if (user == null)
            {
                _unitOfWork.Users.Create(new User() {GlobalId = dto.id, IsDeleted = false});
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<User> GetLocalUser(IIdentity identity)
        {
            var userIdentity = (BSIdentity)identity;

            if (userIdentity != null)
            {
                return await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == userIdentity.UserId);
            }
            return null;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}