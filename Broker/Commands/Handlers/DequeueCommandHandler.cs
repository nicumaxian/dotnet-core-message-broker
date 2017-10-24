using System.Linq;
using Broker.Commands.Attributes;
using Broker.Queues.Services;
using Broker.Server;
using Broker.Server.Entity;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("dequeue")]
    public class DequeueCommandHandler : AbstractRegexHandler
    {
        private readonly IQueueService _queueService;

        public DequeueCommandHandler(IQueueService queueService) : base(@"^([\w\d\.\?\*]*){1}$")
        {
            _queueService = queueService;
        }

        public override Packet GetData(string[] data, ClientContext context)
        {
            var nextMessage = _queueService.GetNextMessage(data[0]);

            if (nextMessage == null)
            {
                return Packet.Ok("No items to dequeue");
            }
            return Packet.Message(nextMessage.QueueIdentifier, nextMessage.Content);
        }
    }
}