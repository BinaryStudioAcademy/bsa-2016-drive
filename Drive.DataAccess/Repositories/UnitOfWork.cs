using System;
using System.Threading.Tasks;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Interfaces;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Drive.DataAccess.Entities.Event;

namespace Drive.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DriveContext _context;
        private bool _isDisposed;
        private readonly IRepositoryFactory _repositoryFactory;

        public UnitOfWork(DriveContext context, IRepositoryFactory factory)
        {
            _context = context;
            _repositoryFactory = factory;
        }

        private IRepository<FolderUnit> _folderRepository;
        private IRepository<FileUnit> _fileRepository;
        private IRepository<User> _userRepository;
        private IRepository<Role> _roleRepository;
        private IRepository<Space> _spaceRepository;
        private IRepository<Log> _logRepository;
        private IRepository<AcademyProCourse> _academyProRepository;
        private IRepository<CodeSample> _codeSamplesRepository;
        private IRepository<HomeTask> _homeTasksRepository;
        private IRepository<ContentLink> _contentLinksRepository;
        private IRepository<Tag> _tagsRepository;
        private IRepository<Lecture> _lecturesRepository;
        private IRepository<Shared> _sharedSpaceRepository;
        private IRepository<EventContent> _eventContentRepository;
        private IRepository<Event> _eventRepository;
        private IRepository<ShareLink> _shareLinkRepository;


        public IRepository<FolderUnit> Folders
        {
            get
            {
                return _folderRepository ??
                       (_folderRepository = _repositoryFactory.CreateRepository<FolderUnit>(_context));
            }
        }

        public IRepository<FileUnit> Files
        {
            get
            {
                return _fileRepository ?? (_fileRepository = _repositoryFactory.CreateRepository<FileUnit>(_context));
            }
        }

        public IRepository<User> Users
        {
            get { return _userRepository ?? (_userRepository = _repositoryFactory.CreateRepository<User>(_context)); }
        }

        public IRepository<Role> Roles
        {
            get { return _roleRepository ?? (_roleRepository = _repositoryFactory.CreateRepository<Role>(_context)); }
        }

        public IRepository<Space> Spaces
        {
            get
            {
                return _spaceRepository ?? (_spaceRepository = _repositoryFactory.CreateRepository<Space>(_context));
            }
        }

        public IRepository<Log> Logs
        {
            get
            {
                return _logRepository ?? (_logRepository = _repositoryFactory.CreateRepository<Log>(_context));
            }
        }

        public IRepository<AcademyProCourse> AcademyProCourses
        {
            get
            {
                return _academyProRepository ?? (_academyProRepository = _repositoryFactory.CreateRepository<AcademyProCourse>(_context));
            }
        }


        public IRepository<CodeSample> CodeSamples
        {
            get
            {
                return _codeSamplesRepository ?? (_codeSamplesRepository = _repositoryFactory.CreateRepository<CodeSample>(_context));
            }
        }

        public IRepository<HomeTask> HomeTasks
        {
            get
            {
                return _homeTasksRepository ?? (_homeTasksRepository = _repositoryFactory.CreateRepository<HomeTask>(_context));
            }
        }

        public IRepository<ContentLink> ContentLinks
        {
            get
            {
                return _contentLinksRepository ?? (_contentLinksRepository = _repositoryFactory.CreateRepository<ContentLink>(_context));
            }
        }


        public IRepository<Tag> Tags
        {
            get
            {
                return _tagsRepository ?? (_tagsRepository = _repositoryFactory.CreateRepository<Tag>(_context));
            }
        }


        public IRepository<Lecture> Lectures
        {
            get
            {
                return _lecturesRepository ?? (_lecturesRepository = _repositoryFactory.CreateRepository<Lecture>(_context));
            }
        }

        public IRepository<Shared> SharedSpace
        {
            get
            {
                return _sharedSpaceRepository ?? (_sharedSpaceRepository = _repositoryFactory.CreateRepository<Shared>(_context));
            }
        }

        public IRepository<EventContent> EventContents
        {
            get
            {
                return _eventContentRepository ?? (_eventContentRepository = _repositoryFactory.CreateRepository<EventContent>(_context));
            }
        }

        public IRepository<Event> Events
        {
            get
            {
                return _eventRepository ?? (_eventRepository = _repositoryFactory.CreateRepository<Event>(_context));
            }
        }

        public IRepository<ShareLink> ShareLinks
        {
            get
            {
                return _shareLinkRepository ?? (_shareLinkRepository = _repositoryFactory.CreateRepository<ShareLink>(_context));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _isDisposed = true;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
        }
}
}
