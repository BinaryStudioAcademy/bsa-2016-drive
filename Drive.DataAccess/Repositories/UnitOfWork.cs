using System;
using System.Threading.Tasks;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Entities.Pro;
using Drive.DataAccess.Interfaces;

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
        private IRepository<ContentLink> _contentLinksRepository;
        private IRepository<Tag> _tagsRepository;
        private IRepository<Lecture> _lecturesRepository;

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
            await _context.SaveChangesAsync();
        }
}
}
