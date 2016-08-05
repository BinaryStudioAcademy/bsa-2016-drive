using Drive.DataAccess.Context;
using Drive.DataAccess.Interfaces;
using Drive.DataAccess.Repositories;
using Ninject.Extensions.Factory;
using Ninject.Modules;

namespace Drive.DataAccess.Infrastructure
{
    public class DataAccessModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IDataAccessInterface>().To<DataAccessClass>();
            Bind<IRepositoryFactory>().ToFactory();
            Bind<DriveContext>().ToSelf();
            Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}
