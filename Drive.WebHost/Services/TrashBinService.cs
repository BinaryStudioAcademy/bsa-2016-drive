using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Driver.Shared.Dto.TrashBin;
using Driver.Shared.Dto.Users;
using Drive.DataAccess.Interfaces;
using System.Data.Entity;
using Drive.Logging;
using System;
using Driver.Shared.Dto;

namespace Drive.WebHost.Services
{
    public class TrashBinService : ITrashBinService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _userService;
        private readonly ILogger _logger;

        public TrashBinService(IUnitOfWork unitOfWork, IUsersService userService, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IEnumerable<TrashBinDto>> GetTrashBinContentAsync()
        {
            List<TrashBinDto> spacesList = new List<TrashBinDto>();

            try
            {
                string userId = _userService.CurrentUserId;
                
                spacesList = await _unitOfWork.Spaces.Query
                                        .Where(s => s.Owner.GlobalId == userId || s.Type == SpaceType.BinarySpace
                                            || s.ReadPermittedUsers.Any(x => x.GlobalId == userId)
                                            || s.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId)))
                                        .Where(s => s.ContentList.Count(d => d.IsDeleted) > 0)
                                        .Select(s => new TrashBinDto
                    {
                        SpaceId = s.Id,
                        Name = s.Name,
                        Folders = s.ContentList.OfType<FolderUnit>()
                        .Where(f => f.IsDeleted)
                        .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                        .Select(f => new TrashBinFolderDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            CreatedAt = f.CreatedAt,
                            SpaceId = f.Space.Id,
                            Author = new AuthorDto { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        }),
                        Files = s.ContentList.OfType<FileUnit>()
                        .Where(f => f.IsDeleted)
                        .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                        .Select(f => new TrashBinFileDto
                        {
                            Description = f.Description,
                            FileType = f.FileType,
                            Id = f.Id,
                            Name = f.Name,
                            CreatedAt = f.CreatedAt,
                            SpaceId = f.Space.Id,
                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        })
                    }).ToListAsync();

                var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

                foreach (var item in spacesList)
                {
                    Parallel.ForEach(item.Files, file =>
                    { file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name; });

                    Parallel.ForEach(item.Folders, folder =>
                    { folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name; });
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }

            return spacesList;
        }

        public async Task<IEnumerable<TrashBinDto>> SearchTrashBinAsync(string text)
        {
            List<TrashBinDto> searchList = new List<TrashBinDto>();

            try
            {
                string userId = _userService.CurrentUserId;

                searchList = await _unitOfWork.Spaces.Query
                                        .Where(s => s.Owner.GlobalId == userId)
                                        .Where(s => s.ContentList.Count(d => d.IsDeleted) > 0)
                                        .Select(s => new TrashBinDto
                                        {
                                            SpaceId = s.Id,
                                            Name = s.Name,
                                            Folders = s.ContentList.OfType<FolderUnit>()
                        .Where(f => f.IsDeleted)
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                        .Select(f => new TrashBinFolderDto
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Description = f.Description,
                            CreatedAt = f.CreatedAt,
                            SpaceId = f.Space.Id,
                            Author = new AuthorDto { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        }),
                        Files = s.ContentList.OfType<FileUnit>()
                        .Where(f => f.IsDeleted)
                        .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                        .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                        .Select(f => new TrashBinFileDto
                        {
                            Description = f.Description,
                            FileType = f.FileType,
                            Id = f.Id,
                            Name = f.Name,
                            CreatedAt = f.CreatedAt,
                            SpaceId = f.Space.Id,
                            Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                        })
                        }).ToListAsync();

                var owners = (await _userService.GetAllAsync()).Select(f => new { Id = f.id, Name = f.name });

                foreach (var item in searchList)
                {
                    Parallel.ForEach(item.Files, file =>
                    { file.Author.Name = owners.FirstOrDefault(o => o.Id == file.Author.GlobalId)?.Name; });

                    Parallel.ForEach(item.Folders, folder =>
                    { folder.Author.Name = owners.FirstOrDefault(o => o.Id == folder.Author.GlobalId)?.Name; });
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return searchList;
        }

        public async Task RestoreFileAsync(int id)
        {
            try
            {
                await _unitOfWork.Files.Restore(id);
                await _unitOfWork?.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
        }

        public async Task DeleteFileAsync(int id)
        {
            try
            {
                await _unitOfWork.Files.ForceDelete(id);
                await _unitOfWork?.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
        }

        public async Task RestoreFolderAsync(int id)
        {
            try
            {
                var rootFolder = await _unitOfWork.Folders.Deleted.Include(f => f.DataUnits).SingleOrDefaultAsync(f => f.Id == id);

                rootFolder.IsDeleted = false;

                foreach (var item in rootFolder.DataUnits)
                {
                    if (item is FolderUnit)
                    {
                        var folder = await _unitOfWork.Folders.GetByIdDeletedAsync(item.Id);

                        folder.IsDeleted = false;

                        await RestoreFolderAsync(folder.Id);
                    }
                    else
                    {
                        var file = await _unitOfWork.Files.GetByIdDeletedAsync(item.Id);

                        file.IsDeleted = false;
                    }
                }

                await _unitOfWork?.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
        }

        public async Task DeleteFolderAsync(int id)
        {
            try
            {
                var rootFolder = await _unitOfWork.Folders.Deleted.Include(f => f.DataUnits).SingleOrDefaultAsync(f => f.Id == id);

                if (!rootFolder.DataUnits.Any())
                {
                    await _unitOfWork.Folders.ForceDelete(id);
                    await _unitOfWork?.SaveChangesAsync();
                    return;
                }

                for (int i = 0; i < rootFolder.DataUnits.Count; i++)
                {
                    if (rootFolder.DataUnits[i] is FolderUnit)
                    {
                        await DeleteFolderAsync(rootFolder.DataUnits[i].Id);
                        i--;
                    }
                    else
                    {
                        await _unitOfWork.Files.ForceDelete(rootFolder.DataUnits[i].Id);
                        i--;
                    }
                }

                if (!rootFolder.DataUnits.Any())
                {
                    await _unitOfWork.Folders.ForceDelete(id);
                }

                await _unitOfWork?.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
        }

        public async Task RestoreAllFromSpacesAsync(IEnumerable<int> spaces)
        {
            foreach (var spaceId in spaces)
            {
                var files = await _unitOfWork.Files.Deleted
                                                .Where(f => f.Space.Id == spaceId)
                                                .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                                                .ToListAsync();
                var folders = await _unitOfWork.Folders.Deleted
                                                .Where(f => f.Space.Id == spaceId)
                                                .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                                                .ToListAsync();
                foreach (var file in files)
                {
                    await RestoreFileAsync(file.Id);
                }
                foreach (var folder in folders)
                {
                    await RestoreFolderAsync(folder.Id);
                }
            }
        }

        public async Task RestoreContentAsync(CopyMoveContentDto content)
        {
            foreach (var id in content.FilesId)
            {
                await RestoreFileAsync(id);
            }
            foreach (var id in content.FoldersId)
            {
                await RestoreFolderAsync(id);
            }
        }

        public async Task ClearAllFromSpaceAsync(int spaceId)
        {
            var files = await _unitOfWork.Files.Deleted
                                            .Where(f => f.Space.Id == spaceId)
                                            .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                                            .ToListAsync();
            var folders = await _unitOfWork.Folders.Deleted
                                            .Where(f => f.Space.Id == spaceId)
                                            .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                                            .ToListAsync();

            foreach (var file in files) {
                await DeleteFileAsync(file.Id);
            }
            foreach (var folder in folders) {
                await DeleteFolderAsync(folder.Id);
            }

        }

        public async Task ClearTrashBinAsync()
        {
            var trashBinContent = await GetTrashBinContentAsync();
            foreach (var space in trashBinContent)
            {
                foreach (var file in space.Files)
                {
                    await DeleteFileAsync(file.Id);
                }
                foreach (var folder in space.Folders)
                {
                    await DeleteFolderAsync(folder.Id);
                }
            }
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

    }
}