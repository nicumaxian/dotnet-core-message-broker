using Broker.Commands.Attributes;
using Microsoft.Extensions.Logging;

namespace Broker.Commands.Handlers
{
    [Command("publish")]
    public class PublishCommandHandler : ICommandHandler
    {
        private readonly ILogger<PublishCommandHandler> _logger;

        public PublishCommandHandler(ILogger<PublishCommandHandler> logger)
        {
            _logger = logger;
        }

        public CommandResponse Run(string[] arguments)
        {
            _logger.LogInformation("Executing publish");

            return CommandResponse.Ok();
        }
    }
}