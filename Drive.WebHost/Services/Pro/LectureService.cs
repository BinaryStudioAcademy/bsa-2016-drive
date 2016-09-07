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
    public class LectureService : ILectureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public LectureService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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
                CourseId = lecture.Course.Id
            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<LectureDto> CreateAsync(LectureDto dto)
        {
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
                }).ToList()
            };

            _unitOfWork.Lectures.Create(lecture);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<LectureDto> UpdateAsync(int id, LectureDto dto)
        {
            var lecture = await _unitOfWork.Lectures.Query.Where(x => x.Id == id).Include(x => x.ContentList).SingleOrDefaultAsync();
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

                var existingLinks = lecture.ContentList.ToList();
                var addedLinks = linksList.Where(link => existingLinks.All(x => x.Id != link.Id)).ToList();
                var deletedLinks = existingLinks.Where(link => linksList.All(x => x.Id != link.Id)).ToList();
                var updatedLinks = existingLinks.Where(link => deletedLinks.All(x => x.Id != link.Id)).ToList();

                addedLinks.ForEach(x => _unitOfWork.ContentLinks.Create(x));
                updatedLinks.ForEach(x => _unitOfWork.ContentLinks.Update(x));
                deletedLinks.ForEach(x => _unitOfWork.ContentLinks.ForceDelete(x.Id));
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