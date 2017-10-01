namespace Broker.Commands.Handlers
{
    public interface ICommandHandler
    {
        CommandResponse Run(string[] arguments);
    }
}