using Broker.Server;
using Broker.Server.Entity;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    public interface ICommandHandler
    {
        Packet Run(string data, ClientContext context);
    }
}