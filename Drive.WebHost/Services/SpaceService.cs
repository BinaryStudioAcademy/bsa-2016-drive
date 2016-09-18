using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;
using Drive.Logging;
using Driver.Shared.Dto.Users;
using Drive.Identity.Services;

namespace Drive.WebHost.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUsersService _userService;
        private readonly IRolesService _roleService;
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;

        public SpaceService(IUnitOfWork unitOfWork, ILogger logger, IUsersService userService, IFolderService folderService, IFileService fileService, IRolesService roleService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
            _roleService = roleService;
            _folderService = folderService;
            _fileService = fileService;
        }

        public async Task<SpaceDto> GetAsync(int id)
        {
            string userId = _userService.CurrentUserId;

            var space = await (from s in _unitOfWork.Spaces.Query
                               let userCanRead = s.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                               let roleCanRead = s.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                               let userCanModify = s.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                               let roleCanModify = s.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                               where s.Id == id
                                && (s.Type == SpaceType.BinarySpace
                                || s.Owner.GlobalId == userId
                                || userCanRead || roleCanRead
                                || userCanModify || roleCanModify)
                               select new SpaceDto
                               {
                                   Id = s.Id,
                                   Name = s.Name,
                                   Type = s.Type,
                                   Description = s.Description,
                                   Owner = s.Owner,
                                   CanModifySpace = s.Type == SpaceType.BinarySpace ?
                                       true : s.Owner.GlobalId == userId ?
                                           true : userCanModify ?
                                               true : roleCanModify ?
                                                   true : false,
                                   MaxFileSize = s.MaxFileSize,
                                   MaxFilesQuantity = s.MaxFilesQuantity,
                                   ReadPermittedUsers = s.ReadPermittedUsers,
                                   ModifyPermittedUsers = s.ModifyPermittedUsers,
                                   ReadPermittedRoles = s.ReadPermittedRoles,
                                   ModifyPermittedRoles = s.ModifyPermittedRoles,
                                   Files = s.ContentList.OfType<FileUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted)
                                        .Select(f => new FileUnitDto
                                        {
                                            Description = f.Description,
                                            FileType = f.FileType,
                                            Id = f.Id,
                                            IsDeleted = f.IsDeleted,
                                            Name = f.Name,
                                            CreatedAt = f.CreatedAt,
                                            Link = f.Link,
                                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                            CanRead = s.Type == SpaceType.BinarySpace ?
                                            true : s.Owner.GlobalId == userId ?
                                                true : f.Owner.GlobalId == userId ?
                                                   true : userCanRead ?
                                                       true : roleCanRead ?
                                                           true : false,
                                            CanModify = s.Type == SpaceType.BinarySpace ?
                                            true : s.Owner.GlobalId == userId ?
                                               true : f.Owner.GlobalId == userId ?
                                                    true : userCanModify ?
                                                       true : roleCanModify ?
                                                           true : false,
                                        }),
                                   Folders = s.ContentList.OfType<FolderUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted)
                                        .Select(f => new FolderUnitDto
                                        {
                                            Id = f.Id,
                                            Name = f.Name,
                                            Description = f.Description,
                                            CreatedAt = f.CreatedAt,
                                            IsDeleted = f.IsDeleted,
                                            SpaceId = f.Space.Id,
                                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                            CanRead = s.Type == SpaceType.BinarySpace ?
                                                true : s.Owner.GlobalId == userId ?
                                                    true : f.Owner.GlobalId == userId ?
                                                       true : userCanRead ?
                                                           true : roleCanRead ?
                                                               true : false,
                                            CanModify = s.Type == SpaceType.BinarySpace ?
                                                true : s.Owner.GlobalId == userId ?
                                                   true : f.Owner.GlobalId == userId ?
                                                        true : userCanModify ?
                                                           true : roleCanModify ?
                                                               true : false,
                                        })
                               }).SingleOrDefaultAsync();
            if (space == null)
                return null;

            var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

            Parallel.ForEach(space.Files,
                file => { file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name; });
            Parallel.ForEach(space.Folders,
                folder => { folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name; });
            return space;
        }

        public async Task<SpaceDto> GetAsync(int id, int page, int count, string sort)
        {
           string userId = _userService.CurrentUserId;

           var space = await (from s in _unitOfWork.Spaces.Query
                                let userCanRead = s.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                let roleCanRead = s.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                let userCanModify = s.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                let roleCanModify = s.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                where s.Id == id
                                 && (s.Type == SpaceType.BinarySpace
                                 || s.Owner.GlobalId == userId
                                 || userCanRead || roleCanRead
                                 || userCanModify || roleCanModify)
                                select new SpaceDto
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    Type = s.Type,
                                    Description = s.Description,
                                    Owner = s.Owner,
                                    CanModifySpace = s.Type == SpaceType.BinarySpace?
                                        true : s.Owner.GlobalId == userId ? 
                                            true : userCanModify ?
                                                true : roleCanModify ?
                                                    true : false,
                                    MaxFileSize = s.MaxFileSize,
                                    MaxFilesQuantity = s.MaxFilesQuantity,
                                    ReadPermittedUsers = s.ReadPermittedUsers,
                                    ModifyPermittedUsers = s.ModifyPermittedUsers,
                                    ReadPermittedRoles = s.ReadPermittedRoles,
                                    ModifyPermittedRoles = s.ModifyPermittedRoles,
                                    Files = s.ContentList.OfType<FileUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted)
                                         .Select(f => new FileUnitDto
                                         {
                                             Description = f.Description,
                                             FileType = f.FileType,
                                             Id = f.Id,
                                             IsDeleted = f.IsDeleted,
                                             Name = f.Name,
                                             CreatedAt = f.CreatedAt,
                                             Link = f.Link,
                                             Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                             CanRead = s.Type == SpaceType.BinarySpace ?
                                             true : s.Owner.GlobalId == userId ?
                                                 true : f.Owner.GlobalId == userId ?
                                                    true : userCanRead ?
                                                        true : roleCanRead ?
                                                            true : false,
                                             CanModify = s.Type == SpaceType.BinarySpace ?
                                             true : s.Owner.GlobalId == userId ?
                                                true : f.Owner.GlobalId == userId ? 
                                                     true : userCanModify? 
                                                        true : roleCanModify ? 
                                                            true : false
                                         }),
                                    Folders = s.ContentList.OfType<FolderUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted)
                                         .Select(f => new FolderUnitDto
                                         {
                                             Id = f.Id,
                                             Name = f.Name,
                                             Description = f.Description,
                                             CreatedAt = f.CreatedAt,
                                             IsDeleted = f.IsDeleted,
                                             SpaceId = f.Space.Id,
                                             Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                             CanRead = s.Type == SpaceType.BinarySpace ?
                                                 true : s.Owner.GlobalId == userId ?
                                                     true : f.Owner.GlobalId == userId ?
                                                        true : userCanRead ?
                                                            true : roleCanRead ?
                                                                true : false,
                                             CanModify = s.Type == SpaceType.BinarySpace ?
                                                 true : s.Owner.GlobalId == userId ?
                                                    true : f.Owner.GlobalId == userId ?
                                                         true : userCanModify ?
                                                            true : roleCanModify ?
                                                                true : false
                                         })
                                }).SingleOrDefaultAsync();


            if (space == null)
                return null;

            if (sort != null && sort.Equals("asc"))
            {
                var folders = space.Folders.OrderBy(f => f.CreatedAt);
                var files = space.Files.OrderBy(f => f.CreatedAt);

                space.Folders = folders;
                space.Files = files;
            }
            else if (sort != null && sort.Equals("desc"))
            {
                var folders = space.Folders.OrderByDescending(f => f.CreatedAt);
                var files = space.Files.OrderByDescending(f => f.CreatedAt);

                space.Folders = folders;
                space.Files = files;
            }

            int skipCount = (page - 1) * count;
            if (space.Folders.Count() <= skipCount)
            {
                skipCount -= space.Folders.Count();
                space.Folders = new List<FolderUnitDto>();
                space.Files = space.Files.Skip(skipCount).Take(count);
            }
            else
            {
                space.Folders = space.Folders.Skip(skipCount).Take(count);
                count -= space.Folders.Count();
                space.Files = space.Files.Take(count);
            }

            var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name }).ToList();
            var user = await _userService.GetCurrentUser();
            foreach (var item in space.Files)
            {
                if (item.Author.GlobalId == userId)
                {
                    owners.Add(new { Id = user.id, Name = user.name + user.surname });
                }
            }

            Parallel.ForEach(space.Files,
                file => { file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name; });
            Parallel.ForEach(space.Folders,
                folder => { folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name; });

            return space;
        }

        public async Task<int> GetSpaceByTypeAsync(SpaceType type)
        {
            string userId = _userService.CurrentUserId;

            var space = await _unitOfWork.Spaces.Query.Where(s => s.Type == type)
                                                      .Where(s => s.Type == SpaceType.BinarySpace || s.Owner.GlobalId == userId)
                                                      .Select(s => s.Id).SingleOrDefaultAsync();

            return space;
        }

        public async Task<int> GetTotalAsync(int id)
        {
            int counter = 0;
            string userId = _userService.CurrentUserId;

            var space = await (from s in _unitOfWork.Spaces.Query
                               let userCanRead = s.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                               let roleCanRead = s.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                               let userCanModify = s.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                               let roleCanModify = s.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                               where s.Id == id
                                && (s.Type == SpaceType.BinarySpace
                                || s.Owner.GlobalId == userId
                                || userCanRead || roleCanRead
                                || userCanModify || roleCanModify)
                               select new 
                               {
                                   Files = s.ContentList.OfType<FileUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted).Count(),
                                   Folders = s.ContentList.OfType<FolderUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted).Count()
                               }).SingleOrDefaultAsync();

            if (space == null)
                return 0;

            counter += space.Files;
            counter += space.Folders;
            return counter;
        }

        public async Task<IList<SpaceDto>> GetAllAsync()
        {
            string userId = _userService.CurrentUserId;

            var spacesList = await _unitOfWork.Spaces.Query.Include(x => x.ReadPermittedUsers).Include(x => x.ReadPermittedRoles).Select(s => new SpaceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ReadPermittedUsers = s.ReadPermittedUsers,
                ReadPermittedRoles = s.ReadPermittedRoles,
                Type = s.Type,
                Owner = s.Owner
            }).ToListAsync();


            for (int i = 0; i < spacesList.Count; i++)
            {
                if (spacesList[i].Type != SpaceType.BinarySpace
                     && spacesList[i].Owner.GlobalId != userId)
                {
                    if (spacesList[i].ReadPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) == null)
                    {
                        if (spacesList[i].ReadPermittedRoles.Count == 0)
                        {
                            spacesList.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            foreach (var item in spacesList[i].ReadPermittedRoles)
                            {
                                var role = await _roleService.GetAsync(item.Id);
                                if (role.Users.FirstOrDefault(x => x.GlobalId == userId) == null)
                                {
                                    spacesList.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }

            return spacesList;
        }

        public async Task<int> CreateAsync(SpaceDto dto)
        {
            string userId = _userService.CurrentUserId;
            User localUser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == userId);
            List<User> ReadPermittedUsers = new List<User>();
            foreach (var item in dto.ReadPermittedUsers)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                ReadPermittedUsers.Add(user);
            }

            List<User> ModifyPermittedUsers = new List<User>();
            foreach (var item in dto.ModifyPermittedUsers)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(u => u.GlobalId == item.GlobalId);
                ModifyPermittedUsers.Add(user);
                var x = ReadPermittedUsers.FirstOrDefault(p => p.GlobalId == user.GlobalId);
                if (x == null)
                {
                    ReadPermittedUsers.Add(user);
                }
            }

            List<Role> ReadPermittedRoles = new List<Role>();
            foreach (var item in dto.ReadPermittedRoles)
            {
                var role = await _unitOfWork?.Roles?.Query.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == item.Id);
                ReadPermittedRoles.Add(role);
            }

            List<Role> ModifyPermittedRoles = new List<Role>();
            foreach (var item in dto.ModifyPermittedRoles)
            {
                var role = await _unitOfWork?.Roles?.Query.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == item.Id);
                ModifyPermittedRoles.Add(role);
                var x = ReadPermittedRoles.FirstOrDefault(p => p.Id == role.Id);
                if (x == null)
                {
                    ReadPermittedRoles.Add(role);
                }
            }

            var space = new Space
            {
                Name = dto.Name,
                Type = SpaceType.OtherSpace,
                Description = dto.Description,
                MaxFilesQuantity = dto.MaxFilesQuantity,
                MaxFileSize = dto.MaxFileSize,
                ReadPermittedUsers = ReadPermittedUsers,
                ModifyPermittedUsers = ModifyPermittedUsers,
                ReadPermittedRoles = ReadPermittedRoles,
                ModifyPermittedRoles = ModifyPermittedRoles,
                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now,
                IsDeleted = false,
                Owner = await _unitOfWork.Users.Query.FirstOrDefaultAsync(u => u.GlobalId == userId)
            };
            _unitOfWork?.Spaces?.Create(space);
            await _unitOfWork?.SaveChangesAsync();
            return space.Id;
        }

        public async Task UpdateAsync(int id, SpaceDto dto)
        {
            var space =
                await
                    _unitOfWork?.Spaces?.Query.Include(x => x.ReadPermittedUsers)
                        .Include(x => x.ModifyPermittedUsers)
                        .Include(x => x.ReadPermittedRoles)
                        .Include(x => x.ModifyPermittedRoles)
                        .SingleOrDefaultAsync(x => x.Id == id);

            if (space == null) return;
            List<User> ReadPermittedUsers = new List<User>();
            foreach (var item in dto.ReadPermittedUsers)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                ReadPermittedUsers.Add(user);
            }
            List<User> ModifyPermittedUsers = new List<User>();
            foreach (var item in dto.ModifyPermittedUsers)
            {
            var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(u => u.GlobalId == item.GlobalId);
                ModifyPermittedUsers.Add(user);
                var x = ReadPermittedUsers.FirstOrDefault(p => p.GlobalId == user.GlobalId);
                if (x == null)
                {
                    ReadPermittedUsers.Add(user);
                }
            }
            List<Role> ReadPermittedRoles = new List<Role>();


            foreach (var item in dto.ReadPermittedRoles)
            {
                var role = await _unitOfWork?.Roles?.Query.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == item.Id);
                ReadPermittedRoles.Add(role);
            }

            List<Role> ModifyPermittedRoles = new List<Role>();
            foreach (var item in dto.ModifyPermittedRoles)
            {
                var role = await _unitOfWork?.Roles?.Query.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == item.Id);
                ModifyPermittedRoles.Add(role);
                var x = ReadPermittedRoles.FirstOrDefault(p => p.Id == role.Id);
                if (x == null)
                {
                    ReadPermittedRoles.Add(role);
                }
            }
            space.Name = dto.Name;
            space.Description = dto.Description;
            space.MaxFileSize = dto.MaxFileSize;
            space.MaxFilesQuantity = dto.MaxFilesQuantity;
            space.ReadPermittedUsers = ReadPermittedUsers;
            space.ModifyPermittedUsers = ModifyPermittedUsers;
            space.ReadPermittedRoles = ReadPermittedRoles;
            space.ModifyPermittedRoles = ModifyPermittedRoles;
            space.LastModified = DateTime.Now;

            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork?.Spaces?.Delete(id);
            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task DeleteWithStaff(int id)
        {
            SpaceDto spaceToDelete = await GetAsync(id);

            foreach (var folder in spaceToDelete.Folders)
            {
                await _folderService.DeleteAsync(folder.Id);
            }
            foreach (var file in spaceToDelete.Files)
            {
                await _fileService.DeleteAsync(file.Id);
            }

            _unitOfWork?.Spaces?.Delete(id);
            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task<SearchResultDto> SearchFoldersAndFilesAsync(int spaceId, int? folderId, string text, int page,
            int count)
        {
            string userId = _userService.CurrentUserId;

            IEnumerable<FolderUnitDto> resultFolders = new List<FolderUnitDto>();
            IEnumerable<FileUnitDto> resultFiles = new List<FileUnitDto>();
            bool canModifySpace = false;
            try
            {
                canModifySpace = await (from s in _unitOfWork.Spaces.Query
                                            let userCanRead = s.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                            let roleCanRead = s.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                            let userCanModify = s.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                            let roleCanModify = s.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                            where s.Id == spaceId
                                             && (s.Type == SpaceType.BinarySpace
                                             || s.Owner.GlobalId == userId
                                             || userCanRead || roleCanRead
                                             || userCanModify || roleCanModify)
                                            select s.Type == SpaceType.BinarySpace ?
                                                    true : s.Owner.GlobalId == userId ?
                                                        true : userCanModify ?
                                                            true : roleCanModify ?
                                                                true : false ).SingleAsync();

                if (folderId != null)
                {
                    resultFolders = await (from f in _unitOfWork.Folders.Query
                                               let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                               let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                               let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                               let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                               where f.FolderUnit.Id == folderId
                                                    && (f.Space.Type == SpaceType.BinarySpace
                                                    || f.Space.Owner.GlobalId == userId
                                                    || userCanRead || roleCanRead
                                                    || userCanModify || roleCanModify)
                                               select new FolderUnitDto
                                               {
                                                   Id = f.Id,
                                                   Name = f.Name,
                                                   Description = f.Description,
                                                   CreatedAt = f.CreatedAt,
                                                   IsDeleted = f.IsDeleted,
                                                   SpaceId = f.Space.Id,
                                                   Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                                   CanRead = f.Space.Type == SpaceType.BinarySpace ?
                                                   true : f.Space.Owner.GlobalId == userId ?
                                                       true : f.Owner.GlobalId == userId ?
                                                          true : userCanRead ?
                                                              true : roleCanRead ?
                                                                  true : false,
                                                   CanModify = f.Space.Type == SpaceType.BinarySpace ?
                                                   true : f.Space.Owner.GlobalId == userId ?
                                                      true : f.Owner.GlobalId == userId ?
                                                           true : userCanModify ?
                                                              true : roleCanModify ?
                                                                  true : false
                                               }).ToListAsync();

                    resultFiles = await (from f in _unitOfWork.Files.Query
                                            let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                            let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                            let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                            let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                            where f.FolderUnit.Id == folderId
                                                 && (f.Space.Type == SpaceType.BinarySpace
                                                 || f.Space.Owner.GlobalId == userId
                                                 || userCanRead || roleCanRead
                                                 || userCanModify || roleCanModify)
                                            select new FileUnitDto
                                            {
                                                Description = f.Description,
                                                FileType = f.FileType,
                                                Id = f.Id,
                                                IsDeleted = f.IsDeleted,
                                                Name = f.Name,
                                                CreatedAt = f.CreatedAt,
                                                Link = f.Link,
                                                Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                                CanRead = f.Space.Type == SpaceType.BinarySpace ?
                                                true : f.Space.Owner.GlobalId == userId ?
                                                    true : f.Owner.GlobalId == userId ?
                                                       true : userCanRead ?
                                                           true : roleCanRead ?
                                                               true : false,
                                                CanModify = f.Space.Type == SpaceType.BinarySpace ?
                                                true : f.Space.Owner.GlobalId == userId ?
                                                   true : f.Owner.GlobalId == userId ?
                                                        true : userCanModify ?
                                                           true : roleCanModify ?
                                                               true : false
                                            }).ToListAsync();

                }
                else
                {
                    resultFolders = await (from f in _unitOfWork.Folders.Query
                                               let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                               let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                               let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                               let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                               where f.Space.Id == spaceId && f.FolderUnit == null
                                                    && (f.Space.Type == SpaceType.BinarySpace
                                                    || f.Space.Owner.GlobalId == userId
                                                    || userCanRead || roleCanRead
                                                    || userCanModify || roleCanModify)
                                               select new FolderUnitDto
                                               {
                                                   Id = f.Id,
                                                   Name = f.Name,
                                                   Description = f.Description,
                                                   CreatedAt = f.CreatedAt,
                                                   IsDeleted = f.IsDeleted,
                                                   SpaceId = f.Space.Id,
                                                   Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                                   CanRead = f.Space.Type == SpaceType.BinarySpace ?
                                                   true : f.Space.Owner.GlobalId == userId ?
                                                       true : f.Owner.GlobalId == userId ?
                                                          true : userCanRead ?
                                                              true : roleCanRead ?
                                                                  true : false,
                                                   CanModify = f.Space.Type == SpaceType.BinarySpace ?
                                                   true : f.Space.Owner.GlobalId == userId ?
                                                      true : f.Owner.GlobalId == userId ?
                                                           true : userCanModify ?
                                                              true : roleCanModify ?
                                                                  true : false
                                               }).ToListAsync();

                    resultFiles = await (from f in _unitOfWork.Files.Query
                                              let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                              let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                              let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                              let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                              where f.Space.Id == spaceId && f.FolderUnit == null
                                                   && (f.Space.Type == SpaceType.BinarySpace
                                                   || f.Space.Owner.GlobalId == userId
                                                   || userCanRead || roleCanRead
                                                   || userCanModify || roleCanModify)
                                              select new FileUnitDto
                                              {
                                                  Description = f.Description,
                                                  FileType = f.FileType,
                                                  Id = f.Id,
                                                  IsDeleted = f.IsDeleted,
                                                  Name = f.Name,
                                                  CreatedAt = f.CreatedAt,
                                                  Link = f.Link,
                                                  Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                                                  CanRead = f.Space.Type == SpaceType.BinarySpace ?
                                                  true : f.Space.Owner.GlobalId == userId ?
                                                      true : f.Owner.GlobalId == userId ?
                                                         true : userCanRead ?
                                                             true : roleCanRead ?
                                                                 true : false,
                                                  CanModify = f.Space.Type == SpaceType.BinarySpace ?
                                                  true : f.Space.Owner.GlobalId == userId ?
                                                     true : f.Owner.GlobalId == userId ?
                                                          true : userCanModify ?
                                                             true : roleCanModify ?
                                                                 true : false
                                              }).ToListAsync();
                }
                if (!string.IsNullOrEmpty(text))
                {
                    resultFiles = resultFiles.Where(f => f.Name.ToLower().Contains(text.ToLower()));
                    resultFolders = resultFolders.Where(f => f.Name.ToLower().Contains(text.ToLower()));
                }

                int skipCount = (page - 1) * count;
                if (resultFolders.Count() <= skipCount)
                {
                    skipCount -= resultFolders.Count();
                    resultFolders = new List<FolderUnitDto>();
                    resultFiles = resultFiles.Skip(skipCount).Take(count);
                }
                else
                {
                    resultFolders = resultFolders.Skip(skipCount).Take(count);
                    count -= resultFolders.Count();
                    resultFiles = resultFiles.Take(count);
                }

                var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

                Parallel.ForEach(resultFiles,
                    file => { file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name; });
                Parallel.ForEach(resultFolders,
                    folder => { folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name; });
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return new SearchResultDto { Folders = resultFolders.ToList(), Files = resultFiles.ToList(), CanModifySpace = canModifySpace };
        }

        public async Task<int> NumberOfFoundFoldersAndFilesAsync(int spaceId, int? folderId, string text)
        {
            int counter = 0;
            try
            {
                string userId = _userService.CurrentUserId;

                if (folderId != null)
                {
                    var resultFolders = await (from f in _unitOfWork.Folders.Query
                                           let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                           let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                           let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                           let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                           where f.FolderUnit.Id == folderId
                                                && (f.Space.Type == SpaceType.BinarySpace
                                                || f.Space.Owner.GlobalId == userId
                                                || userCanRead || roleCanRead
                                                || userCanModify || roleCanModify)
                                           select new
                                           {
                                               Name = f.Name
                                           }).ToListAsync();

                    var resultFiles = await (from f in _unitOfWork.Files.Query
                                         let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                         let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                         let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                         let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                         where f.FolderUnit.Id == folderId
                                              && (f.Space.Type == SpaceType.BinarySpace
                                              || f.Space.Owner.GlobalId == userId
                                              || userCanRead || roleCanRead
                                              || userCanModify || roleCanModify)
                                         select new 
                                         {
                                             Name = f.Name
                                         }).ToListAsync();

                    counter += resultFolders.Count(f => f.Name.ToLower().Contains(text.ToLower()));
                    counter += resultFiles.Count(f => f.Name.ToLower().Contains(text.ToLower()));
                }
                else
                {
                    var resultFolders = await (from f in _unitOfWork.Folders.Query
                                           let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                           let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                           let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                           let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                           where f.Space.Id == spaceId && f.FolderUnit == null
                                                && (f.Space.Type == SpaceType.BinarySpace
                                                || f.Space.Owner.GlobalId == userId
                                                || userCanRead || roleCanRead
                                                || userCanModify || roleCanModify)
                                           select new 
                                           {
                                               Name = f.Name
                                           }).ToListAsync();

                     var resultFiles = await (from f in _unitOfWork.Files.Query
                                         let userCanRead = f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                         let roleCanRead = f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                         let userCanModify = f.Space.ModifyPermittedUsers.Any(x => x.GlobalId == userId)
                                         let roleCanModify = f.Space.ModifyPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))
                                         where f.Space.Id == spaceId && f.FolderUnit == null
                                              && (f.Space.Type == SpaceType.BinarySpace
                                              || f.Space.Owner.GlobalId == userId
                                              || userCanRead || roleCanRead
                                              || userCanModify || roleCanModify)
                                         select new
                                         {
                                             Name = f.Name
                                         }).ToListAsync();

                    counter += resultFolders.Count(f => f.Name.ToLower().Contains(text.ToLower()));
                    counter += resultFiles.Count(f => f.Name.ToLower().Contains(text.ToLower()));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return counter;
        }

        public async Task MoveContentAsync(CopyMoveContentDto content)
        {
            if (content.FilesId.Count() > 0)
            {
                var files = await _unitOfWork.Files.Query.Include(f => f.FolderUnit).Where(f => content.FilesId.Contains(f.Id)).ToListAsync();
                for (int i = 0; i < files.Count(); i++)
                {
                    files[i].FolderUnit = await _unitOfWork?.Folders?.GetByIdAsync(content.ParentId);
                    files[i].Space = await _unitOfWork.Spaces.GetByIdAsync(content.SpaceId);
                }
            }
            if (content.FoldersId.Count() > 0)
            {
                var folders = await _unitOfWork.Folders.Query.Include(f => f.FolderUnit).Where(f => content.FoldersId.Contains(f.Id)).ToListAsync();
                for (int i = 0; i < folders.Count(); i++)
                {
                    folders[i].FolderUnit = await _unitOfWork?.Folders?.GetByIdAsync(content.ParentId);
                    folders[i].Space = await _unitOfWork.Spaces.GetByIdAsync(content.SpaceId);
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CopyContentAsync(CopyMoveContentDto content)
        {
            if (content.FilesId.Count() > 0)
            {
                var filesDto = content.FilesId
                    .Select(fileId => new FileUnitDto
                    {
                        Id = fileId,
                        SpaceId = content.SpaceId,
                        ParentId = content.ParentId
                    });
                foreach (var file in filesDto)
                {
                    await _fileService.CreateCopyAsync(file.Id, file);
                }
            }
            if (content.FoldersId.Count() > 0)
            {
                var foldersDto = content.FoldersId
                    .Select(folderId => new FolderUnitDto
                    {
                        Id = folderId,
                        SpaceId = content.SpaceId,
                        ParentId = content.ParentId
                    });
                foreach (var folder in foldersDto)
                {
                    await _folderService.CreateCopyAsync(folder.Id, folder);
                }
            }
        }

        public async Task DeleteContentAsync(CopyMoveContentDto content)
        {
            foreach (var id in content.FilesId)
            {
                await _fileService.DeleteAsync(id);
            }
            foreach (var id in content.FoldersId)
            {
                await _folderService.DeleteAsync(id);
            }
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

        private async Task<SpaceDto> Pagination(SpaceDto space, int page, int count, string sort)
        {
            if (sort != null && sort.Equals("asc"))
            {
                var folders = space.Folders.OrderBy(f => f.CreatedAt);
                var files = space.Files.OrderBy(f => f.CreatedAt);

                space.Folders = folders;
                space.Files = files;
            }
            else if (sort != null && sort.Equals("desc"))
            {
                var folders = space.Folders.OrderByDescending(f => f.CreatedAt);
                var files = space.Files.OrderByDescending(f => f.CreatedAt);

                space.Folders = folders;
                space.Files = files;
            }

            int skipCount = (page - 1) * count;
            if (space.Folders.Count() <= skipCount)
            {
                skipCount -= space.Folders.Count();
                space.Folders = new List<FolderUnitDto>();
                space.Files = space.Files.Skip(skipCount).Take(count);
            }
            else
            {
                space.Folders = space.Folders.Skip(skipCount).Take(count);
                count -= space.Folders.Count();
                space.Files = space.Files.Take(count);
            }

            var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

            Parallel.ForEach(space.Files,
                file => { file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name; });
            Parallel.ForEach(space.Folders,
                folder => { folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name; });

            return space;
        }

    }
}