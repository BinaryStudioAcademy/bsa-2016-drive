using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.DataAccess.Interfaces
{
    public interface IRepository
    { }

    public interface IRepository<T> : IRepository
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        IQueryable<T> Query { get; }
    }

}
