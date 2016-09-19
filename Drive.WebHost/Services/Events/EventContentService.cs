using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Drive.DataAccess.Entities.Event;
using Driver.Shared.Dto.Events;
using Drive.DataAccess.Interfaces;
using Drive.Logging;

namespace Drive.WebHost.Services.Events
{
    public class EventContentService : IEventContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUsersService _userService;

        public EventContentService(IUnitOfWork unitOfWork, ILogger logger, IUsersService userService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
        }
        public async Task<EventContentDto> CreateAsync(EventContentDto dto)
        {
            var ec = new EventContent
            {
                Name = dto.Name,
                Description = dto.Description,
                IsDeleted = false,
                Content = dto.Content,
                ContentType = dto.ContentType,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                Event = await _unitOfWork.Events.Query.FirstOrDefaultAsync(e => e.Id == dto.EventId),
                Order = dto.Order
            };

            _unitOfWork.EventContents.Create(ec);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.EventContents.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventContentDto>> GetAllAsync()
        {
            var result = await _unitOfWork.EventContents.Query.Select(ec => new EventContentDto
            {
                Id = ec.Id,
                Name = ec.Name,
                Description = ec.Description,
                CreatedAt = ec.CreatedAt,
                LastModified = ec.LastModified,
                IsDeleted = ec.IsDeleted,
                EventId = ec.Event.Id,
                Content = ec.Content,
                ContentType = ec.ContentType,
                Order = ec.Order
            }).ToListAsync();

            return result;
        }

        public async Task<EventContentDto> GetAsync(int id)
        {
            var result = await _unitOfWork.EventContents.Query.Where(x => x.Id == id).Select(ec => new EventContentDto
            {
                Id = ec.Id,
                Name = ec.Name,
                Description = ec.Description,
                CreatedAt = ec.CreatedAt,
                LastModified = ec.LastModified,
                IsDeleted = ec.IsDeleted,
                EventId = ec.Event.Id,
                Content = ec.Content,
                ContentType = ec.ContentType,
                Order = ec.Order
            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<EventContentDto> UpdateAsync(int id, EventContentDto dto)
        {
            var ec = await _unitOfWork.EventContents.GetByIdAsync(id);
            ec.Name = dto.Name;
            ec.Description = dto.Description;
            ec.Content = dto.Content;
            ec.ContentType = dto.ContentType;
            ec.IsDeleted = dto.IsDeleted;
            ec.LastModified = DateTime.UtcNow;
            ec.Order = dto.Order;

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}