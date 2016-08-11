using System;
using System.Collections.Generic;
using System.Linq;
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

            var dto = from d in data
                      select new FileUnitDto()
                      {
                          Id = d.Id,
                          IsDeleted = d.IsDeleted,
                          FyleType = d.FileType,
                          Name = d.Name,
                          Description = d.Description
                      };

            return dto;
        }

        public async Task<FileUnitDto> GetAsync(int id)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(id);

            return new FileUnitDto
            {
                Id = file.Id,
                IsDeleted = file.IsDeleted,
                FyleType = file.FileType,
                Name = file.Name,
                Description = file.Description
            };
        }

        public async Task<int> CreateAsync(FileUnitDto dto)
        {
            var file = new FileUnit()
            {
                Name = dto.Name,
                FileType = FileType.None,
                Description = dto.Description,

                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now,
                IsDeleted = false
            };

            _unitOfWork.Files.Create(file);
            await _unitOfWork.SaveChangesAsync();

            return file.Id;
        }

        public async Task UpdateAsync(int id, FileUnitDto dto)
        {
            var file = await _unitOfWork.Files.GetByIdAsync(id);

            file.Name = dto.Name;
            file.FileType = FileType.None;
            file.Description = dto.Description;
            file.IsDeleted = dto.IsDeleted;
            file.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
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