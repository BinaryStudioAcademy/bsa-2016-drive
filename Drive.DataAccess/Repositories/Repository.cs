using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private IDbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public T GetById(int id)
        {
            return Entities.Find(id);
        }

        public void Create(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                Entities.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                _context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var entity = GetById(id);
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                Entities.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.ToList();
        }

        protected IDbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        public IQueryable<T> Query => Entities;
    }

}



