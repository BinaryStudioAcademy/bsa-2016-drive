using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Drive.Identity.Entities;
using Drive.Identity.Services;
using Driver.Shared.Dto;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _usersService;
        private readonly IFileService _fileService;

        public FolderService(IUnitOfWork unitOfWork, IUsersService usersService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _usersService = usersService;
            _fileService = fileService;
        }

        public async Task<IEnumerable<FolderUnitDto>> GetAllAsync()
        {
            var folders = await _unitOfWork?.Folders?.Query.Select(f => new FolderUnitDto
            {
                Id = f.Id,
                Description = f.Description,
                Name = f.Name,
                IsDeleted = f.IsDeleted,
                CreatedAt = f.CreatedAt,
                LastModified = f.LastModified,
                SpaceId = f.Space.Id
            }).ToListAsync();

            return folders;
        }

        public async Task<IEnumerable<FolderUnitDto>> GetAllByParentIdAsync(int spaceId, int? parentId)
        {
            var folders = await _unitOfWork?.Folders?.Query.Where(f => f.Space.Id == spaceId)
                                                           .Where(f => f.FolderUnit.Id == parentId)
                                                           .Select(f => new FolderUnitDto
            {
                Id = f.Id,
                Description = f.Description,
                Name = f.Name,
                IsDeleted = f.IsDeleted,
                CreatedAt = f.CreatedAt,
                LastModified = f.LastModified,
                SpaceId = f.Space.Id
            }).ToListAsync();

            return folders;
        }

        public async Task<FolderUnitDto> GetAsync(int id)
        {
            var folder = await _unitOfWork?.Folders?.GetByIdAsync(id);

            if (folder == null)
                return null;

            return new FolderUnitDto
            {
                Id = folder.Id,
                Description = folder.Description,
                Name = folder.Name,
                IsDeleted = folder.IsDeleted,
                CreatedAt = folder.CreatedAt,
                LastModified = folder.LastModified,
                SpaceId = folder.Space.Id
            };
        }

        public async Task<FolderUnitDto> GetDeletedAsync(int id)
        {
            var folder = await _unitOfWork.Folders.Deleted.Where(f => f.Id == id).Select(f => new FolderUnitDto()
            {
                Id = f.Id,
                Description = f.Description,
                Name = f.Name,
                IsDeleted = f.IsDeleted,
                CreatedAt = f.CreatedAt,
                LastModified = f.LastModified,
                SpaceId = f.Space.Id
            }).SingleOrDefaultAsync();

            return folder;
        }

        public async Task<FolderUnitDto> CreateAsync(FolderUnitDto dto)
        {
            var user = await _usersService?.GetCurrentUser();

            var space = await _unitOfWork?.Spaces?.GetByIdAsync(dto.SpaceId);
            var parentFolder = await _unitOfWork?.Folders?.GetByIdAsync(dto.ParentId);


            if (space != null)
            {
                var folder = new FolderUnit
                {
                    Description = dto.Description,
                    Name = dto.Name,

                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    IsDeleted = false,
                    Space = space,
                    FolderUnit = parentFolder,
                    Owner = await _unitOfWork.Users.Query.FirstOrDefaultAsync(u => u.GlobalId == user.serverUserId)
                };

                _unitOfWork?.Folders?.Create(folder);
                await _unitOfWork?.SaveChangesAsync();

                dto.Id = folder.Id;
                dto.CreatedAt = folder.CreatedAt;
                dto.LastModified = folder.LastModified;
                dto.Author = new AuthorDto() { Id = folder.Owner.Id, Name = user.name + ' ' + user.surname };

                return dto;
            }
            return null;
        }

        public async Task<FolderUnitDto> UpdateAsync(int id, FolderUnitDto dto)
        {
            var folder = await _unitOfWork?.Folders?.GetByIdAsync(id);

            if (folder == null)
                return null;

            folder.Description = dto.Description;
            folder.IsDeleted = dto.IsDeleted;
            folder.Name = dto.Name;
            folder.LastModified = DateTime.Now;

            await _unitOfWork?.SaveChangesAsync();

            dto.LastModified = DateTime.Now;

            return dto;
        }

        public async Task<FolderUnitDto> UpdateDeletedAsync(int id, int? oldParentId, FolderUnitDto dto)
        {
            var folder = await _unitOfWork?.Folders?.Deleted.Include(f => f.DataUnits).SingleOrDefaultAsync(f => f.Id == id);
            if (folder == null)
                return null;

            folder.IsDeleted = false;

            folder.Name = dto.Name;
            folder.Description = dto.Description;
            folder.IsDeleted = dto.IsDeleted;
            folder.LastModified = DateTime.Now;

            var space = await _unitOfWork.Spaces.GetByIdAsync(dto.SpaceId);

            if (oldParentId != null)
            {
                var oldParentFolder = await _unitOfWork.Folders.Query.Include(f => f.DataUnits).SingleOrDefaultAsync(f => f.Id == oldParentId);

                var list = new List<DataUnit>();
                foreach (var item in oldParentFolder.DataUnits)
                {
                    if (item.Id != folder.Id)
                    {
                        list.Add(item);
                    }
                }

                oldParentFolder.DataUnits = list;
            }

            var parentFolder = await _unitOfWork.Folders.GetByIdAsync(dto.ParentId);

            folder.Space = space;
            folder.FolderUnit = parentFolder ?? null;

            foreach (var item in folder.DataUnits)
            {
                item.IsDeleted = false;

                item.Space = await _unitOfWork.Spaces.GetByIdAsync(folder.Space.Id);

                if (item is FolderUnit)
                {
                    await ChangeSpaceId(item.Id, folder.Space.Id);
                }
            }

            await _unitOfWork?.SaveChangesAsync();

            return dto;
        }

        public async Task<FolderUnitDto> CreateCopyAsync(int id, FolderUnitDto dto)
        {
            var folder = await _unitOfWork?.Folders?.Query.Include(f => f.DataUnits)
                                                          .Include(f => f.ModifyPermittedUsers)
                                                          .Include(f => f.ReadPermittedUsers)
                                                          .SingleOrDefaultAsync(f => f.Id == id);

            if (folder == null)
                return null;

            var space = await _unitOfWork.Spaces.GetByIdAsync(dto.SpaceId);

            var user = await _usersService?.GetCurrentUser();

            var copy = new FolderUnit
            {
                Name = folder.Name,
                Description = folder.Description,
                IsDeleted = folder.IsDeleted,
                CreatedAt = folder.CreatedAt,
                LastModified = folder.LastModified,
                Space = space,
                Owner = await _unitOfWork.Users.Query.FirstOrDefaultAsync(u => u.GlobalId == user.serverUserId),
                ModifyPermittedUsers = folder.ModifyPermittedUsers,
                ReadPermittedUsers = folder.ReadPermittedUsers
            };

            if (dto.ParentId != null)
            {
                var parent = await _unitOfWork.Folders.GetByIdAsync(dto.ParentId);

                copy.FolderUnit = parent;
                copy.Space = space;

                //folder.FolderUnit = parent;
            }

            _unitOfWork.Folders.Create(copy);

            await _unitOfWork?.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            var rootFolder = await _unitOfWork.Folders.Query.Include(f => f.DataUnits).SingleOrDefaultAsync(f => f.Id == id);

            rootFolder.IsDeleted = true;

            foreach (var item in rootFolder.DataUnits)
            {
                if (item is FolderUnit)
                {
                    var folder = await _unitOfWork.Folders.GetByIdAsync(item.Id);

                    folder.IsDeleted = true;

                    await DeleteAsync(folder.Id);
                }
                else
                {
                    var file = await _unitOfWork.Files.GetByIdAsync(item.Id);

                    file.IsDeleted = true;
                }
            }

            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task<FolderContentDto> GetContentAsync(int id, int page, int count, string sort)
        {
            IEnumerable<FolderUnitDto> folders = await _unitOfWork.Folders.Query.Where(x => x.FolderUnit.Id == id)
            .Select(f => new FolderUnitDto
            {
                Name = f.Name,
                Description = f.Description,
                Id = f.Id,
                IsDeleted = f.IsDeleted,
                CreatedAt = f.CreatedAt,
                Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
            }).ToListAsync();
            IEnumerable<FileUnitDto> files = await _unitOfWork.Files.Query.Where(x => x.FolderUnit.Id == id)
                .Select(f => new FileUnitDto
                {
                    Name = f.Name,
                    Description = f.Description,
                    Id = f.Id,
                    IsDeleted = f.IsDeleted,
                    Link = f.Link,
                    CreatedAt = f.CreatedAt,
                    FileType = f.FileType,
                    Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                }).ToListAsync();

            if (sort != null && sort.Equals("asc"))
            {
                var foldersOrdered = folders.OrderBy(f => f.CreatedAt);
                var filesOrdered = files.OrderBy(f => f.CreatedAt);

                folders = foldersOrdered;
                files = filesOrdered;
            }
            else if (sort != null && sort.Equals("desc"))
            {
                var foldersOrdered = folders.OrderByDescending(f => f.CreatedAt);
                var filesOrdered = files.OrderByDescending(f => f.CreatedAt);

                folders = foldersOrdered;
                files = filesOrdered;
            }

            int skipCount = (page - 1) * count;
            if (folders.Count() <= skipCount)
            {
                skipCount -= folders.Count();
                folders = new List<FolderUnitDto>();
                files = files.Skip(skipCount).Take(count);
            }
            else
            {
                folders = folders.Skip(skipCount).Take(count);
                count -= folders.Count();
                files = files.Take(count);
            }

            var owners = (await _usersService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

            Parallel.ForEach(files, file =>
            {
                file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name;
            });
            Parallel.ForEach(folders, folder =>
            {
                folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name;
            });


            return new FolderContentDto
            {
                Files = files,
                Folders = folders
            };
        }
        public async Task<FolderContentDto> GetContentAsync(int id)
        {
            IEnumerable<FolderUnitDto> folders = await _unitOfWork.Folders.Query.Where(x => x.FolderUnit.Id == id)
                .Select(f => new FolderUnitDto
                {
                    Id = f.Id,
                }).ToListAsync();
            IEnumerable<FileUnitDto> files = await _unitOfWork.Files.Query.Where(x => x.FolderUnit.Id == id)
                .Select(f => new FileUnitDto
                {
                    Id = f.Id,
                }).ToListAsync();

            return new FolderContentDto
            {
                Files = files,
                Folders = folders
            };
        }


        public async Task<int> GetContentTotalAsync(int id)
        {
            int counter = 0;
            var folders = await _unitOfWork.Folders.Query.Where(x => x.FolderUnit.Id == id).ToListAsync();
            var files = await _unitOfWork.Files.Query.Where(x => x.FolderUnit.Id == id).ToListAsync();
            counter += folders.Count();
            counter += files.Count();
            return counter;
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

        private async Task ChangeSpaceId(int id, int spaceId)
        {
            var folder =
                await _unitOfWork?.Folders?.Deleted.Include(f => f.DataUnits).SingleOrDefaultAsync(f => f.Id == id);

            foreach (var item in folder.DataUnits)
            {
                item.IsDeleted = false;

                item.Space = await _unitOfWork.Spaces.GetByIdAsync(spaceId);

                if (item is FolderUnit)
                {
                    await ChangeSpaceId(item.Id, spaceId);
                }
            }
        }
    }
}