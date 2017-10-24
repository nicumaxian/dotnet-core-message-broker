using System;
using System.Linq;
using Broker.Commands.Attributes;
using Broker.Queues.Entities;
using Broker.Queues.Services;
using Broker.Server;
using Broker.Server.Entity;
using Microsoft.Extensions.Logging;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("publish")]
    public class PublishCommandHandler : AbstractRegexHandler
    {
        private readonly ILogger<PublishCommandHandler> _logger;
        private readonly IQueueService _queueService;

        public PublishCommandHandler(ILogger<PublishCommandHandler> logger, IQueueService queueService) : base(
            @"^([\w\d\.]*){1}\s{1,}(.*)$")
        {
            _logger = logger;
            _queueService = queueService;
        }

        public override Packet GetData(string[] data, ClientContext context)
        {
            var queue = new MbQueue(data[0]);

            if (_queueService.GetQueues(queue.Identifier).Any())
            {
                _queueService.Publish(new MbMessage(queue.Identifier, data[1], DateTime.UtcNow));

                return Packet.Ok();
            }

            return Packet.Error(Errors.TopicNotFound);
        }
    }
}