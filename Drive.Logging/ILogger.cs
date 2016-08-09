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
        void WriteFatal(string log);
        void WriteError(Exception e, Type type, string log);
        void WriteError(Exception e, string log);
        void WriteError(string log);
        void WriteWarn(string log);
        void WriteInfo(string log);
        void WriteDebug(string log);
        void WriteTrace(string log);
    }
}
