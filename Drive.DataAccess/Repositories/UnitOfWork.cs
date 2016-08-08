using System;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
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

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
