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
            var data = await _unitOfWork.Folders.GetAllAsync();

            var dto = from d in data
                      select new FolderUnitDto
                      {
                          Id = d.Id,
                          Description = d.Description,
                          Name = d.Name,
                          IsDeleted = d.IsDeleted
                      };

            return dto;
        }

        public async Task<FolderUnitDto> GetAsync(int id)
        {
            var folder = await _unitOfWork.Folders.GetByIdAsync(id);

            return new FolderUnitDto
            {
                Id = folder.Id,
                Description = folder.Description,
                Name = folder.Name,
                IsDeleted = folder.IsDeleted
            };
        }

        public async Task<int> CreateAsync(FolderUnitDto dto)
        {
            var folder = new FolderUnit
            {
                Description = dto.Description,
                Name = dto.Name,

                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now,
                IsDeleted = false
            };

            _unitOfWork.Folders.Create(folder);
            await _unitOfWork.SaveChangesAsync();

            return folder.Id;
        }

        public async Task UpdateAsync(FolderUnitDto dto)
        {
            var folder = new FolderUnit
            {
                Description = dto.Description,
                IsDeleted = dto.IsDeleted,
                Name = dto.Name,

                LastModified = DateTime.Now
            };

            _unitOfWork.Folders.Update(folder);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.Folders.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}