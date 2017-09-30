using Microsoft.Extensions.Logging;

namespace Broker.Application
{
    public class BrokerApp : IBrokerApp
    {
        private readonly ILogger<BrokerApp> _logger;

        public BrokerApp(ILogger<BrokerApp> logger)
        {
            _logger = logger;
        }

        public void Start()
        {
            _logger.LogInformation("Application started");
        }

        public void Stop()
        {
            _logger.LogInformation("Application stoped");
        }
    }
}