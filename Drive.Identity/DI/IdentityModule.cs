using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Drive.Core.HttpClient;

namespace Drive.Identity.DI
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IIdentityInterface>().To<IdentityClass>();
            Kernel.Bind<IHttpClient>().To<AsyncHttpClient>();
        }
    }
}
