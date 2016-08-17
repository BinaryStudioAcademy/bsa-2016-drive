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
    public class ContentLinkService : IContentLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ContentLinkService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<ContentLinkDto>> GetAllAsync()
        {
            var result = await _unitOfWork.ContentLinks.Query.Select(link => new ContentLinkDto
            {
                Id = link.Id,
                Name = link.Name,
                Description = link.Description,
                IsDeleted = link.IsDeleted,
                Link = link.Link,
                LinkType = link.LinkType
            }).ToListAsync();

            return result;
        }

        public async Task<ContentLinkDto> GetAsync(int id)
        {
            var result = await _unitOfWork.ContentLinks.Query.Where(c => c.Id == id).Select(link => new ContentLinkDto
            {
                Id = link.Id,
                Name = link.Name,
                Description = link.Description,
                IsDeleted = link.IsDeleted,
                Link = link.Link,
                LinkType = link.LinkType
            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<ContentLinkDto> CreateAsync(ContentLinkDto dto)
        {
            var link = new ContentLink
            {
                Name = dto.Name,
                Description = dto.Description,
                IsDeleted = false,
                Link = dto.Link,
                LinkType = dto.LinkType
            };

            _unitOfWork.ContentLinks.Create(link);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<ContentLinkDto> UpdateAsync(int id, ContentLinkDto dto)
        {
            var link = await _unitOfWork.ContentLinks.GetByIdAsync(id);
            link.Name = dto.Name;
            link.Description = dto.Description;
            link.Link = dto.Link;
            link.LinkType = dto.LinkType;
            link.IsDeleted = dto.IsDeleted;

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.ContentLinks.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}