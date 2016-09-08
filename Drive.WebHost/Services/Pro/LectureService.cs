using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Interfaces;
using Drive.Logging;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Services.Pro
{
    public class LectureService : ILectureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUsersService _userService;

        public LectureService(IUnitOfWork unitOfWork, ILogger logger, IUsersService userService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IEnumerable<LectureDto>> GetAllAsync()
        {
            var result = await _unitOfWork.Lectures.Query.Select(lecture => new LectureDto
            {
                Id = lecture.Id,
                Name = lecture.Name,
                CreatedAt = lecture.CreatedAt,
                Description = lecture.Description,
                ModifiedAt = lecture.ModifiedAt,
                IsDeleted = lecture.IsDeleted,
                StartDate = lecture.StartDate,
                CourseId = lecture.Course.Id
            }).ToListAsync();

            return result;
        }

        public async Task<LectureDto> GetAsync(int id)
        {
            var result = await _unitOfWork.Lectures.Query.Where(x => x.Id == id).Select(lecture => new LectureDto
            {
                Id = lecture.Id,
                Name = lecture.Name,
                CreatedAt = lecture.CreatedAt,
                Description = lecture.Description,
                ModifiedAt = lecture.ModifiedAt,
                IsDeleted = lecture.IsDeleted,
                StartDate = lecture.StartDate,
                VideoLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Video).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link,
                    LinkType = link.LinkType
                }),
                SlidesLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Slide).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link,
                    LinkType = link.LinkType
                }),
                SampleLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Sample).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link,
                    LinkType = link.LinkType
                }),
                UsefulLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Useful).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link,
                    LinkType = link.LinkType
                }),
                RepositoryLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Repository).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link,
                    LinkType = link.LinkType
                }),
                CodeSamples = lecture.CodeSamples.Select(sample => new CodeSampleDto
                {
                    Id = sample.Id,
                    Name = sample.Name,
                    IsDeleted = sample.IsDeleted,
                    Code = sample.Code
                }),
                HomeTasks = lecture.HomeTasks.Where(x => !x.IsDeleted).Select(task => new HomeTaskDto
                {
                    Id = task.Id,
                    Description = task.Description,
                    IsDeleted = task.IsDeleted,
                    DeadlineDate = task.DeadlineDate
                }),
                CourseId = lecture.Course.Id,
                Author = new AuthorDto { Id = lecture.Author.Id }
            }).SingleOrDefaultAsync();

            var author = await _userService.GetAsync(result.Author.Id);
            result.Author.Name = author.name + " " + author.surname;

            return result;
        }

        public async Task<LectureDto> CreateAsync(LectureDto dto)
        {
            var userId = _userService.CurrentUserId;

            var linksList = new List<ContentLink>();

            linksList.AddRange(ProcessList(dto.VideoLinks, LinkType.Video));
            linksList.AddRange(ProcessList(dto.RepositoryLinks, LinkType.Repository));
            linksList.AddRange(ProcessList(dto.SampleLinks, LinkType.Sample));
            linksList.AddRange(ProcessList(dto.SlidesLinks, LinkType.Slide));
            linksList.AddRange(ProcessList(dto.UsefulLinks, LinkType.Useful));

            var lecture = new Lecture
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Course = await _unitOfWork.AcademyProCourses.GetByIdAsync(dto.CourseId),
                ContentList = linksList,
                CodeSamples = dto.CodeSamples.Select(sample => new CodeSample()
                {
                    Name = sample.Name,
                    Code = sample.Code,
                    IsDeleted = false
                }).ToList(),
                Author = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == userId),
                HomeTasks = dto.HomeTasks.Select(task => new HomeTask
                {
                    Description = task.Description,
                    DeadlineDate = task.DeadlineDate,
                    IsDeleted = false
                }).ToList()
            };

            _unitOfWork.Lectures.Create(lecture);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<LectureDto> UpdateAsync(int id, LectureDto dto)
        {
            var lecture = await _unitOfWork.Lectures.Query.Where(x => x.Id == id).SingleOrDefaultAsync();
            var links = await _unitOfWork.ContentLinks.Query.AsNoTracking().Where(x => x.Lecture.Id == id).ToListAsync();
            var tasks = await _unitOfWork.HomeTasks.Query.AsNoTracking().Where(x => x.Lecture.Id == id).ToListAsync();

            if (lecture != null)
            {
                _unitOfWork.Lectures.Update(lecture);

                var linksList = new List<ContentLink>();

                linksList.AddRange(ProcessList(dto.VideoLinks, LinkType.Video));
                linksList.AddRange(ProcessList(dto.RepositoryLinks, LinkType.Repository));
                linksList.AddRange(ProcessList(dto.SampleLinks, LinkType.Sample));
                linksList.AddRange(ProcessList(dto.SlidesLinks, LinkType.Slide));
                linksList.AddRange(ProcessList(dto.UsefulLinks, LinkType.Useful));

                lecture.Name = dto.Name;
                lecture.Description = dto.Description;
                lecture.StartDate = dto.StartDate;
                lecture.ModifiedAt = DateTime.Now;
                lecture.IsDeleted = dto.IsDeleted;
                lecture.CourseId = dto.CourseId;

                linksList.ForEach(x => x.Lecture = lecture);

                var addedLinks = linksList.Where(link => links.All(x => x.Id != link.Id)).ToList();
                var deletedLinks = links.Where(link => linksList.All(x => x.Id != link.Id)).ToList();
                var updatedLinks = linksList.Where(link => deletedLinks.All(x => x.Id != link.Id)).Where(link => addedLinks.All(x => x.Id != link.Id)).ToList();

                var taskList = dto.HomeTasks.Select(task => new HomeTask
                {
                    Id = task.Id,
                    IsDeleted = false,
                    Description = task.Description,
                    DeadlineDate = task.DeadlineDate,
                    Lecture = lecture
                }).ToList();

                var addedTasks = taskList.Where(link => tasks.All(x => x.Id != link.Id)).ToList();
                var deletedTasks = tasks.Where(link => taskList.All(x => x.Id != link.Id)).ToList();
                var updatedTasks = taskList.Where(link => deletedTasks.All(x => x.Id != link.Id)).Where(link => addedTasks.All(x => x.Id != link.Id)).ToList();

                addedLinks.ForEach(x => _unitOfWork.ContentLinks.Create(x));
                updatedLinks.ForEach(x => _unitOfWork.ContentLinks.Update(x));
                deletedLinks.ForEach(x => _unitOfWork.ContentLinks.Delete(x.Id));

                addedTasks.ForEach(x => _unitOfWork.HomeTasks.Create(x));
                updatedTasks.ForEach(x => _unitOfWork.HomeTasks.Update(x));
                deletedTasks.ForEach(x => _unitOfWork.HomeTasks.Delete(x.Id));
            }

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.Lectures.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IList<ContentLink> ProcessList(IEnumerable<ContentLinkDto> dtoList, LinkType type)
        {
            return new List<ContentLink>(dtoList.Select(x => new ContentLink
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsDeleted = false,
                Link = x.Link,
                LinkType = type
            }));
        }
    }
}