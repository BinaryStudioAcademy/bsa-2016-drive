using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drive.Logging.DI;
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

        public void WriteFatal(string message, Type callername)
        {
            _log.Fatal().Message(message).Property("ClassName", callername.Name).Write();
        }

        public void WriteError(string message, Type callername)
        {
            _log.Error().Message(message).Property("ClassName", callername.Name).Write();
        }

        public void WriteError(Exception e, string message, Type callername)
        {
            _log.Error().Exception(e).Message(message).Property("ClassName", callername.Name).Write();
        }

        public void WriteWarn(string message, Type callername)
        {
            _log.Warn().Message(message).Property("ClassName", callername.Name).Write();
        }

        public void WriteInfo(string message, Type callername)
        {
            _log.Info().Message(message).Property("ClassName", callername.Name).Write();
        }

        public void WriteDebug(string message, Type callername)
        {
            _log.Debug().Message(message).Property("ClassName", callername.Name).Write();
        }

        public void WriteTrace(string message, Type callername)
        {
            _log.Trace().Message(message).Property("ClassName", callername.Name).Write();
        }
    }
}