﻿using System;
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
            var space = await _unitOfWork.Spaces.Query.Where(s => s.Id == id).Select(s => new SpaceDto
            {
                Id = s.Id,
                Name = s.Name,
                Type = s.Type,
                Description = s.Description,
                MaxFileSize = s.MaxFileSize,
                MaxFilesQuantity = s.MaxFilesQuantity,
                ReadPermittedUsers = s.ReadPermittedUsers,
                ModifyPermittedUsers = s.ModifyPermittedUsers,

                Files = s.ContentList.OfType<FileUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted).Select(f => new FileUnitDto
                {
                    Description = f.Description,
                    FileType = f.FileType,
                    Id = f.Id,
                    IsDeleted = f.IsDeleted,
                    Name = f.Name,
                    Link = f.Link,
                    CreatedAt = f.CreatedAt,
                    Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                }),
                Folders = s.ContentList.OfType<FolderUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted).Select(f => new FolderUnitDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    CreatedAt = f.CreatedAt,
                    IsDeleted = f.IsDeleted,
                    SpaceId = f.Space.Id,
                    Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                })
            }).SingleOrDefaultAsync();

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

            var space = await _unitOfWork.Spaces.Query.Where(s => s.Id == id).Select(s => new SpaceDto
            {
                Id = s.Id,
                Name = s.Name,
                Type = s.Type,
                Description = s.Description,
                Owner = s.Owner,
                MaxFileSize = s.MaxFileSize,
                MaxFilesQuantity = s.MaxFilesQuantity,
                ReadPermittedUsers = s.ReadPermittedUsers,
                ModifyPermittedUsers = s.ModifyPermittedUsers,
                ReadPermittedRoles = s.ReadPermittedRoles,
                ModifyPermittedRoles = s.ModifyPermittedRoles,
                Files = s.ContentList.OfType<FileUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted)
                .Where(f => (s.Type == SpaceType.BinarySpace)
                || (f.Owner.GlobalId == userId)
                || (f.ReadPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null
                || f.ReadPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null))
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
                    CanRead = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId,
                    CanModify = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId,
                }),
                Folders = s.ContentList.OfType<FolderUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted)
                .Where(f => (s.Type == SpaceType.BinarySpace)
                || (f.Owner.GlobalId == userId)
                || (f.ReadPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null
                || f.ReadPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null))
                .Select(f => new FolderUnitDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    CreatedAt = f.CreatedAt,
                    IsDeleted = f.IsDeleted,
                    SpaceId = f.Space.Id,
                    Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId },
                    CanRead = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId,
                    CanModify = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId
                })
            }).SingleOrDefaultAsync();

            if (space == null)
                return null;

            foreach (var item in space.Files)
            {
                if (space.Type == SpaceType.BinarySpace)
                {
                    item.CanRead = true;
                }
            }

            if (space.Type != SpaceType.BinarySpace
                 && space.Owner.GlobalId != userId)
            {
                if (space.ReadPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) == null)
                {
                    if (space.ReadPermittedRoles.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        foreach (var item in space.ReadPermittedRoles)
                        {
                            if (item.Users.FirstOrDefault(x => x.GlobalId == userId) == null)
                            {
                                return null;
                            }
                        }
                    }
                }
            }

            return await Pagination(space, page, count, sort);          
        }

        public async Task<SpaceDto> GetSpaceByTypeAsync(SpaceType type, int page, int count, string sort)
        {
            string userId = _userService.CurrentUserId;

            var space = await _unitOfWork.Spaces.Query.Where(s => s.Type == type)
                                                      .Where(s => s.Type == SpaceType.BinarySpace || s.Owner.GlobalId == userId)
                                                      .Select(s => new SpaceDto
            {
                Id = s.Id,
                Name = s.Name,
                Type = s.Type,
                Description = s.Description,
                Owner = s.Owner,
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
                    CanRead = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId,
                    CanModify = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId
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
                    CanRead = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId,
                    CanModify = s.Type == SpaceType.BinarySpace ? true : f.Owner.GlobalId == userId ? true : f.ModifyPermittedUsers.FirstOrDefault(x => x.GlobalId == userId) != null ? true : f.MorifyPermittedRoles.FirstOrDefault(x => x.Users.FirstOrDefault(p => p.GlobalId == userId) != null) != null ? true : f.Owner.GlobalId == userId
                })
            }).SingleOrDefaultAsync();

            return await Pagination(space, page, count, sort);
        }

        public async Task<int> GetTotalAsync(int id)
        {
            int counter = 0;
            var space = await _unitOfWork.Spaces.Query.Where(s => s.Id == id).Select(s => new
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
                            for (int j = 0; j < spacesList[i].ReadPermittedRoles.Count; j++)
                            {
                                if (spacesList[i].ReadPermittedRoles[j].Users.FirstOrDefault(x => x.GlobalId == userId) == null)
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
            bool ownerIncluded = dto.ReadPermittedUsers.Where(x => x.GlobalId == userId).Count() > 0;
            List<User> ReadPermittedUsers = new List<User>();
            foreach (var item in dto.ReadPermittedUsers)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                if (user == null)
                {
                    UserDto userdto = new UserDto();
                    userdto.serverUserId = item.GlobalId;
                    await _userService.CreateAsync(userdto);
                    var suser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                    ReadPermittedUsers.Add(suser);
                }
                else
                {
                    ReadPermittedUsers.Add(user);
                }
            }
            if (!ownerIncluded)
                ReadPermittedUsers.Add(new User() { GlobalId = userId, IsDeleted = false });

            ownerIncluded = dto.ModifyPermittedUsers.Where(x => x.GlobalId == userId).Count() > 0;

            List<User> ModifyPermittedUsers = new List<User>();
            foreach (var item in dto.ModifyPermittedUsers)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                if (user == null)
                {
                    UserDto userdto = new UserDto();
                    userdto.serverUserId = item.GlobalId;
                    await _userService.CreateAsync(userdto);
                    var suser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                    ModifyPermittedUsers.Add(suser);
                    ReadPermittedUsers.Add(suser);
                }
                else
                {
                    ModifyPermittedUsers.Add(user);
                    var x = ReadPermittedUsers.FirstOrDefault(p => p.GlobalId == user.GlobalId);
                    if (x == null)
                    {
                        ReadPermittedUsers.Add(user);
                    }
                }
            }

            if (!ownerIncluded)
                ModifyPermittedUsers.Add(new User() { GlobalId = userId, IsDeleted = false });

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
                if (user == null)
                {
                    UserDto userdto = new UserDto();
                    userdto.serverUserId = item.GlobalId;
                    await _userService.CreateAsync(userdto);
                    var suser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                    ReadPermittedUsers.Add(suser);
                }
                else
                {
                    ReadPermittedUsers.Add(user);
                }
            }
            List<User> ModifyPermittedUsers = new List<User>();
            foreach (var item in dto.ModifyPermittedUsers)
            {
                var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                if (user == null)
                {
                    UserDto userdto = new UserDto();
                    userdto.serverUserId = item.GlobalId;
                    await _userService.CreateAsync(userdto);
                    var suser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == item.GlobalId);
                    ModifyPermittedUsers.Add(suser);
                    ReadPermittedUsers.Add(suser);
                }
                else
                {
                    ModifyPermittedUsers.Add(user);
                    var x = ReadPermittedUsers.FirstOrDefault(p => p.GlobalId == user.GlobalId);
                    if (x == null)
                    {
                        ReadPermittedUsers.Add(user);
                    }
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
            IEnumerable<FolderUnitDto> resultFolder = new List<FolderUnitDto>();
            IEnumerable<FileUnitDto> resultFiles = new List<FileUnitDto>();
            try
            {
                if (folderId != null)
                {
                    resultFolder = await _unitOfWork.Folders.Query.
                        Where(f => f.FolderUnit.Id == folderId)
                        .Select(f => new FolderUnitDto()
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            IsDeleted = f.IsDeleted,
                            CreatedAt = f.CreatedAt,
                            LastModified = f.LastModified,
                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        }).ToListAsync();

                    resultFiles = await _unitOfWork.Files.Query.
                        Where(f => f.FolderUnit.Id == folderId)
                        .Select(f => new FileUnitDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            FileType = f.FileType,
                            IsDeleted = f.IsDeleted,
                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        }).ToListAsync();
                }
                else
                {
                    resultFolder = await _unitOfWork.Folders.Query.
                        Where(f => f.Space.Id == spaceId && f.FolderUnit == null)
                        .Select(f => new FolderUnitDto()
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            IsDeleted = f.IsDeleted,
                            CreatedAt = f.CreatedAt,
                            LastModified = f.LastModified,
                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        }).ToListAsync();

                    resultFiles = await _unitOfWork.Files.Query.
                        Where(f => f.Space.Id == spaceId && f.FolderUnit == null)
                        .Select(f => new FileUnitDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            FileType = f.FileType,
                            IsDeleted = f.IsDeleted,
                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        }).ToListAsync();
                }
                if (!string.IsNullOrEmpty(text))
                {
                    resultFiles = resultFiles.Where(f => f.Name.ToLower().Contains(text.ToLower()));
                    resultFolder = resultFolder.Where(f => f.Name.ToLower().Contains(text.ToLower()));
                }

                int skipCount = (page - 1) * count;
                if (resultFolder.Count() <= skipCount)
                {
                    skipCount -= resultFolder.Count();
                    resultFolder = new List<FolderUnitDto>();
                    resultFiles = resultFiles.Skip(skipCount).Take(count);
                }
                else
                {
                    resultFolder = resultFolder.Skip(skipCount).Take(count);
                    count -= resultFolder.Count();
                    resultFiles = resultFiles.Take(count);
                }

                var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

                Parallel.ForEach(resultFiles,
                    file => { file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name; });
                Parallel.ForEach(resultFolder,
                    folder => { folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name; });
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return new SearchResultDto { Folders = resultFolder.ToList(), Files = resultFiles.ToList() };
        }

        public async Task<int> NumberOfFoundFoldersAndFilesAsync(int spaceId, int? folderId, string text)
        {
            int counter = 0;
            try
            {
                if (folderId != null)
                {
                    var folder = await _unitOfWork.Folders.Query.Where(f => f.Id == folderId)
                        .Select(s => new
                        {
                            Folders = s.DataUnits.OfType<FolderUnit>().Where(f => !f.IsDeleted),
                            Files = s.DataUnits.OfType<FileUnit>().Where(f => !f.IsDeleted)
                        }).SingleOrDefaultAsync();
                    if (folder == null)
                        return 0;
                    counter += folder.Folders
                        .Count(f => f.Name.ToLower().Contains(text.ToLower()));

                    counter += folder.Files
                        .Count(f => f.Name.ToLower().Contains(text.ToLower()));
                }
                else
                {
                    var space = await _unitOfWork.Spaces.Query
                        .Where(s => s.Id == spaceId)
                        .Select(s => new
                        {
                            Folders = s.ContentList.OfType<FolderUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted),
                            Files = s.ContentList.OfType<FileUnit>().Where(f => f.FolderUnit == null && !f.IsDeleted)
                        }).SingleOrDefaultAsync();
                    if (space == null)
                        return 0;
                    counter += space.Folders
                        .Count(f => f.Name.ToLower().Contains(text.ToLower()));
                    counter += space.Files
                        .Count(f => f.Name.ToLower().Contains(text.ToLower()));
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return counter;
        }

        public async Task CreateUserAndFirstSpaceAsync(string globalId)
        {
            if (globalId != string.Empty)
            {
                try
                {
                    var user = await _unitOfWork.Users.Query.SingleOrDefaultAsync<User>(u => u.GlobalId == globalId);

                    if (user == null)
                    {
                        user = new User() { GlobalId = globalId, IsDeleted = false };
                        _unitOfWork.Users.Create(user);

                        var users = new List<User>();
                        users.Add(user);
                        _unitOfWork.Spaces.Create(new Space()
                        {
                            Name = "My Space",
                            Description = "My Space",
                            MaxFileSize = 1024,
                            MaxFilesQuantity = 100,
                            ModifyPermittedUsers = users,
                            ReadPermittedUsers = users,
                            IsDeleted = false,
                            CreatedAt = DateTime.Now,
                            LastModified = DateTime.Now,
                            Owner = user,
                            Type = SpaceType.MySpace
                        });

                        await _unitOfWork.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteError(ex, ex.Message);
                }
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

        public async Task<IEnumerable<TrashBinDto>> GetTrashBinContentAsync()
        {
            string userId = _userService.CurrentUserId;

            var spacesList = await _unitOfWork.Spaces.Query.Where(s => s.Owner.GlobalId == userId).Select(s => new TrashBinDto
            {
                SpaceId = s.Id,
                Name = s.Name,
                Folders = s.ContentList.OfType<FolderUnit>()
                    .Where(f => f.IsDeleted)
                    .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted) )
                    .Select(f => new FolderUnitDto
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Description = f.Description,
                        CreatedAt = f.CreatedAt,
                        IsDeleted = f.IsDeleted,
                        SpaceId = f.Space.Id,
                        Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                    }),
                Files = s.ContentList.OfType<FileUnit>()
                    .Where(f => f.IsDeleted)
                    .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted) )
                    .Select(f => new FileUnitDto
                    {
                        Description = f.Description,
                        FileType = f.FileType,
                        Id = f.Id,
                        IsDeleted = f.IsDeleted,
                        Name = f.Name,
                        CreatedAt = f.CreatedAt,
                        Link = f.Link,
                        Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                    })
            }).ToListAsync();

            return spacesList;
        }
        
    }
}