using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Driver.Shared.Dto.Events;
using Driver.Shared.Dto.Users;
using Drive.DataAccess.Repositories;
using Drive.DataAccess.Interfaces;
using Drive.DataAccess.Entities;
using System.Data.Entity;
using Drive.DataAccess.Entities.Event;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services.Events
{
    public class EventService : IEventService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IUsersService _userService;

        public EventService(IUnitOfWork unitOfWork, IUsersService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<EventDto> CreateAsync(EventDto dto)
        {
            var userId = _userService.CurrentUserId;
            var newEvent = new Event
            {
                Author = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == dto.Author.GlobalId),
                IsDeleted = false,
                EventDate = dto.EventDate,
                 EventType = dto.EventType,
                 FileUnit = new FileUnit
                 {
                     Name = dto.FileUnit.Name,
                     Description = dto.FileUnit.Description,
                     CreatedAt = DateTime.Now,
                     LastModified = DateTime.Now,
                     Owner = await _unitOfWork.Users.Query.SingleOrDefaultAsync(x => x.GlobalId == userId),
                     FileType = FileType.Events,
                     IsDeleted = false,
                     FolderUnit = await _unitOfWork.Folders.Query.SingleOrDefaultAsync(f => f.Id == dto.FileUnit.ParentId),
                     Space = await _unitOfWork.Spaces.Query.SingleOrDefaultAsync(f => f.Id == dto.FileUnit.SpaceId)
                 }
            };
            _unitOfWork.Events.Create(newEvent);
            await _unitOfWork.SaveChangesAsync();
            return dto;
    }

        public async Task DeleteAsync(int id)
        {
            var _event = await _unitOfWork.Events.Query.Include(x => x.FileUnit).SingleOrDefaultAsync(x => x.Id == id);
            _unitOfWork.Files.Delete(_event.FileUnit.Id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var authors = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });
            var events =  await _unitOfWork.Events.Query.Select(_event => new EventDto
            {
                Id = _event.Id,
                IsDeleted = _event.IsDeleted,
                EventDate = _event.EventDate,
                FileUnit = new FileUnitDto
                {
                    Id = _event.FileUnit.Id,
                    Name = _event.FileUnit.Name,
                    FileType = _event.FileUnit.FileType,
                    Description = _event.FileUnit.Description,
                    CreatedAt = _event.FileUnit.CreatedAt,
                    LastModified = _event.FileUnit.LastModified
                },
                EventType = _event.EventType,
                Author = new AuthorDto() { Id = _event.Author.Id, GlobalId = _event.Author.GlobalId }
            }).ToListAsync();

            Parallel.ForEach(events,
                newEvent => { newEvent.Author.Name = authors.FirstOrDefault(a => a.Id == newEvent.Author.GlobalId)?.Name; });

            return events;
        }

        public async Task<EventDto> GetAsync(int id)
        {
            var events = await _unitOfWork.Events.Query.Where(x => x.Id == id).Include(c => c.ContentLinks).Select(ev => new EventDto
            {
                Id = ev.Id,
                FileUnit = new FileUnitDto
                {
                    Id = ev.FileUnit.Id,
                    Name = ev.FileUnit.Name,
                    Description = ev.FileUnit.Description
                },
                CreatedAt = ev.FileUnit.CreatedAt,
                EventType = ev.EventType,
                ContentPhotos = ev.ContentLinks.Where(content => content.ContentType == ContentType.Photo).Select(c => new EventContentDto
                {
                    Id = c.Id,
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Description = c.Description,
                    Link = c.Content,
                    Order = c.Order,
                    CreatedAt = c.CreatedAt,
                    LastModified = c.LastModified
                }),
                ContentVideoLinks = ev.ContentLinks.Where(content => content.ContentType == ContentType.Video).Select(c => new EventContentDto
                {
                    Id = c.Id,
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Description = c.Description,
                    Link = c.Content,
                    Order = c.Order,
                    CreatedAt = c.CreatedAt,
                    LastModified = c.LastModified
                }),
                ContentSimpleLinks = ev.ContentLinks.Where(content => content.ContentType == ContentType.Link).Select(c => new EventContentDto
                {
                    Id = c.Id,
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Description = c.Description,
                    Link = c.Content,
                    Order = c.Order,
                    CreatedAt = c.CreatedAt,
                    LastModified = c.LastModified
                }),
                ContentTexts = ev.ContentLinks.Where(content => content.ContentType == ContentType.Text).Select(c => new EventContentDto
                {
                    Id = c.Id,
                    ContentType = c.ContentType,
                    Name = c.Name,
                    Description = c.Description,
                    Link = c.Content,
                    Order = c.Order,
                    CreatedAt = c.CreatedAt,
                    LastModified = c.LastModified
                }),
                EventDate = ev.EventDate
            }).FirstOrDefaultAsync();

            return events;
        }

        public Task<EventDto> UpdateAsync(int id, EventDto dto)
        {
            throw new NotImplementedException();
        }
    }
}