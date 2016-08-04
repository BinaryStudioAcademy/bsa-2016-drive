using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.DataAccess.Context;
using Drive.DataAccess.Interfaces;
using Ninject.Modules;
using Ninject.Extensions.Factory;
using Drive.DataAccess.Repositories;
using Ninject;
using Ninject.Activation;

namespace Drive.DataAccess.DI
{
    public class DataAccessModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IDataAccessInterface>().To<DataAccessClass>();
            Bind<IRepositoryFactory>().ToFactory();
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument("DriveContext", dc => Kernel.Get<DriveContext>()).WithConstructorArgument("IRepositoryFactory", ir => Kernel.Get<IRepositoryFactory>());
            Bind(typeof(IRepository<>)).To(typeof(Repository<>));
        }

        private object DbContextCallBack(IContext context)
        {
            return Kernel.Get<DriveContext>();
        }
    }
}
