using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using Drive.DataAccess.Interfaces;

namespace Drive.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
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
        public async Task<T> GetByIdAsync(int id)
        {
            return await Entities.SingleOrDefaultAsync(i => i.Id == id);
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

                entity.IsDeleted = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ForceDelete(int id)
        {
            try
            {
                var entity = GetById(id);
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                if( entity.IsDeleted == true)
                    Entities.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Restore(int id)
        {
            try
            {
                var entity = GetById(id);
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                if (entity.IsDeleted == true)
                    entity.IsDeleted = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.Where(x=>x.IsDeleted == false).ToList();
        }

        public IEnumerable<T> GetAllDeleted()
        {
            return Entities.Where(x => x.IsDeleted == true).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.Where(x=>x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllDeletedAsync()
        {
            return await Entities.Where(x=>x.IsDeleted==true).ToArrayAsync();
        }

        protected IDbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        public IQueryable<T> Query => Entities;

    }

}



