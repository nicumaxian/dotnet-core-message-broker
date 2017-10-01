using Broker.Commands.Attributes;
using Broker.Server;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("subscribe")]
    public class SubscribeCommandHandler : ICommandHandler
    {
        public Packet Run(string[] arguments, ClientContext context)
        {
            return Packet.Error("Unknown");
        }
    }
}