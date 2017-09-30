using Broker.Commands.Attributes;

namespace Broker.Commands
{
    [Command("subscribe")]
    public class SubscribeCommandHandler : ICommandHandler
    {
        public string Run(string[] arguments)
        {
            return "OK";
        }
    }
}