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
                IsDeleted = course.IsDeleted,
                StartDate = course.StartDate,
                Lectures = course.Lectures.Select(lecture => new LectureDto
                {
                    Id = lecture.Id,
                    Name = lecture.Name,
                    Description = lecture.Description,
                    StartDate = lecture.StartDate,
                    CreatedAt = lecture.CreatedAt
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
                }),
                Author = new AuthorDto() {Id = course.Author.Id, GlobalId = course.Author.GlobalId}
            }).ToListAsync();

            Parallel.ForEach(courses,
                course => { course.Author.Name = authors.FirstOrDefault(o => o.Id == course.Author.GlobalId)?.Name; });

            return courses;
        }

        public async Task<AcademyProCourseDto> GetAsync(int id)
        {

            var courses = await _unitOfWork.AcademyProCourses.Query.Where(c => c.Id == id).Select(course => new AcademyProCourseDto
            {
                Id = course.Id,
                IsDeleted = course.IsDeleted,
                StartDate = course.StartDate,
                Lectures = course.Lectures.Select(lecture => new LectureDto
                {
                    Id = lecture.Id,
                    Name = lecture.Name,
                    Description = lecture.Description,
                    StartDate = lecture.StartDate,
                    CreatedAt = lecture.CreatedAt
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
            var userId = _userService.CurrentUserId;
            var course = new AcademyProCourse
            {
                StartDate = dto.StartDate,
                IsDeleted = false,
                Tags = new List<Tag>(),
                FileUnit = new FileUnit
                {
                    Name = dto.FileUnit.Name,
                    Description = dto.FileUnit.Description,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    Owner = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == userId),
                    FileType = FileType.AcademyPro,
                    IsDeleted = false
                },
                Author = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == dto.Author.GlobalId)
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
            var course = await _unitOfWork.AcademyProCourses.Query.Include(x => x.FileUnit).Include(x => x.Tags).SingleOrDefaultAsync(c => c.Id == id);
            course.StartDate = dto.StartDate;
            course.IsDeleted = dto.IsDeleted;
            course.FileUnit.Name = dto.FileUnit.Name;
            course.FileUnit.Description = dto.FileUnit.Description;
            course.FileUnit.LastModified = DateTime.Now;
            var user = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == dto.Author.GlobalId);
            if (user == null)
            {
                user = new User() {IsDeleted = false, GlobalId = dto.Author.GlobalId};
                _unitOfWork.Users.Create(user);
            }

            course.Author = user;

            dto.Tags.ForEach(tag =>
            {
                if (course.Tags?.Count(t => t.Name == tag.Name) == 0)
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