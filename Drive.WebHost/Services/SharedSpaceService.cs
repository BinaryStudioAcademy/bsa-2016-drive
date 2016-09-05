using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Driver.Shared.Dto;
using Drive.DataAccess.Interfaces;
using Drive.Logging;
using Drive.DataAccess.Entities;
using System.Data.Entity;
using Driver.Shared.Dto.Users;
using Drive.Identity.Services;

namespace Drive.WebHost.Services
{
    public class SharedSpaceService : ISharedSpaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUsersService _userService;
        private readonly ISpaceService _spaceService;

        public SharedSpaceService(IUnitOfWork unitOfWork, ILogger logger, IUsersService userService, ISpaceService spaceService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
            _spaceService = spaceService;
        }

        public async Task<IEnumerable<UserSharedSpaceDto>> GetPermissionsOfSharedDataAsync(int id)
        {
            var filePermission = await _unitOfWork.SharedSpace.Query
                .Where(f => f.Content.Id == id)
                .Select(f => new UserSharedSpaceDto {
                    GlobalId = f.User.GlobalId,
                    IsDeleted = f.IsDeleted,
                    CanModify = f.CanModify,
                    CanRead = f.CanRead                    
                }).ToListAsync();
            return filePermission;
        }

        public async Task CreateOrUpdatePermissionsOfSharedDataAsync(List<UserSharedSpaceDto> users, int id)
        {
            foreach (var user in users)
            {
                var fileShared = await _unitOfWork.SharedSpace.Query.Include(t => t.Content).Include(t => t.User).SingleOrDefaultAsync(f => f.Content.Id == id && f.User.GlobalId == user.GlobalId);
                
                if (fileShared == null)
                {
                    var fileSharedDeleted = await _unitOfWork.SharedSpace.Deleted.Include(t => t.Content).Include(t => t.User).SingleOrDefaultAsync(f => f.Content.Id == id && f.User.GlobalId == user.GlobalId);
                    if (fileSharedDeleted == null)
                    {
                        await _spaceService.CreateUserAndFirstSpaceAsync(user.GlobalId);
                        var userDb = await _unitOfWork.Users.Query.FirstOrDefaultAsync(x => x.GlobalId == user.GlobalId);
                        var file = await _unitOfWork.Files.Query.SingleOrDefaultAsync(f => f.Id == id);
                        if (file != null)
                        {
                            var newSharedFile = new Shared()
                            {
                                IsDeleted = user.IsDeleted,
                                Content = file,
                                User = userDb,
                                CanModify = user.CanModify,
                                CanRead = user.CanRead,
                            };
                            _unitOfWork.SharedSpace.Create(newSharedFile);
                        }
                    }
                    else
                    {
                        fileSharedDeleted.CanModify = user.CanModify;
                        fileSharedDeleted.CanRead = user.CanRead;
                        fileSharedDeleted.IsDeleted = user.IsDeleted;
                        if (!user.CanModify && !user.CanRead)
                            fileShared.IsDeleted = true;
                    }
                }
                else
                {
                    fileShared.CanModify = user.CanModify;
                    fileShared.CanRead = user.CanRead;
                    fileShared.IsDeleted = user.IsDeleted;
                    if (!user.CanModify && !user.CanRead)
                        fileShared.IsDeleted = true;
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<SharedSpaceDto> GetAsync(int page, int count, string sort)
        {
            string userId = _userService.CurrentUserId;
            IEnumerable<FileUnitDto> files = await _unitOfWork.SharedSpace.Query
                .Where(s=>!s.IsDeleted && !s.Content.IsDeleted && s.User.GlobalId == userId && s.Content is FileUnit).Select(f=> new FileUnitDto()
                {
                    Description = f.Content.Description,
                    FileType = (f.Content as FileUnit).FileType, 
                    Id = f.Content.Id,
                    IsDeleted = f.Content.IsDeleted,
                    Name = f.Content.Name,
                    CreatedAt = f.Content.CreatedAt,
                    Link = (f.Content as FileUnit).Link,
                    Author = new AuthorDto() { Id = f.Content.Owner.Id, GlobalId = f.Content.Owner.GlobalId }
                }).ToListAsync();

            if (sort != null && sort.Equals("asc"))
            {
                files = files.OrderBy(f => f.CreatedAt);
            }
            else if (sort != null && sort.Equals("desc"))
            {
                files = files.OrderByDescending(f => f.CreatedAt);
            }

            int skipCount = (page - 1) * count;
            files = files.Skip(skipCount).Take(count).ToList();
            

            var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });
            Parallel.ForEach(files, file =>
            {
                file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name;
            });
            return new SharedSpaceDto() { Files = files.ToList() };
        }

        public async Task<int> GetTotalAsync()
        {
            string userId = _userService.CurrentUserId;
            var filesCount = await _unitOfWork.SharedSpace.Query
                .Where(s => !s.IsDeleted && !s.Content.IsDeleted && s.User.GlobalId == userId)
                .CountAsync();
            return filesCount;
        }

        public async Task<SharedSpaceDto> SearchAsync(string text, int page, int count)
        {
            string userId = _userService.CurrentUserId;
            IEnumerable<FileUnitDto> files = await _unitOfWork.SharedSpace.Query
                .Where(s => !s.IsDeleted && !s.Content.IsDeleted && s.User.GlobalId == userId && s.Content is FileUnit)
                .Select(f => new FileUnitDto()
                {
                    Description = f.Content.Description,
                    FileType = (f.Content as FileUnit).FileType,
                    Id = f.Content.Id,
                    IsDeleted = f.Content.IsDeleted,
                    Name = f.Content.Name,
                    CreatedAt = f.Content.CreatedAt,
                    Link = (f.Content as FileUnit).Link,
                    Author = new AuthorDto() { Id = f.Content.Owner.Id, GlobalId = f.Content.Owner.GlobalId }
                }).ToListAsync();

            if (!string.IsNullOrEmpty(text))
            {
                files = files.Where(f => f.Name.ToLower().Contains(text.ToLower()));
            }

            int skipCount = (page - 1) * count;
            files = files.Skip(skipCount).Take(count).ToList();


            var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });
            Parallel.ForEach(files, file =>
            {
                file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name;
            });
            return new SharedSpaceDto() { Files = files.ToList() };

        }

        public async Task<int> SearchTotalAsync(string text)
        {
            string userId = _userService.CurrentUserId;
            // TODO delete field from select
            IEnumerable<FileUnitDto> files = await _unitOfWork.SharedSpace.Query
                .Where(s => !s.IsDeleted && !s.Content.IsDeleted && s.User.GlobalId == userId && s.Content is FileUnit)
                .Select(f => new FileUnitDto()
                {
                    Description = f.Content.Description,
                    FileType = (f.Content as FileUnit).FileType,
                    Id = f.Content.Id,
                    IsDeleted = f.Content.IsDeleted,
                    Name = f.Content.Name,
                    CreatedAt = f.Content.CreatedAt,
                    Link = (f.Content as FileUnit).Link,
                    Author = new AuthorDto() { Id = f.Content.Owner.Id, GlobalId = f.Content.Owner.GlobalId }
                }).ToListAsync();

            if (!string.IsNullOrEmpty(text))
            {
                files = files.Where(f => f.Name.ToLower().Contains(text.ToLower()));
            }
            return files.Count();
        }

        public async Task Delete(int id)
        {
            string userId = _userService.CurrentUserId;
            //TODO add include

            var file = await _unitOfWork.SharedSpace.Query.SingleOrDefaultAsync(f => f.Content.Id == id && f.User.GlobalId == userId);
            if (file != null)
            {
                file.IsDeleted = true;
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}