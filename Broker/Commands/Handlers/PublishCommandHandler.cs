using Broker.Commands.Attributes;
using Broker.Topics.Services;
using Microsoft.Extensions.Logging;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("publish")]
    public class PublishCommandHandler : ICommandHandler
    {
        private readonly ILogger<PublishCommandHandler> _logger;
        private readonly ITopicService _topicService;

        public PublishCommandHandler(ILogger<PublishCommandHandler> logger)
        {
            _logger = logger;
        }

        public Packet Run(string[] arguments)
        {
            _logger.LogInformation("Executing publish");

            return Packet.Ok();
        }
    }
}