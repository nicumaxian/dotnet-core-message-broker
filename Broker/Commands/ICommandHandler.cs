namespace Broker.Commands
{
    public interface ICommandHandler
    {
        string Run(string[] arguments);
    }
}