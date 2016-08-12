using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto;
using Drive.Logging;
using System.Data.Entity;

namespace Drive.WebHost.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public SpaceService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SpaceDto> GetAsync(int id)
        {
            var data = await _unitOfWork.Spaces.GetByIdAsync(id);

            return new SpaceDto
            {
                Name = data.Name,
                Description = data.Description,
                MaxFileSize = data.MaxFileSize,
                MaxFilesQuantity = data.MaxFilesQuantity,
                ReadPermittedUsers = data.ReadPermittedUsers,
                Files = from file in data.ContentList.OfType<FileUnit>()
                        select new FileUnitDto
                        {
                            Name = file.Name,
                            Description = file.Description,
                            Id = file.Id,
                            IsDeleted = file.IsDeleted
                        },
                Folders = from folder in data.ContentList.OfType<FolderUnit>()
                          select new FolderUnitDto
                          {
                              Name = folder.Name,
                              Description = folder.Description,
                              Id = folder.Id,
                              IsDeleted = folder.IsDeleted
                          }
            };
        }

        public async Task<IEnumerable<SpaceDto>> GetAllAsync()
        {
            var data = await _unitOfWork.Spaces.GetAllAsync();

            var dto = from d in data
                      select new SpaceDto
                      {
                          Name = d.Name,
                          Description = d.Description,
                          MaxFileSize = d.MaxFileSize,
                          MaxFilesQuantity = d.MaxFilesQuantity,
                          ReadPermittedUsers = d.ReadPermittedUsers,
                          Files = from file in d.ContentList.OfType<FileUnit>()
                                  select new FileUnitDto
                                  {
                                      Name = file.Name,
                                      Description = file.Description,
                                      Id = file.Id,
                                      IsDeleted = file.IsDeleted
                                  },
                          Folders = from folder in d.ContentList.OfType<FolderUnit>()
                                    select new FolderUnitDto
                                    {
                                        Name = folder.Name,
                                        Description = folder.Description,
                                        Id = folder.Id,
                                        IsDeleted = folder.IsDeleted
                                    }
                      };
            return dto;
        }

        public async Task CreateAsync(SpaceDto dto)
        {
            var space = new Space
            {
                Name = dto.Name,
                Description = dto.Description,
                MaxFilesQuantity = dto.MaxFilesQuantity,
                MaxFileSize = dto.MaxFileSize,
                ReadPermittedUsers = dto.ReadPermittedUsers,
                CreatedAt = DateTime.Now,
                LastModified = DateTime.Now,
                IsDeleted = false
            };
            _unitOfWork.Spaces.Create(space);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, SpaceDto dto)
        {
            var space = await _unitOfWork.Spaces.GetByIdAsync(id);

            space.Name = dto.Name;
            space.Description = dto.Description;
            space.MaxFileSize = dto.MaxFileSize;
            space.MaxFilesQuantity = dto.MaxFilesQuantity;
            space.ReadPermittedUsers = dto.ReadPermittedUsers;
            space.LastModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Spaces.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<SearchResultDto> SearchFoldersAndFilesAsync(int spaceId, int? folderId, string text, int page, int count)
        {
            IEnumerable<FolderUnitDto> resultFolder = new List<FolderUnitDto>();
            IEnumerable<FileUnitDto> resultFiles = new List<FileUnitDto>();
            try
            {
                var space = await _unitOfWork.Spaces.Query
                    .Where(s => s.Id == spaceId)
                    .Select(s => new
                    {
                        Folders = s.ContentList.OfType<FolderUnit>(),
                        Files = s.ContentList.OfType<FileUnit>()
                    }).SingleOrDefaultAsync();
                if (space == null)
                    return null;
                if (folderId != null)
                {
                    var folder = space.Folders.SingleOrDefault(f => f.Id == folderId);
                    if (folder == null)
                        return null;
                    resultFolder = folder.DataUnits.OfType<FolderUnit>()
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Select(f => new FolderUnitDto()
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            IsDeleted = f.IsDeleted
                        });

                    resultFiles = folder.DataUnits.OfType<FileUnit>()
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Select(f => new FileUnitDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            FyleType = f.FileType,
                            IsDeleted = f.IsDeleted
                        });
                }
                else
                {
                    resultFolder = space.Folders
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Select(f => new FolderUnitDto()
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            IsDeleted = f.IsDeleted
                        });
                    resultFiles = space.Files
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Select(f => new FileUnitDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            FyleType = f.FileType,
                            IsDeleted = f.IsDeleted
                        });
                }

                int skipCount = (page - 1) * count;
                if (resultFolder.Count() <= skipCount)
                {
                    skipCount -= resultFolder.Count();
                    resultFolder = new List<FolderUnitDto>();
                    resultFiles = resultFiles.Skip(skipCount).Take(count);
                }
                else
                {
                    resultFolder = resultFolder.Skip(skipCount).Take(count);
                    count -= resultFolder.Count();
                    resultFiles = resultFiles.Take(count);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return new SearchResultDto { Folders = resultFolder.ToList(), Files = resultFiles.ToList() };
        }

        public async Task<int> NumberOfFoundFoldersAndFilesAsync(int spaceId, int? folderId, string text)
        {
            int counter = 0;
            try
            {
                var space = await _unitOfWork.Spaces.Query
                    .Where(s => s.Id == spaceId)
                    .Select(s => new
                    {
                        Folders = s.ContentList.OfType<FolderUnit>(),
                        Files = s.ContentList.OfType<FileUnit>()
                    }).SingleOrDefaultAsync();
                if (space == null)
                    return 0;
                if (folderId != null)
                {
                    var folder = space.Folders.SingleOrDefault(f => f.Id == folderId);
                    if (folder == null)
                        return 0;
                    counter += folder.DataUnits.OfType<FolderUnit>()
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Count();

                    counter += folder.DataUnits.OfType<FileUnit>()
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Count();
                }
                else
                {
                    counter += space.Folders
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Count();
                    counter += space.Files
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Count();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return counter;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}