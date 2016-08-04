using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (_folderRepository == null)
                    _folderRepository = _repositoryFactory.CreateRepository<FolderUnit>(_context);
                return _folderRepository;
            }
        }

        public IRepository<FileUnit> Files
        {
            get
            {
                if (_fileRepository == null)
                    _fileRepository = _repositoryFactory.CreateRepository<FileUnit>(_context);
                return _fileRepository;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = _repositoryFactory.CreateRepository<User>(_context);
                return _userRepository;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = _repositoryFactory.CreateRepository<Role>(_context);
                return _roleRepository;
            }
        }

        public IRepository<Space> Spaces
        {
            get
            {
                if (_spaceRepository == null)
                    _spaceRepository = _repositoryFactory.CreateRepository<Space>(_context);
                return _spaceRepository;
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

        //public Repository<T> GetRepository<T>() where T : class
        //{
        //    if (_repositories == null)
        //    {
        //        _repositories = new Dictionary<string, object>();
        //    }

        //    var type = typeof(T).Name;

        //    if (!_repositories.ContainsKey(type))
        //    {
        //        var repositoryType = typeof(Repository<>);
        //        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
        //        _repositories.Add(type, repositoryInstance);
        //    }

        //    return (Repository<T>)_repositories[type];
        //}

    }
}
