using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Drive.Logging;

namespace Drive.WebHost.Api
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private ILogger _logger;

        public GlobalExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.WriteError("Error from Log method.");
        }
    }
}