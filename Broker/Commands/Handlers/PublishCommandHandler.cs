using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Broker.Commands.Attributes;
using Broker.Server;
using Broker.Topics.Entities;
using Broker.Topics.Services;
using Microsoft.Extensions.Logging;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("publish")]
    public class PublishCommandHandler : AbstractRegexHandler
    {
        private readonly ILogger<PublishCommandHandler> _logger;
        private readonly ITopicService _topicService;

        public PublishCommandHandler(ILogger<PublishCommandHandler> logger, ITopicService topicService) : base(
            @"^([\w\d\.]*){1}\s{1,}(.*)$")
        {
            _logger = logger;
            _topicService = topicService;
        }


        public override Packet GetData(string[] data, ClientContext context)
        {
            var topic = new Topic(data[0]);

            if (_topicService.GetTopics(topic.Identifier).Any())
            {
                _topicService.Publish(new TopicMessage(topic, data[1]));

                return Packet.Ok();
            }

            return Packet.Error(Errors.TopicNotFound);
        }
    }
}