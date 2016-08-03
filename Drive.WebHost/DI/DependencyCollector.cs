using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Drive.Core.DI;
using Drive.DataAccess.DI;
using Drive.Identity.DI;
using Ninject;

namespace Drive.WebHost.DI
{
    public class DependencyCollector
    {
        private IKernel _kernel;

        public DependencyCollector()
        {
            //Load modules
            _kernel = new StandardKernel(new CoreModule(), new DataAccessModule(), new IdentityModule());
        }
    }
}