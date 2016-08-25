using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drive.DataAccess.Interfaces
{
    public interface IRepository
    { }

    public interface IRepository<T> : IRepository
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdDeletedAsync(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void ForceDelete(int id);
        void Restore(int id);
        IQueryable<T> Query { get; }
        IQueryable<T> Deleted { get; }
    }

}
