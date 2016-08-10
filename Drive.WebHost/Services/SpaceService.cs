using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SpaceDto> GetAsync(int id)
        {
            var data = await _unitOfWork.Spaces.GetByIdAsync(id);

            return new SpaceDto
            {
                Name = data.Name,
                Description = data.Description,
                MaxFileSize = data.MaxFileSize,
                MaxFilesQuantity = data.MaxFilesQuantity,
                ReadPermittedUsers = data.ReadPermittedUsers,
                Files = from file in data.ContentList.OfType<FileUnit>()
                        select new FileUnitDto
                        {
                            Name = file.Name,
                            Description = file.Description,
                            Id = file.Id,
                            IsDeleted = file.IsDeleted
                        },
                Folders = from folder in data.ContentList.OfType<FolderUnit>()
                          select new FolderUnitDto
                          {
                              Name = folder.Name,
                              Description = folder.Description,
                              Id = folder.Id,
                              IsDeleted = folder.IsDeleted
                          }
            };
        }

        public async Task<IEnumerable<SpaceDto>> GetAllAsync()
        {
            var data = await _unitOfWork.Spaces.GetAllAsync();

            var dto = from d in data
                      select new SpaceDto
                      {
                          Name = d.Name,
                          Description = d.Description,
                          MaxFileSize = d.MaxFileSize,
                          MaxFilesQuantity = d.MaxFilesQuantity,
                          ReadPermittedUsers = d.ReadPermittedUsers,
                          Files = from file in d.ContentList.OfType<FileUnit>()
                                  select new FileUnitDto
                                  {
                                      Name = file.Name,
                                      Description = file.Description,
                                      Id = file.Id,
                                      IsDeleted = file.IsDeleted
                                  },
                          Folders = from folder in d.ContentList.OfType<FolderUnit>()
                                    select new FolderUnitDto
                                    {
                                        Name = folder.Name,
                                        Description = folder.Description,
                                        Id = folder.Id,
                                        IsDeleted = folder.IsDeleted
                                    }
                      };
            return dto;
        }

        public async Task<int> CreateAsync(SpaceDto dto)
        {
            var space = new Space
            {
                Name = dto.Name,
                Description = dto.Description,
                MaxFilesQuantity = dto.MaxFilesQuantity,
                MaxFileSize = dto.MaxFileSize,
                ReadPermittedUsers = dto.ReadPermittedUsers,
                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now,
                IsDeleted = false
            };
            _unitOfWork.Spaces.Create(space);
            await _unitOfWork.SaveChangesAsync();
            return space.Id;
        }

        public async Task UpdateAsync(int id, SpaceDto dto)
        {
            var space = await _unitOfWork.Spaces.GetByIdAsync(id);

            space.Name = dto.Name;
            space.Description = dto.Description;
            space.MaxFileSize = dto.MaxFileSize;
            space.MaxFilesQuantity = dto.MaxFilesQuantity;
            space.ReadPermittedUsers = dto.ReadPermittedUsers;
            space.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Spaces.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}