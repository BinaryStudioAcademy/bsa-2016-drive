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
using System.Collections;

namespace Drive.WebHost.Services.Events
{
    public class EventService : IEventService
    {
        IUnitOfWork _unitOfWork;
        IUsersService _userService;

        public EventService(IUnitOfWork unitOfWork, IUsersService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<CreateEventDto> CreateAsync(CreateEventDto dto)
        {
            var userId = _userService.CurrentUserId;

            var contentList = new List<EventContent>();
            contentList.AddRange(dto.ContentList.
                        Select(c => new EventContent
                                                {
                                                    Name = c.Name,
                                                    Description = c.Description,
                                                    IsDeleted = c.IsDeleted,
                                                    CreatedAt = DateTime.Now,
                                                    LastModified = DateTime.Now,
                                                    ContentType = c.ContentType,
                                                    Content = c.Content,
                                                    Order = c.Order
                                                          
                                                }));
            var newEvent = new Event
            {
                IsDeleted = false,
                EventDate = dto.EventDate,
                EventType = dto.EventType,
                ContentList = contentList,
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
            var events =  await _unitOfWork.Events.Query.Include(e => e.FileUnit.Owner).Select(_event => new EventDto
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
                    LastModified = _event.FileUnit.LastModified,
                    Author = new AuthorDto() { Id = _event.FileUnit.Owner.Id, GlobalId = _event.FileUnit.Owner.GlobalId }
                },
                EventType = _event.EventType
            }).ToListAsync();

            Parallel.ForEach(events,
                newEvent => { newEvent.FileUnit.Author.Name = authors.FirstOrDefault(a => a.Id == newEvent.FileUnit.Author.GlobalId)?.Name; });

            return events;
        }

        public Task<EventDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<EventDto> UpdateAsync(int id, EventDto dto)
        {
            throw new NotImplementedException();
        }

        public List<int> GetEventTypes()
        {
            var result = new List<int>();
            foreach (var r in Enum.GetValues(typeof(ContentType)))
            {
                result.Add((int)r);
            }
            return result;
        }
    }
}