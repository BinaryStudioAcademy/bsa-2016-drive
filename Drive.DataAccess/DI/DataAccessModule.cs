using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace Drive.DataAccess.DI
{
    public class DataAccessModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<IDataAccessInterface>().To<DataAccessClass>();
        }
    }
}
