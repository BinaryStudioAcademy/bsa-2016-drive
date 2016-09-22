using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Driver.Shared.Dto;
using Drive.DataAccess.Interfaces;
using Drive.Logging;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using Drive.DataAccess.Entities;
using System.Configuration;
using Driver.Shared.Dto.Users;

namespace Drive.WebHost.Services
{
    public class SharedByLinkService : ISharedByLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IUsersService _userService;
        private readonly ISpaceService _spaceService;

        public SharedByLinkService(IUnitOfWork unitOfWork, ILogger logger, IUsersService userService, ISpaceService spaceService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
            _spaceService = spaceService;
        }


        public async Task<ShareLinkDto> GetShareLinkAsync(int sLinkId)
        {
            var shareLink = await _unitOfWork.ShareLinks.Query.Include(l => l.Content).Where(l => l.Id == sLinkId)
                .Select(l => new ShareLinkDto() {
                    Id = l.Id,
                    Link = l.Link,
                    Files = l.Content.OfType<FileUnit>().Select(c => new FileUnitDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        FileType = c.FileType,
                    }),
                    Folders = l.Content.OfType<FolderUnit>().Select(c => new FolderUnitDto()
                    {
                        Id = c.Id,
                        Name = c.Name
                    })

                }).FirstOrDefaultAsync();

            return shareLink;
        }

        public async Task<string> SetShareLinkAsync(CopyMoveContentDto content)
        {
            var files = await _unitOfWork.Files.Query.Include(f => f.ShareLinks).Where(f => content.FilesId.Contains(f.Id)).ToListAsync();
            var folders = await _unitOfWork.Folders.Query.Include(f => f.ShareLinks).Where(f => content.FoldersId.Contains(f.Id)).ToListAsync();
            List<DataUnit> sharedContent = new List<DataUnit>();
            sharedContent.AddRange(folders);
            sharedContent.AddRange(files);
            ShareLink shareLink = new ShareLink()
            {
                IsDeleted = false,
                Link = GenerateLink(content),
                Content = sharedContent
            };
            _unitOfWork.ShareLinks.Create(shareLink);
            await _unitOfWork.SaveChangesAsync();

            return string.Format("{0}{1}{2}", ConfigurationManager.AppSettings["basePath"], "/shared/", shareLink.Link);
        }

        private string GenerateLink(Object ob)
        {
            string dataSource = string.Format("{0}{1}", ob.GetHashCode().ToString(), DateTime.Now);
            byte[] data = Encoding.Unicode.GetBytes(dataSource);
            SHA1 shaM = new SHA1Managed();
            byte[] hash = shaM.ComputeHash(data);

            string hashStr = string.Empty;
            foreach (byte x in hash)
            {
                hashStr += String.Format("{0:x2}", x);
            }
            return hashStr;
        }

        public async Task<ShareLinkDto> GetContentByLink(string link)
        {
            var shareLink = await _unitOfWork.ShareLinks.Query.Include(l => l.Content).Where(l => l.Link == link)
                .Select(l => new ShareLinkDto()
                {
                    Id = l.Id,
                    Link = l.Link,
                    Files = l.Content.OfType<FileUnit>().Where( c => !(c is ImageUnit)).Select(c => new FileUnitDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        FileType = c.FileType,
                        Link = c.Link
                    }),
                    Images = l.Content.OfType<ImageUnit>().Select(c => new ImageUnitDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        FileType = c.FileType,
                        Link = c.Link,
                        Prev_Link = c.Prev_Link
                    }),
                    Folders = l.Content.OfType<FolderUnit>().Select(c => new FolderUnitDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    })

                }).FirstOrDefaultAsync();

            return shareLink;
        }
    }
}