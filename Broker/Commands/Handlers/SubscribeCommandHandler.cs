using Broker.Commands.Attributes;

namespace Broker.Commands.Handlers
{
    [Command("subscribe")]
    public class SubscribeCommandHandler : ICommandHandler
    {
        public CommandResponse Run(string[] arguments)
        {
            return CommandResponse.Ok();
        }
    }
}