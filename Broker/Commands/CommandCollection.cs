using System.Collections.Generic;

namespace Broker.Commands
{
    public class CommandCollection : ICommandCollection
    {
        private IDictionary<string, ICommandHandler> _commandHandlers = new Dictionary<string, ICommandHandler>();

        public void Register(string command, ICommandHandler handler)
        {
            _commandHandlers.Add(command,handler);
        }

        public ICommandHandler GetHandler(string command)
        {
            return _commandHandlers[command];
        }

        public bool HasCommand(string command)
        {
            return _commandHandlers.ContainsKey(command);
        }
    }
}