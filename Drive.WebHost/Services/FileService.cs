using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading.Tasks;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _usersService;

        public FileService(IUnitOfWork unitOfWork, IUsersService usersService)
        {
            _unitOfWork = unitOfWork;
            _usersService = usersService;
        }

        public async Task<IEnumerable<FileUnitDto>> GetAllAsync()
        {
            var files = await _unitOfWork?.Files?.Query.Select(f => new FileUnitDto()
            {
                Id = f.Id,
                Description = f.Description,
                Name = f.Name,
                IsDeleted = f.IsDeleted,
                CreatedAt = f.CreatedAt,
                LastModified = f.LastModified,
                SpaceId = f.Space.Id,
                FileType = f.FileType,
                Link = f.Link
            }).ToListAsync();

            return files;
        }

        public async Task<FileUnitDto> GetAsync(int id)
        {
            var file = await _unitOfWork?.Files?.GetByIdAsync(id);

            if (file != null)
            {
                return new FileUnitDto
                {
                    Id = file.Id,
                    IsDeleted = file.IsDeleted,
                    FileType = file.FileType,
                    Name = file.Name,
                    Description = file.Description,
                    SpaceId = file.Space.Id,
                    Link = file.Link,
                    CreatedAt = file.CreatedAt

                };
            }
            return null;
        }

        public async Task<FileUnitDto> CreateAsync(FileUnitDto dto)
        {
            var user = await _usersService.GetCurrentUser();
            var space = await _unitOfWork?.Spaces?.GetByIdAsync(dto.SpaceId);
            var parentFolder = await _unitOfWork?.Folders.GetByIdAsync(dto.ParentId);

            if (space != null)
            {
                var file = new FileUnit()
                {
                    Name = dto.Name,
                    FileType = dto.FileType,
                    Link = dto.Link,
                    Description = dto.Description,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    IsDeleted = false,
                    Space = space,
                    Parent = parentFolder,
                    Owner = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(u => u.GlobalId == user.serverUserId)
                };


                _unitOfWork?.Files?.Create(file);
                await _unitOfWork?.SaveChangesAsync();

                dto.Id = file.Id;
                dto.CreatedAt = file.CreatedAt;
                dto.LastModified = file.LastModified;
                dto.Author = new AuthorDto() { Id = file.Owner.Id, Name = user.name + ' ' + user.surname };
                dto.FileType = file.FileType;

                return dto;
            }
            return null;
        }

        public async Task<FileUnitDto> UpdateAsync(int id, FileUnitDto dto)
        {
            var file = await _unitOfWork?.Files?.GetByIdAsync(id);

            if (file == null)
                return null;

            file.Name = dto.Name;
            file.FileType = dto.FileType;
            file.Description = dto.Description;
            file.IsDeleted = dto.IsDeleted;
            file.LastModified = DateTime.Now;
            file.Link = dto.Link;

            await _unitOfWork?.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork?.Files?.Delete(id);
            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task<ICollection<AppDto>> FilterApp(FileType fileType)
        {
            var result = await _unitOfWork.Files.Query
               .Where(f => f.FileType == fileType)
                 .GroupBy(f => f.Space).Select(f => new AppDto()
                 {
                     SpaceId = f.Key.Id,
                     Name = f.Key.Name,
                     Files = f.Select(d => new FileUnitDto
                     {
                         Id = d.Id,
                         IsDeleted = d.IsDeleted,
                         FileType = d.FileType,
                         Name = d.Name,
                         Link = d.Link,
                         CreatedAt = d.CreatedAt,
                         Author = new AuthorDto() { Id = d.Owner.Id, GlobalId = d.Owner.GlobalId },
                         Description = d.Description
                     }),
                 }).ToListAsync();

            var owners = (await _usersService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });
            foreach (var item in result)
            {
                Parallel.ForEach(item.Files, file =>
                {
                    file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name;
                });
            }
            return result;
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}