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
                Author = new AuthorDto() { Id = course.Author.Id, GlobalId = course.Author.GlobalId }
            }).ToListAsync();

            Parallel.ForEach(courses,
                course => { course.Author.Name = authors.FirstOrDefault(a => a.Id == course.Author.GlobalId)?.Name; });

            return courses;
        }

        public async Task<AcademyProCourseDto> GetAsync(int id)
        {
            string userId = _userService.CurrentUserId;
            var authors = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

            var resultCourse = await _unitOfWork.AcademyProCourses.Query.Where(c => c.Id == id).Select(course => new AcademyProCourseDto
            {
                Id = course.Id,
                IsDeleted = course.IsDeleted,
                StartDate = course.StartDate,
                Lectures = course.Lectures.Where(x => x.IsDeleted == false).Select(lecture => new LectureDto
                {
                    Id = lecture.Id,
                    Name = lecture.Name,
                    Description = lecture.Description,
                    StartDate = lecture.StartDate,
                    CreatedAt = lecture.CreatedAt,
                    CanModify = lecture.Author.GlobalId == userId,
                    Author = new AuthorDto { Id = lecture.Author.Id, GlobalId = lecture.Author.GlobalId }
                }),
                FileUnit = new FileUnitDto
                {
                    Id = course.FileUnit.Id,
                    Name = course.FileUnit.Name,
                    FileType = course.FileUnit.FileType,
                    Description = course.FileUnit.Description,
                    CreatedAt = course.FileUnit.CreatedAt,
                    LastModified = course.FileUnit.LastModified,
                    CanModify = course.Author.GlobalId == userId
                },
                Tags = course.Tags.Select(tag => new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name
                }),
                Author = new AuthorDto { Id = course.Author.Id, GlobalId = course.Author.GlobalId }
            }).SingleOrDefaultAsync();

            resultCourse.Author.Name = authors.SingleOrDefault(a => a.Id == resultCourse.Author.GlobalId)?.Name;

            Parallel.ForEach(resultCourse.Lectures,
                lecture => { lecture.Author.Name = authors.FirstOrDefault(a => a.Id == lecture.Author.GlobalId)?.Name; });


            return resultCourse;
        }

        public async Task<AcademyProCourseDto> CreateAsync(AcademyProCourseDto dto)
        {
            var userId = _userService.CurrentUserId;

            var user = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == dto.Author.GlobalId);

            var course = new AcademyProCourse
            {
                StartDate = dto.StartDate.ToUniversalTime(),
                IsDeleted = false,
                Tags = new List<Tag>(),
                FileUnit = new FileUnit
                {
                    Name = dto.FileUnit.Name,
                    Description = dto.FileUnit.Description,
                    CreatedAt = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    Owner = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == userId),
                    FileType = FileType.AcademyPro,
                    IsDeleted = false,
                    FolderUnit = await _unitOfWork.Folders.Query.SingleOrDefaultAsync(f => f.Id == dto.FileUnit.ParentId),
                    Space = await _unitOfWork.Spaces.Query.SingleOrDefaultAsync(s => s.Id == dto.FileUnit.SpaceId)
                },
                Author = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == dto.Author.GlobalId)
            };

            dto.Tags.ForEach(tag =>
            {
                course.Tags.Add(_unitOfWork.Tags.Query.FirstOrDefault(x => x.Name == tag.Name) ?? new Tag { Name = tag.Name, IsDeleted = false });
            });

            _unitOfWork.AcademyProCourses.Create(course);
            await _unitOfWork.SaveChangesAsync();
            dto.Id = course.Id;
            return dto;
        }

        public async Task<AcademyProCourseDto> UpdateAsync(int id, AcademyProCourseDto dto)
        {
            var course = await _unitOfWork.AcademyProCourses.Query.Include(x => x.FileUnit).Include(x => x.Tags).SingleOrDefaultAsync(c => c.Id == id);
            course.StartDate = dto.StartDate.ToUniversalTime();
            course.IsDeleted = dto.IsDeleted;
            course.FileUnit.Name = dto.FileUnit.Name;
            course.FileUnit.Description = dto.FileUnit.Description;
            course.FileUnit.LastModified = DateTime.UtcNow;
            var user = await _unitOfWork.Users.Query.SingleOrDefaultAsync(u => u.GlobalId == dto.Author.GlobalId);

            course.Author = user;

            for(int i=0;i < course.Tags?.Count;i++)
            {
                var tag = course.Tags[i];
                if (dto.Tags?.FirstOrDefault(t => t.Name == tag.Name) == null)
                    course.Tags?.RemoveAt(i);
            }

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
            var course = await _unitOfWork.AcademyProCourses.Query
                .Include(x => x.FileUnit)
                .SingleOrDefaultAsync(x => x.Id == id);
            _unitOfWork.Files.Delete(course.FileUnit.Id);
            //_unitOfWork.AcademyProCourses.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppsAPDto>> SearchCourses(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return await FilterCourses();
            }

            var authors = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

            var courses = await _unitOfWork.AcademyProCourses.Query.Include(c => c.Tags).Include(c => c.FileUnit).
                                                                    Where(x => (x.FileUnit.Name.Contains(text.ToLower()) ||
                                                                    x.Tags.Any(t => t.Name.Contains(text.ToLower()))) && !x.FileUnit.IsDeleted).
                                                                    GroupBy(c => c.FileUnit.Space).
                                                                    Select(course => new AppsAPDto
                                                                    {
                                                                        SpaceId = course.Key.Id,
                                                                        SpaceType = course.Key.Type,
                                                                        Name = course.Key.Name,
                                                                        Courses = course.Select(c => new AcademyProCourseDto
                                                                        {
                                                                            Id = c.Id,
                                                                            IsDeleted = c.IsDeleted,
                                                                            StartDate = c.StartDate,
                                                                            Lectures = c.Lectures.Select(lecture => new LectureDto
                                                                            {
                                                                                Id = lecture.Id,
                                                                                Name = lecture.Name,
                                                                                Description = lecture.Description,
                                                                                StartDate = lecture.StartDate,
                                                                                CreatedAt = lecture.CreatedAt,
                                                                                Author = new AuthorDto { Id = lecture.Author.Id, GlobalId = lecture.Author.GlobalId }
                                                                            }),
                                                                            FileUnit = new FileUnitDto
                                                                            {
                                                                                Id = c.FileUnit.Id,
                                                                                Name = c.FileUnit.Name,
                                                                                FileType = c.FileUnit.FileType,
                                                                                Description = c.FileUnit.Description,
                                                                                CreatedAt = c.FileUnit.CreatedAt,
                                                                                LastModified = c.FileUnit.LastModified,
                                                                                SpaceId = c.FileUnit.Space.Id
                                                                            },
                                                                            Tags = c.Tags.Select(tag => new TagDto
                                                                            {
                                                                                Id = tag.Id,
                                                                                Name = tag.Name
                                                                            }),
                                                                            Author = new AuthorDto { Id = c.Author.Id, GlobalId = c.Author.GlobalId }
                                                                        })
                                                                    }).ToListAsync();

            Parallel.ForEach(courses,
                course => {
                    Parallel.ForEach(course.Courses,
                        c => { c.Author.Name = authors.FirstOrDefault(a => a.Id == c.Author.GlobalId)?.Name; });
                });

            return courses;
        }

        private async Task<IEnumerable<AppsAPDto>> FilterCourses()
        {
            string userId = _userService.CurrentUserId;
            var courses = await _unitOfWork.AcademyProCourses.Query.Include(c => c.FileUnit).Include(c => c.Tags)
               .Where(c => (c.FileUnit.Space.Type == SpaceType.BinarySpace
               || c.FileUnit.Space.Owner.GlobalId == userId
               || c.FileUnit.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
               || c.FileUnit.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId))) && !c.FileUnit.IsDeleted)
               .GroupBy(c => c.FileUnit.Space)
                 .Select(course => new AppsAPDto
                 {
                     SpaceId = course.Key.Id,
                     SpaceType = course.Key.Type,
                     Name = course.Key.Name,
                     Courses = course.Select(c => new AcademyProCourseDto
                     {
                         Id = c.Id,
                         IsDeleted = c.IsDeleted,
                         StartDate = c.StartDate,
                         Lectures = c.Lectures.Select(lecture => new LectureDto
                         {
                             Id = lecture.Id,
                             Name = lecture.Name,
                             Description = lecture.Description,
                             StartDate = lecture.StartDate,
                             CreatedAt = lecture.CreatedAt,
                             Author = new AuthorDto { Id = lecture.Author.Id, GlobalId = lecture.Author.GlobalId }
                         }),
                         FileUnit = new FileUnitDto
                         {
                             Id = c.FileUnit.Id,
                             Name = c.FileUnit.Name,
                             FileType = c.FileUnit.FileType,
                             Description = c.FileUnit.Description,
                             CreatedAt = c.FileUnit.CreatedAt,
                             LastModified = c.FileUnit.LastModified,
                             SpaceId = c.FileUnit.Space.Id,
                             CanModify = c.Author.GlobalId == userId
                         },
                         Tags = c.Tags.Select(tag => new TagDto
                         {
                             Id = tag.Id,
                             Name = tag.Name
                         }),
                         Author = new AuthorDto { Id = c.Author.Id, GlobalId = c.Author.GlobalId }
                     })
                 }).ToListAsync();

            var authors = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

            Parallel.ForEach(courses,
               course => {
                   Parallel.ForEach(course.Courses,
                       c => { c.Author.Name = authors.FirstOrDefault(a => a.Id == c.Author.GlobalId)?.Name; });
               });

            return courses;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}