using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Driver.Shared.Dto.Users;
using Drive.Logging;
using Drive.WebHost.Providers;

namespace Drive.WebHost.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersProvider _userProvider;
        private readonly ILogger _logger;

        public UsersService(IUnitOfWork unitOfWork, IUsersProvider userProvider, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _userProvider = userProvider;
            _logger = logger;
        }

        public async Task<UserDto> GetAsync(int id)
        {
            var user = await _unitOfWork?.Users?.GetByIdAsync(id);
            return await _userProvider?.GetByIdAsync(user?.GlobalId);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userProvider?.GetAsync();
            var localUsers = await _unitOfWork.Users.GetAllAsync();
            return users.Where(x => localUsers.SingleOrDefault(l => l.GlobalId == x.id) != null);
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

        public async Task<UserDto> GetCurrentUser()
        {
            return await _userProvider.GetCurrentUser();
        }

        public async Task<IEnumerable<UserDto>> SyncWithRemoteUsers()
        {
            try
            {
                var users = await _userProvider.GetAsync();

                foreach (var user in users)
                {
                    var localUser = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == user.id);
                    if (localUser == null)
                    {
                        var deletedUser = await _unitOfWork.Users.Deleted.SingleOrDefaultAsync(u => u.GlobalId == user.id);
                        if (deletedUser == null)
                        {
                            localUser = new User() { GlobalId = user.id, IsDeleted = false };
                            _unitOfWork.Users.Create(localUser);

                            var usersWithPermissions = new List<User>();
                            usersWithPermissions.Add(localUser);
                            _unitOfWork.Spaces.Create(new Space()
                            {
                                Name = "My Space",
                                Description = "My Space",
                                MaxFileSize = 1024,
                                MaxFilesQuantity = 100,
                                ModifyPermittedUsers = usersWithPermissions,
                                ReadPermittedUsers = usersWithPermissions,
                                IsDeleted = false,
                                CreatedAt = DateTime.UtcNow,
                                LastModified = DateTime.UtcNow,
                                Owner = localUser,
                                Type = SpaceType.MySpace
                            });
                        }
                        else
                        {
                            ChangeUserIsActive(deletedUser, false);                            
                        }
                    }
                }

                var localUsers = (await _unitOfWork.Users.GetAllAsync())?.Where(lu => users.Where(u => u.id == lu.GlobalId) == null);
                Parallel.ForEach(localUsers, lUser =>
                    ChangeUserIsActive(lUser, true));

                await _unitOfWork.SaveChangesAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
                return null;
            }
        }

        private void ChangeUserIsActive(User user,bool value)
        {
            user.IsDeleted = value;
            var spaces = _unitOfWork.Spaces.Deleted.Include(s => s.ContentList).Where(s => s.Owner.Id == user.Id);
            Parallel.ForEach(spaces, space =>
            {
                space.IsDeleted = false;
                Parallel.ForEach(space.ContentList, dataUnit =>
                    dataUnit.IsDeleted = value);
            });
        }
        public string CurrentUserId
        {
            get { return _userProvider.CurrentUserId; }
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

        public async Task<IEnumerable<UserDto>> GetAllWithoutCurrentAsync()
        {
            var users = await _userProvider?.GetAsync();
            if (users == null)
                return null;
            List<UserDto> usersList = users.ToList();
            var currentUser = _userProvider.CurrentUserId;
            for (int i = 0; i < usersList.Count(); i++)
            {
                if (usersList[i].id == currentUser)
                {
                    usersList.RemoveAt(i);
                    break;
                }
            }
            return usersList;
        }
    }
}