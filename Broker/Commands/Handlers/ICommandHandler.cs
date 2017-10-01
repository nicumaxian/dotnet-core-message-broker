using Utils.Packets;

namespace Broker.Commands.Handlers
{
    public interface ICommandHandler
    {
        Packet Run(string[] arguments);
    }
}