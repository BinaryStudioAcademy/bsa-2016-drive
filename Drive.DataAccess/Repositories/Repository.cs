using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
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
            return this.Entities.Find(id);
        }

        public void Create(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Add(entity);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _context.Entry(entity).State = EntityState.Modified;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var entity = GetById(id);
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this.Entities.Remove(entity);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return Entities;
        }

        private IDbSet<T> Entities => _entities ?? _context.Set<T>();

    }
}


