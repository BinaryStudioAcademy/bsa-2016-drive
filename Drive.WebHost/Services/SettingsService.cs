using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SettingsDto> GetAsync(int id)
        {
            var settings = await _unitOfWork.Spaces.Query.Where(s => s.Id == id).Select(s => new SettingsDto()
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                MaxFileSize = s.MaxFileSize,
                MaxFilesQuantity = s.MaxFilesQuantity,
            }).SingleOrDefaultAsync();

            return settings;
        }

        public async Task<SettingsDto> UpdateAsync(int id, SettingsDto dto)
        {
            var space = await _unitOfWork?.Spaces?.GetByIdAsync(id);

            if (space == null)
                return null;

            space.Description = dto.Description;
            space.Name = dto.Name;
            space.MaxFilesQuantity = dto.MaxFilesQuantity;
            space.MaxFileSize = dto.MaxFileSize;
            space.LastModified = DateTime.Now;

            await _unitOfWork?.SaveChangesAsync();

            return dto;
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}