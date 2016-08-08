using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Drive.Logging
{
    public class Logging : ILogging
    {
        public Logger Log { get; }

        public Logging(Logger log)
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(@"C:\FILES\GitHub\bsa-2016-drive\Drive.Logging\NLog.config", true);

            Log = log;

            //using (DriveContext context = new DriveContext())
            //{
            //    context.LogEntries.Add(new LogEntry());
            //    context.SaveChanges();
            //}
        }
    }
}
