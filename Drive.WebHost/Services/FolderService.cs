using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FolderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                          LastModified = folder.LastModified
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
                LastModified = folder.LastModified
            };
        }

        public async Task<FolderUnitDto> CreateAsync(FolderUnitDto dto)
        {
            var folder = new FolderUnit
            {
                Description = dto.Description,
                Name = dto.Name,

                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now,
                IsDeleted = false
            };

            _unitOfWork?.Folders?.Create(folder);
            await _unitOfWork?.SaveChangesAsync();

            dto.Id = folder.Id;
            dto.CreatedAt = folder.CreatedAt;
            dto.LastModified = folder.LastModified;

            return dto;
        }

        public async Task<FolderUnitDto> UpdateAsync(int id, FolderUnitDto dto)
        {
            var folder = await _unitOfWork.Folders.GetByIdAsync(id);

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

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}