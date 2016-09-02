using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Drive.Logging
{
    public interface ILogger
    {
        void WriteFatal(string message);
        void WriteError(Exception e, string message);
        void WriteError(string message);
        void WriteWarn(string message);
        void WriteInfo(string message);
        void WriteDebug(string message);
        void WriteTrace(string message);
    }
}
