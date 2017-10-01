using Broker.Commands.Attributes;
using Broker.Server;
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

        public PublishCommandHandler(ILogger<PublishCommandHandler> logger, ITopicService topicService)
        {
            _logger = logger;
            _topicService = topicService;
        }

        public Packet Run(string[] arguments,ClientContext context)
        {
            _logger.LogInformation("Executing publish");

            return Packet.Ok();
        }
    }
}