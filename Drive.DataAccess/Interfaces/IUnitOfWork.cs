using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Entities.Event;

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
        IRepository<HomeTask> HomeTasks { get; }
        IRepository<ContentLink> ContentLinks { get; }
        IRepository<Tag> Tags { get; }
        IRepository<Lecture> Lectures { get; }
        IRepository<Shared> SharedSpace { get; }
        IRepository<EventContent> EventContents { get; }
        IRepository<Event> Events { get; }
        IRepository<ShareLink> ShareLinks { get; }
        Task SaveChangesAsync();
    }
}
