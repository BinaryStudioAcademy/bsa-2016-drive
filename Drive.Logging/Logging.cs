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
        private Logger _log;

        public Logging()
        {

            _log = LogManager.GetLogger("TestLoger");

            //using (DriveContext context = new DriveContext())
            //{
            //    context.LogEntries.Add(new LogEntry());
            //    context.SaveChanges();
            //}
        }

        public void Write(string log)
        {
            _log.Log(LogLevel.Info, log);
        }
    }
}
