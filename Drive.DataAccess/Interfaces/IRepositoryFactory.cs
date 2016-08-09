using Drive.DataAccess.Repositories;
using System.Data.Entity;

namespace Drive.DataAccess.Interfaces
{
    public interface IRepositoryFactory
    {
        Repository<T> CreateRepository<T>(DbContext context) where T : class, IEntity;
    }
}
