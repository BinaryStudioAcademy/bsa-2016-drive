using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;

namespace Drive.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<FolderUnit> Folders { get; }
        IRepository<FileUnit> Files { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Space> Spaces { get; }
        IRepository<Log> Logs { get; }

        Task SaveChangesAsync();
    }
}
