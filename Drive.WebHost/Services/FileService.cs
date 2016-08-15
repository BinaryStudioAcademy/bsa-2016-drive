using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;

namespace Drive.WebHost.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FileUnitDto>> GetAllAsync()
        {
            var data = await _unitOfWork.Files.GetAllAsync();
            if (data != null)
            {
                var dto = from d in data
                    select new FileUnitDto()
                    {
                        Id = d.Id,
                        IsDeleted = d.IsDeleted,
                        FyleType = d.FileType,
                        Name = d.Name,
                        Description = d.Description,
                        Owner = d.Owner,
                        SpaceId = d.Space.Id
                    };

                return dto;
            }
            return null;
        }

        public async Task<FileUnitDto> GetAsync(int id)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(id);

            if (file != null)
            {
                return new FileUnitDto
                {
                    Id = file.Id,
                    IsDeleted = file.IsDeleted,
                    FyleType = file.FileType,
                    Name = file.Name,
                    Description = file.Description,
                    Owner = file.Owner,
                    SpaceId = file.Space.Id
                };
            }
            return null;
        }

        public async Task<FileUnitDto> CreateAsync(FileUnitDto dto)
        {
            var space = await _unitOfWork.Spaces.GetByIdAsync(dto.SpaceId);
            if (space != null)
            {
                var file = new FileUnit()
                {
                    Name = dto.Name,
                    //Link = dto.Link,
                    FileType = FileType.None,
                    Description = dto.Description,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    IsDeleted = false,
                    Owner = dto.Owner,
                    Space = space
                };


                _unitOfWork.Files.Create(file);
                await _unitOfWork.SaveChangesAsync();

                dto.Id = file.Id;
                dto.CreatedAt = file.CreatedAt;
                dto.LastModified = file.LastModified;

                return dto;
            }
            return null;
        }

        public async Task<FileUnitDto> UpdateAsync(int id, FileUnitDto dto)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(id);

            file.Name = dto.Name;
            file.FileType = FileType.None;
            file.Description = dto.Description;
            file.IsDeleted = dto.IsDeleted;
            file.LastModified = DateTime.Now;
            file.Owner = dto.Owner;

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.Files.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}