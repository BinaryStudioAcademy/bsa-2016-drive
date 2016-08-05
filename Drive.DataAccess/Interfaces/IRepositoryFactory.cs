using Drive.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drive.DataAccess.Interfaces
{
    public interface IRepositoryFactory
    {
        Repository<T> CreateRepository<T>(DbContext context) where T : class;
    }
}
