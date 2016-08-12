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

        public Logger(Type type)
        {
            _log = LogManager.GetLogger(type.Name);
        }

        public void WriteFatal(string message)
        {
            _log.Fatal().Message(message).Property("ClassName", _log.Name).Write();
        }

        public void WriteError(string message)
        {
            _log.Error().Message(message).Property("ClassName", _log.Name).Write();
        }

        public void WriteError(Exception e, string message)
        {
            _log.Error().Exception(e).Message(message).Property("ClassName", _log.Name).Write();
        }

        public void WriteWarn(string message)
        {
            _log.Warn().Message(message).Property("ClassName", _log.Name).Write();
        }

        public void WriteInfo(string message)
        {
            _log.Info().Message(message).Property("ClassName", _log.Name).Write();
        }

        public void WriteDebug(string message)
        {
            _log.Debug().Message(message).Property("ClassName", _log.Name).Write();
        }

        public void WriteTrace(string message)
        {
            _log.Trace().Message(message).Property("ClassName", _log.Name).Write();
        }
    }
}