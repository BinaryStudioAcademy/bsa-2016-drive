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

        public FolderService(IUnitOfWork unitOfWork, IUsersService usersService)
        {
            _unitOfWork = unitOfWork;
            _usersService = usersService;
        }

        public async Task<IEnumerable<FolderUnitDto>> GetAllAsync()
        {
            var folders = await _unitOfWork?.Folders?.GetAllAsync();

            if (folders == null)
                return null;

            var dto = from folder in folders
                      select new FolderUnitDto
                      {
                          Id = folder.Id,
                          Description = folder.Description,
                          Name = folder.Name,
                          IsDeleted = folder.IsDeleted,
                          CreatedAt = folder.CreatedAt,
                          LastModified = folder.LastModified,
                          SpaceId = folder.Space.Id
                      };

            return dto;
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
                    Parent = parentFolder,
                    Owner = await _unitOfWork.Users.Query.FirstOrDefaultAsync(u => u.GlobalId == user.serverUserId)
                };

                _unitOfWork?.Folders?.Create(folder);
                await _unitOfWork?.SaveChangesAsync();

                dto.Id = folder.Id;
                dto.CreatedAt = folder.CreatedAt;
                dto.LastModified = folder.LastModified;
                dto.Author = new AuthorDto() {Id = folder.Owner.Id, Name = user.name + ' ' + user.surname };

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

        public async Task DeleteAsync(int id)
        {
            _unitOfWork?.Folders?.Delete(id);
            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task<FolderContentDto> GetContentAsync(int id, int page, int count)
        {
            IEnumerable<FolderUnitDto> folders = await _unitOfWork.Folders.Query.Where(x => x.Parent.Id == id)
            .Select(f => new FolderUnitDto
            {
                Name = f.Name,
                Description = f.Description,
                Id = f.Id,
                IsDeleted = f.IsDeleted,
                CreatedAt = f.CreatedAt,
                Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
            }).ToListAsync();
            IEnumerable<FileUnitDto> files = await _unitOfWork.Files.Query.Where(x => x.Parent.Id == id)
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

        public async Task<int> GetContentTotalAsync(int id)
        {
            int counter = 0;
            var folders = await _unitOfWork.Folders.Query.Where(x => x.Parent.Id == id).ToListAsync();
            var files = await _unitOfWork.Files.Query.Where(x => x.Parent.Id == id).ToListAsync();
            counter += folders.Count();
            counter += files.Count();
            return counter;
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}