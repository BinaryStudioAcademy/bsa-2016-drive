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
            //var user = await _unitOfWork.Users.GetByIdAsync(id);
            var user = new User
            {
                Id = 1
            };
            var result = await _userProvider.GetByIdAsync(user.GlobalId);
            return result;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _userProvider.GetAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}