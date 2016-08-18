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
            var folders = await _unitOfWork.Folders.GetAllAsync();

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
            var folder = await _unitOfWork.Folders.GetByIdAsync(id);

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
            var user = await _usersService.GetLocalUser();

            var space = await _unitOfWork.Spaces.GetByIdAsync(dto.SpaceId);
            var parentFolder = await _unitOfWork.Folders.GetByIdAsync(dto.ParentId);

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
                    Owner = _unitOfWork.Users.Query.FirstOrDefault(u => u.GlobalId== user.serverUserId)
                };

                _unitOfWork.Folders.Create(folder);
                await _unitOfWork.SaveChangesAsync();

                dto.Id = folder.Id;
                dto.CreatedAt = folder.CreatedAt;
                dto.LastModified = folder.LastModified;
                dto.Author = new AuthorDto() {Id = folder.Owner.Id, Name = user.name};

                return dto;
            }
            return null;
        }

        public async Task<FolderUnitDto> UpdateAsync(int id, FolderUnitDto dto)
        {
            var folder = await _unitOfWork.Folders.GetByIdAsync(id);

            folder.Description = dto.Description;
            folder.IsDeleted = dto.IsDeleted;
            folder.Name = dto.Name;
            folder.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            dto.LastModified = DateTime.Now;

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.Folders.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<FolderContentDto> GetContentAsync(int id)
        {
            var folders = await _unitOfWork.Folders.Query.Where(x => x.Parent.Id == id).OfType<DataUnit>().ToListAsync();
            var files = await _unitOfWork.Files.Query.Where(x => x.Parent.Id == id).OfType<DataUnit>().ToListAsync();

            return new FolderContentDto
            {
                Files = from file in files
                        select new FileUnitDto
                        {
                            Name = file.Name,
                            Description = file.Description,
                            Id = file.Id,
                            IsDeleted = file.IsDeleted,
                        },
                Folders = from folder in folders
                          select new FolderUnitDto
                          {
                              Name = folder.Name,
                              Description = folder.Description,
                              Id = folder.Id,
                              IsDeleted = folder.IsDeleted,
                              CreatedAt = folder.CreatedAt,
                              LastModified = folder.LastModified
                          }
            };
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}