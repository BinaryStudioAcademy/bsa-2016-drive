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

namespace Drive.WebHost.Services.Pro
{
    public class AcademyProCourseService : IAcademyProCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public AcademyProCourseService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<AcademyProCourseDto>> GetAllAsync()
        {
            var courses = await _unitOfWork.AcademyProCourses.Query.Select(course => new AcademyProCourseDto
            {
                Id = course.Id,
                Name = course.Name,
                CreatedAt = course.CreatedAt,
                Description = course.Description,
                ModifiedAt = course.ModifiedAt,
                IsDeleted = course.IsDeleted,
                StartDate = course.StartDate,
                Lectures = course.Lectures.Select(lecture => new LectureDto
                {
                    Id = lecture.Id,
                    Name = lecture.Name,
                    Description = lecture.Description,
                    StartDate = lecture.StartDate
                })
            }).ToListAsync();

            return courses;
        }

        public async Task<AcademyProCourseDto> GetAsync(int id)
        {
            var courses = await _unitOfWork.AcademyProCourses.Query.Where(c => c.Id == id).Select(course => new AcademyProCourseDto
            {
                Id = course.Id,
                Name = course.Name,
                CreatedAt = course.CreatedAt,
                Description = course.Description,
                ModifiedAt = course.ModifiedAt,
                IsDeleted = course.IsDeleted,
                StartDate = course.StartDate,
                Lectures = course.Lectures.Select(lecture => new LectureDto
                {
                    Id = lecture.Id,
                    Name = lecture.Name,
                    Description = lecture.Description,
                    StartDate = lecture.StartDate,
                })
            }).SingleOrDefaultAsync();

            return courses;
        }

        public async Task<AcademyProCourseDto> CreateAsync(AcademyProCourseDto dto)
        {
            var course = new AcademyProCourse
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            _unitOfWork.AcademyProCourses.Create(course);
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<AcademyProCourseDto> UpdateAsync(int id, AcademyProCourseDto dto)
        {
            var course = await _unitOfWork.AcademyProCourses.GetByIdAsync(id);
            course.Name = dto.Name;
            course.Description = dto.Description;
            course.StartDate = dto.StartDate;
            course.ModifiedAt = DateTime.Now;

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