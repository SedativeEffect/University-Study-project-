using System;
using Microsoft.Extensions.Logging;

namespace module_10.BLL.Utils
{
    public static class LogHelper
    {
        public static void LogAndThrow(this ILogger logger, Exception ex, LogLevel level)
        {
            logger.Log(level, ex, ex.Message);
            throw ex;
        }
    }
}
