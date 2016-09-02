using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Driver.Shared.Dto;
using Driver.Shared.Dto.Users;
using Drive.DataAccess.Interfaces;
using System.Data.Entity;

namespace Drive.WebHost.Services
{
    public class TrashBinService : ITrashBinService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _userService;

        public TrashBinService(IUnitOfWork unitOfWork, IUsersService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<IEnumerable<TrashBinDto>> GetTrashBinContentAsync()
        {
            string userId = _userService.CurrentUserId;

            var spacesList = await _unitOfWork.Spaces.Query
                                    .Where(s => s.Owner.GlobalId == userId)
                                    .Where(s => s.ContentList.Count(d => d.IsDeleted) > 0)
                                    .Select(s => new TrashBinDto
                                    {
                                        SpaceId = s.Id,
                                        Name = s.Name,
                                        Folders = s.ContentList.OfType<FolderUnit>()
                    .Where(f => f.IsDeleted)
                    .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                    .Select(f => new FolderUnitDto
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Description = f.Description,
                        CreatedAt = f.CreatedAt,
                        IsDeleted = f.IsDeleted,
                        SpaceId = f.Space.Id,
                        Author = new AuthorDto { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                    }),
                                        Files = s.ContentList.OfType<FileUnit>()
                    .Where(f => f.IsDeleted)
                    .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                    .Select(f => new FileUnitDto
                    {
                        Description = f.Description,
                        FileType = f.FileType,
                        Id = f.Id,
                        IsDeleted = f.IsDeleted,
                        Name = f.Name,
                        CreatedAt = f.CreatedAt,
                        Link = f.Link,
                        SpaceId = f.Space.Id,
                        Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                    })
                                    }).ToListAsync();

            return spacesList;
        }

        public async Task<IEnumerable<TrashBinDto>> SearchTrashBinAsync(string text)
        {
            string userId = _userService.CurrentUserId;

            var searchList = await _unitOfWork.Spaces.Query
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
                    .Select(f => new FolderUnitDto
                    {
                        Id = f.Id,
                        Name = f.Name,
                        Description = f.Description,
                        CreatedAt = f.CreatedAt,
                        IsDeleted = f.IsDeleted,
                        SpaceId = f.Space.Id,
                        Author = new AuthorDto { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                    }),
                                        Files = s.ContentList.OfType<FileUnit>()
                    .Where(f => f.IsDeleted)
                    .Where(f => f.Name.ToLower().Contains(text.ToLower()))
                    .Where(f => (f.FolderUnit == null) || (!f.FolderUnit.IsDeleted))
                    .Select(f => new FileUnitDto
                    {
                        Description = f.Description,
                        FileType = f.FileType,
                        Id = f.Id,
                        IsDeleted = f.IsDeleted,
                        Name = f.Name,
                        CreatedAt = f.CreatedAt,
                        Link = f.Link,
                        SpaceId = f.Space.Id,
                        Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                    })
                                    }).ToListAsync();

            foreach (var item in searchList)
            {
                Parallel.ForEach(item.Files, file =>
                {   
                });
                Parallel.ForEach(item.Folders, folder =>
                {
                });
            }
            return searchList;
        }

        public async Task RestoreFileAsync(int id)
        {
            await _unitOfWork.Files.Restore(id);
            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task DeleteFileAsync(int id)
        {
            await _unitOfWork.Files.ForceDelete(id);
            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task RestoreFolderAsync(int id)
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

        public async Task DeleteFolderAsync(int id)
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


        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

    }
}