using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;

namespace Drive.Logging
{
    public class Logger : NLog.Logger, ILogger
    {
        private readonly NLog.Logger _log;

        public Logger()
        {
            _log = LogManager.GetLogger("TestLoger");
        }

        public void WriteFatal(string log)
        {
            _log.Log(LogLevel.Fatal, log);
        }

        public void WriteError(string log)
        {
            _log.Log(LogLevel.Error, log);
        }

        public void WriteError(Exception e, string log)
        {
            _log.Error(e, log);

        }
        public void WriteError(Exception e, Type type, string log)
        {
            _log.Error().Exception(e).Message(log).Property("ClassName", type.ToString()).Write();
        }

        public void WriteWarn(string log)
        {
            _log.Log(LogLevel.Warn, log);
        }

        public void WriteInfo(string log)
        {
            _log.Log(LogLevel.Info, log);
        }

        public void WriteDebug(string log)
        {
            _log.Log(LogLevel.Debug, log);
        }

        public void WriteTrace(string log)
        {
            _log.Log(LogLevel.Trace, log);
        }
    }
}