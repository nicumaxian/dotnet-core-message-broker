using Broker.Commands.Handlers;

namespace Broker.Commands
{
    public interface ICommandCollection
    {
        void Register(string command, ICommandHandler handler);

        ICommandHandler GetHandler(string command);

        bool HasCommand(string command);
    }
}