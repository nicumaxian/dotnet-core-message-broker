using Broker.Commands.Attributes;
using Microsoft.Extensions.Logging;

namespace Broker.Commands
{
    [Command("publish")]
    public class PublishCommandHandler : ICommandHandler
    {
        private readonly ILogger<PublishCommandHandler> _logger;

        public PublishCommandHandler(ILogger<PublishCommandHandler> logger)
        {
            _logger = logger;
        }

        public string Run(string[] arguments)
        {
            _logger.LogInformation("Executing publish");

            return "OK";
        }
    }
}