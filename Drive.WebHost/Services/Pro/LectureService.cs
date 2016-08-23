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
            }).ToListAsync();

            return result;
        }

        public async Task<LectureDto> GetAsync(int id)
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
                VideoLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Video).Select(link => new ContentLinkDto
                {
                   Id = link.Id,
                   Name = link.Name,
                   Description = link.Description,
                   Link = link.Link
                }),
                SlidesLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Slide).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link
                }),
                SampleLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Sample).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link
                }),
                UsefulLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Useful).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link
                }),
                RepositoryLinks = lecture.ContentList.Where(links => links.LinkType == LinkType.Repository).Select(link => new ContentLinkDto
                {
                    Id = link.Id,
                    Name = link.Name,
                    Description = link.Description,
                    Link = link.Link
                }),
                CodeSamples = lecture.CodeSamples.Select(sample => new CodeSampleDto
                {
                   Id = sample.Id,
                   Name = sample.Name,
                   IsDeleted = sample.IsDeleted,
                   Code = sample.Code
                })
            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<LectureDto> CreateAsync(LectureDto dto)
        {
            var lecture = new Lecture
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            _unitOfWork.Lectures.Create(lecture);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<LectureDto> UpdateAsync(int id, LectureDto dto)
        {
            var lecture = await _unitOfWork.Lectures.GetByIdAsync(id);
            lecture.Name = dto.Name;
            lecture.Description = dto.Description;
            lecture.StartDate = dto.StartDate;
            lecture.ModifiedAt = DateTime.Now;
            lecture.IsDeleted = dto.IsDeleted;

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
    }
}