using Broker.Commands.Attributes;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("subscribe")]
    public class SubscribeCommandHandler : ICommandHandler
    {
        public Packet Run(string[] arguments)
        {
            return Packet.Error("Unknown");
        }
    }
}