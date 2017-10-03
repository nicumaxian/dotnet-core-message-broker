using System.Linq;
using Broker.Commands.Attributes;
using Broker.Queues.Services;
using Broker.Server;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("queue-list")]
    public class QueueListCommandHandler : ICommandHandler
    {
        private readonly IQueueService _queueService;

        public QueueListCommandHandler(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public Packet Run(string data, ClientContext context)
        {
            var enumerable = _queueService.GetQueues()
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