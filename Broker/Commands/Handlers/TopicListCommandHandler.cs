using System.Linq;
using Broker.Commands.Attributes;
using Broker.Server;
using Broker.Topics.Services;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("topic-list")]
    public class TopicListCommandHandler : ICommandHandler
    {
        private readonly ITopicService _topicService;

        public TopicListCommandHandler(ITopicService topicService)
        {
            _topicService = topicService;
        }

        public Packet Run(string data, ClientContext context)
        {
            var enumerable = _topicService.GetTopics()
                .Select(s => s.Identifier);

            if (enumerable.Any())
            {
                
                var result = enumerable
                    .Aggregate((a, b) => $"{a}\n{b}");
            
                return Packet.Ok(result);
            }
            
            return Packet.Ok();
        }
    }
}