using Broker.Server;
using Microsoft.Extensions.Logging;

namespace Broker.Application
{
    public class BrokerApp : IBrokerApp
    {
        private readonly ILogger<BrokerApp> _logger;
        private readonly IServer _server;

        public BrokerApp(ILogger<BrokerApp> logger, IServer server)
        {
            _logger = logger;
            _server = server;
        }

        public void Start()
        {
            _logger.LogInformation("Application started");
            // todo : Make it configurable in external file
            _server.Start("127.0.0.1",31013);
        }

        public void Stop()
        {
            _logger.LogInformation("Application stoped");
        }
    }
}