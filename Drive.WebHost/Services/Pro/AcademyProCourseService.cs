using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Interfaces;
using Drive.Logging;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto;
using Driver.Shared.Dto.Pro;
using Driver.Shared.Dto.Users;
using WebGrease.Css.Extensions;

namespace Drive.WebHost.Services.Pro
{
    public class AcademyProCourseService : IAcademyProCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUsersService _userService;

        public AcademyProCourseService(IUnitOfWork unitOfWork, ILogger logger, IUsersService userService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IEnumerable<AcademyProCourseDto>> GetAllAsync()
        {
            var authors = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

            var courses = await _unitOfWork.AcademyProCourses.Query.Select(course => new AcademyProCourseDto
            {
                Id = course.Id,
                CreatedAt = course.CreatedAt,
                ModifiedAt = course.ModifiedAt,
                IsDeleted = course.IsDeleted,
                StartDate = course.StartDate,
                Lectures = course.Lectures.Select(lecture => new LectureDto
                {
                    Id = lecture.Id,
                    Name = lecture.Name,
                    Description = lecture.Description,
                    StartDate = lecture.StartDate
                }),
                FileUnit = new FileUnitDto
                {
                    Id = course.FileUnit.Id,
                    Name = course.FileUnit.Name,
                    FileType = course.FileUnit.FileType,
                    Description = course.FileUnit.Description,
                    CreatedAt = course.FileUnit.CreatedAt,
                    LastModified = course.FileUnit.LastModified
                },
                Tags = course.Tags.Select(tag => new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name
                })
            }).ToListAsync();
            
            return courses;
        }

        public async Task<AcademyProCourseDto> GetAsync(int id)
        {

            var courses = await _unitOfWork.AcademyProCourses.Query.Where(c => c.Id == id).Select(course => new AcademyProCourseDto
            {
                Id = course.Id,
                CreatedAt = course.CreatedAt,
                ModifiedAt = course.ModifiedAt,
                IsDeleted = course.IsDeleted,
                StartDate = course.StartDate,
                Lectures = course.Lectures.Select(lecture => new LectureDto
                {
                    Id = lecture.Id,
                    Name = lecture.Name,
                    Description = lecture.Description,
                    StartDate = lecture.StartDate,
                }),
                FileUnit = new FileUnitDto
                {
                    Id = course.FileUnit.Id,
                    Name = course.FileUnit.Name,
                    FileType = course.FileUnit.FileType,
                    Description = course.FileUnit.Description,
                    CreatedAt = course.FileUnit.CreatedAt,
                    LastModified = course.FileUnit.LastModified
                },
                Tags = course.Tags.Select(tag => new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name
                })
            }).SingleOrDefaultAsync();

            return courses;
        }

        public async Task<AcademyProCourseDto> CreateAsync(AcademyProCourseDto dto)
        {
            var course = new AcademyProCourse
            {
                StartDate = dto.StartDate,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };

            dto.Tags.ForEach(tag =>
            {
                course.Tags.Add(_unitOfWork.Tags.Query.FirstOrDefault(x => x.Name == tag.Name) ?? new Tag { Name = tag.Name, IsDeleted = false });
            });

            _unitOfWork.AcademyProCourses.Create(course);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<AcademyProCourseDto> UpdateAsync(int id, AcademyProCourseDto dto)
        {
            var course = await _unitOfWork.AcademyProCourses.GetByIdAsync(id);
            course.StartDate = dto.StartDate;
            course.ModifiedAt = DateTime.Now;
            course.IsDeleted = dto.IsDeleted;

            dto.Tags.ForEach(tag =>
            {
                course.Tags.Add(_unitOfWork.Tags.Query.FirstOrDefault(x => x.Name == tag.Name) ?? new Tag { Name = tag.Name, IsDeleted = false });
            });

            await _unitOfWork.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork.AcademyProCourses.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}