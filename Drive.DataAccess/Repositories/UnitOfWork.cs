using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private bool _isDisposed;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        //public UnitOfWork()
        //{
        //}

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

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (Repository<T>)_repositories[type];
        }

    }
}
