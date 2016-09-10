using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Driver.Shared.Dto;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Driver.Shared.Dto.Users;
using System.IO;
using System.Web;
using Google.Apis.Drive.v3;

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

        public async Task<IEnumerable<FileUnitDto>> GetAllByParentIdAsync(int spaceId, int? parentId)
        {
            var files = await _unitOfWork.Files.Query.Where(f => f.Space.Id == spaceId)
                                                     .Where(f => f.FolderUnit.Id == parentId)
                                                     .Select(f => new FileUnitDto()
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
            var file = await _unitOfWork.Files.Query.Where(f => f.Id == id).Select(f => new FileUnitDto()
            {
                Id = f.Id,
                IsDeleted = f.IsDeleted,
                FileType = f.FileType,
                Name = f.Name,
                Description = f.Description,
                SpaceId = f.Space.Id,
                Link = f.Link,
                CreatedAt = f.CreatedAt
            }).SingleOrDefaultAsync();

            return file;
        }

        public async Task<FileUnitDto> GetDeletedAsync(int id)
        {
            var file = await _unitOfWork.Files.Deleted.Where(f => f.Id == id).Select(f => new FileUnitDto()
            {
                Id = f.Id,
                IsDeleted = f.IsDeleted,
                FileType = f.FileType,
                Name = f.Name,
                Description = f.Description,
                SpaceId = f.Space.Id,
                Link = f.Link,
                CreatedAt = f.CreatedAt
            }).SingleOrDefaultAsync();

            return file;
        }

        public async Task<FileUnitDto> CreateAsync(FileUnitDto dto)
        {
            var user = await _usersService.GetCurrentUser();
            var localUser = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(x => x.GlobalId == user.serverUserId);

            var space = await _unitOfWork?.Spaces?.GetByIdAsync(dto.SpaceId);
            var parentFolder = await _unitOfWork?.Folders.GetByIdAsync(dto.ParentId);

            List<User> ReadPermittedUsers = new List<User>();

            ReadPermittedUsers.Add(localUser);

            List<User> ModifyPermittedUsers = new List<User>();

            ModifyPermittedUsers.Add(localUser);


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
                    FolderUnit = parentFolder,
                    Owner = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(u => u.GlobalId == user.serverUserId),
                    ReadPermittedUsers = ReadPermittedUsers,
                    ModifyPermittedUsers = ModifyPermittedUsers
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
            if (dto.ParentId != 0)
            {
                file.FolderUnit = await _unitOfWork.Folders.GetByIdAsync(dto.ParentId);
            }
            await _unitOfWork?.SaveChangesAsync();

            return dto;
        }

        public async Task<FileUnitDto> UpdateDeletedAsync(int id, int? oldParentId, FileUnitDto dto)
        {
            var file = await _unitOfWork?.Files?.GetByIdDeletedAsync(id);

            if (file == null)
                return null;

            file.Name = dto.Name;
            file.FileType = dto.FileType;
            file.Description = dto.Description;
            file.IsDeleted = dto.IsDeleted;
            file.LastModified = DateTime.Now;
            file.Link = dto.Link;

            var space = await _unitOfWork.Spaces.GetByIdAsync(dto.SpaceId);

            if (oldParentId != null)
            {
                var oldParentFolder =
                    await
                        _unitOfWork.Folders.Query.Include(f => f.DataUnits)
                            .SingleOrDefaultAsync(f => f.Id == oldParentId);

                var list = new List<DataUnit>();
                foreach (var item in oldParentFolder.DataUnits)
                {
                    if (item.Id != file.Id)
                    {
                        list.Add(item);
                    }
                }

                oldParentFolder.DataUnits = list;
            }

            var parentFolder = await _unitOfWork.Folders.GetByIdAsync(dto.ParentId);

            file.Space = space;
            file.FolderUnit = parentFolder ?? null;

            await _unitOfWork?.SaveChangesAsync();

            return dto;
        }

        public async Task CreateCopyAsync(int id, FileUnitDto dto)
        {
            var file = await _unitOfWork?.Files.Query
                                                .Include(f => f.ModifyPermittedUsers)
                                                .Include(f => f.ReadPermittedUsers)
                                                .Include(f => f.MorifyPermittedRoles)
                                                .Include(f => f.ReadPermittedRoles)
                                                .SingleOrDefaultAsync(f => f.Id == id); ;

            if (file == null)
                return;

            var space = await _unitOfWork.Spaces.GetByIdAsync(dto.SpaceId);

            var user = await _usersService?.GetCurrentUser();

            string name = file.Name;

            if (await _unitOfWork.Files.Query.FirstOrDefaultAsync(f => f.Name == file.Name &&
                                        (f.FolderUnit.Id == dto.ParentId || (dto.ParentId == 0 && f.Space.Id == dto.SpaceId))) != null)
            {
                name = name + "-copy";
            }
            var copy = new FileUnit
            {
                Name = name,
                Description = file.Description,
                FileType = file.FileType,
                IsDeleted = file.IsDeleted,
                LastModified = DateTime.Now,
                CreatedAt = DateTime.Now,
                Link = file.Link,
                Space = space,
                Owner = await _unitOfWork.Users.Query.FirstOrDefaultAsync(u => u.GlobalId == user.serverUserId),
                ModifyPermittedUsers = file.ModifyPermittedUsers,
                ReadPermittedUsers = file.ReadPermittedUsers,
                MorifyPermittedRoles = file.MorifyPermittedRoles,
                ReadPermittedRoles = file.ReadPermittedRoles
            };

            if (dto.ParentId != 0)
            {
                var parent = await _unitOfWork.Folders.GetByIdAsync(dto.ParentId);
                copy.FolderUnit = parent;
            }

            _unitOfWork.Files.Create(copy);

            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _unitOfWork?.Files?.Delete(id);
            await _unitOfWork?.SaveChangesAsync();
        }

        public async Task<int> SearchCourse(int fileId)
        {
            var result = await _unitOfWork.AcademyProCourses.Query.SingleOrDefaultAsync(c => c.FileUnit.Id == fileId);

            return result.Id;
        }

        public async Task<ICollection<AppDto>> FilterApp(FileType fileType)
        {
            string userId = _usersService.CurrentUserId;
            var result = await _unitOfWork.Files.Query
               .Where(f => f.FileType == fileType)
               .Where(f => f.Space.Type == SpaceType.BinarySpace
               || f.Space.Owner.GlobalId == userId
               || f.Space.ReadPermittedUsers.Any(x => x.GlobalId == userId)
               || f.Space.ReadPermittedRoles.Any(x => x.Users.Any(p => p.GlobalId == userId)))
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
                         Description = d.Description,
                         SpaceId = d.Space.Id
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

        public async Task<ICollection<AppDto>> SearchFiles(FileType fileType, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return await FilterApp(fileType);
            }
            else
            {
                var result = await _unitOfWork.Files.Query
               .Where(f => f.FileType == fileType & f.Name.ToLower().Contains(text.ToLower()))
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
        }

        public async Task<string> UploadFile(HttpPostedFile file, int spaceId, int parentId)
        {
            var filename = file.FileName;
            string link = "";
            DriveService service;
            try
            {
                 service = AuthorizationService.ServiceAccountAuthorization();
            }
            catch(Exception e)
            {
                throw e;
            }

            Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
            body.Name = System.IO.Path.GetFileName(filename);
            body.MimeType = GetMimeType(filename);
            body.Parents = null;

            // File's content.
            Stream filestream = file.InputStream;
            byte[] byteArray = ReadFully(filestream);

            MemoryStream stream = new MemoryStream(byteArray);
            try
            {
                FilesResource.CreateMediaUpload request = service.Files.Create(body, stream, GetMimeType(filename));
                await request.UploadAsync();
                link = request.ResponseBody.Id;
            }
            catch (Exception)
            {
                throw new Exception("Failed to upload file to drive!");
            }

            var user = await _usersService.GetCurrentUser();
            var space = await _unitOfWork?.Spaces?.GetByIdAsync(spaceId);
            var parentFolder = await _unitOfWork?.Folders.GetByIdAsync(parentId);

            try
            {
                if (space != null)
                {
                    var fileDto = new FileUnit()
                    {
                        Name = filename,
                        FileType = FileType.Physical,
                        Link = link,
                        Description = "",
                        CreatedAt = DateTime.Now,
                        LastModified = DateTime.Now,
                        IsDeleted = false,
                        Space = space,
                        FolderUnit = parentFolder,
                        Owner = await _unitOfWork?.Users?.Query.FirstOrDefaultAsync(u => u.GlobalId == user.serverUserId)
                    };

                    _unitOfWork?.Files?.Create(fileDto);
                    await _unitOfWork?.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to save file to data base!");
            }
            return "File uploaded successfully. File Id: " + link;
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}