using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

            var spacesList = await _unitOfWork.Spaces.Query.Where(s => s.Owner.GlobalId == userId).Select(s => new TrashBinDto
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
                        Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
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
                        Author = new AuthorDto() { Id = f.Owner.Id, GlobalId = f.Owner.GlobalId }
                    })
            }).ToListAsync();

            return spacesList;
        }
    
    }
}