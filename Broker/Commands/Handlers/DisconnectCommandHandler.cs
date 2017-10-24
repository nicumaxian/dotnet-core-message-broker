using System.Linq;
using Broker.Commands.Attributes;
using Broker.Queues.Services;
using Broker.Server;
using Broker.Server.Entity;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("disconnect")]
    public class DisconnectCommandHandler : ICommandHandler
    {
        public Packet Run(string data, ClientContext context)
        {
            context.Disconnect();
            
            return Packet.Disconnect();
        }
    }
}