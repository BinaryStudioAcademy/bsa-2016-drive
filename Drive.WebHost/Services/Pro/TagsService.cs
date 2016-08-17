using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Interfaces;
using Drive.Logging;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;

namespace Drive.WebHost.Services.Pro
{
    public class TagsService : ITagsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public TagsService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var result = await _unitOfWork.Tags.Query.Select(tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                IsDeleted = tag.IsDeleted,
            }).ToListAsync();

            return result;
        }

        public async Task<TagDto> GetAsync(int id)
        {
            var result = await _unitOfWork.Tags.Query.Where(c => c.Id == id).Select(tag => new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                IsDeleted = tag.IsDeleted,
            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<int> CreateAsync(TagDto dto)
        {
            var tag = new Tag
            {
                Name = dto.Name,
                IsDeleted = false,
            };

            _unitOfWork.Tags.Create(tag);
            await _unitOfWork.SaveChangesAsync();
            return tag.Id;
        }

        public async Task UpdateAsync(int id, TagDto dto)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);

            tag.Name = dto.Name;
            tag.IsDeleted = dto.IsDeleted;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.CodeSamples.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}