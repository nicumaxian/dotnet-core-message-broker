using Broker.Commands.Attributes;
using Broker.Commands.Services;
using Broker.Server;
using Broker.Topics.Entities;
using Broker.Topics.Services;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("topic-create")]
    public class TopicCreateCommandHandler : AbstractRegexHandler
    {
        private readonly ITopicService _topicService;

        public TopicCreateCommandHandler(ITopicService topicService) : base(@"^([\w\d\.]*){1}$")
        {
            _topicService = topicService;
        }

        public override Packet GetData(string[] data, ClientContext context)
        {
            _topicService.Register(new Topic(data[0]));

            return Packet.Ok();
        }
    }
}