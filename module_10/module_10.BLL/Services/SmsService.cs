using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using module_10.DL.Interfaces;

namespace module_10.BLL.Services
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;

        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }
        public Task NotifyBySms(string number, string message)
        {
            _logger.LogDebug("start sms sending...");
            /// emulations sms sending
            return Task.CompletedTask;
        }
    }
}
