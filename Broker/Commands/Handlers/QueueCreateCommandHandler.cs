using Broker.Commands.Attributes;
using Broker.Queues.Entities;
using Broker.Queues.Services;
using Broker.Server;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("queue-create")]
    public class QueueCreateCommandHandler : AbstractRegexHandler
    {
        private readonly IQueueService _queueService;

        public QueueCreateCommandHandler(IQueueService queueService) : base(@"^([\w\d\.]*){1}$")
        {
            _queueService = queueService;
        }

        public override Packet GetData(string[] data, ClientContext context)
        {
            _queueService.Register(new MbQueue(data[0]));

            return Packet.Ok();
        }
    }
}