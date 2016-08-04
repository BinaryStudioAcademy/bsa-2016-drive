using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace Drive.Core.DI
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<ICoreInterface>().To<CoreClass>();
        }
    }
}
