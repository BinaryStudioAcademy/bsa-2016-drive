using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Entities.Pro;

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
        IRepository<AcademyProCourse> AcademyProCourses { get; }
        IRepository<CodeSample> CodeSamples { get; }
        IRepository<ContentLink> ContentLinks { get; }
        IRepository<Tag> Tags { get; }
        IRepository<Lecture> Lectures { get; }

        Task SaveChangesAsync();
    }
}
