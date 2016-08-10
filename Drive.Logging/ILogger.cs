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
        void WriteFatal(string message, Type callername);
        void WriteError(Exception e, string message, Type callername);
        void WriteError(string message, Type callername);
        void WriteWarn(string message, Type callername);
        void WriteInfo(string message, Type callername);
        void WriteDebug(string message, Type callername);
        void WriteTrace(string message, Type callername);
    }
}
