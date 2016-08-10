using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using NLog;
using NLog.Fluent;

namespace Drive.Logging.DI
{
    public class LoggingModule : NinjectModule
    {
        public static string className { get; set; }
        public override void Load()
        {
            //Bind<ILogger>().To<Logger>();
            Bind<ILogger>().ToMethod(x =>
            {
                var className = x.Request.ParentRequest.Service.FullName;
                var log = (ILogger)LogManager.GetLogger(className, typeof(Logger));
                return log;
            });
        }
    }
}
