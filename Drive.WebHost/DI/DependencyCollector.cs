using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drive.Core.DI;
using Drive.DataAccess.Context;
using Drive.DataAccess.DI;
using Drive.DataAccess.Interfaces;
using Drive.DataAccess.Repositories;
using Drive.Identity.DI;
using Ninject;
using Ninject.Parameters;

namespace Drive.WebHost.DI
{
    public class DependencyCollector
    {
        private IKernel _kernel;
        public IRepositoryFactory RepositoryFactory;
        public IUnitOfWork UnitOfWork;


        public DependencyCollector(DriveContext dc)
        {
            //Load modules
            _kernel = new StandardKernel(new CoreModule(), new DataAccessModule(), new IdentityModule());

            RepositoryFactory = _kernel.Get<IRepositoryFactory>();
            var context = new ConstructorArgument("DriveContext", dc);
            var factory = new ConstructorArgument("IRepositoryFactory", RepositoryFactory);
            UnitOfWork = _kernel.Get<IUnitOfWork>(context, factory);
        }
    }
}