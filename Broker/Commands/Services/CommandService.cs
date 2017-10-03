using System.Linq;
using System.Text.RegularExpressions;
using Broker.Commands.Exceptions;
using Broker.Server;
using Microsoft.Extensions.Logging;
using Utils.Packets;

namespace Broker.Commands.Services
{
    public class CommandService : ICommandService
    {
        private static readonly Regex ParseRegex = new Regex(@"^([^\s]+){1}\s{0,1}(.*)$", RegexOptions.Compiled);
        private readonly ICommandCollection _commandCollection;
        private readonly ILogger<CommandService> _logger;

        public CommandService(ILogger<CommandService> logger, ICommandCollection commandCollection)
        {
            _logger = logger;
            _commandCollection = commandCollection;
        }

        public Packet Execute(string command, ClientContext context)
        {
            var matchCollection = ParseCommand(command);

            var commandIdentifier = matchCollection[1].Value;
            if (_commandCollection.HasCommand(commandIdentifier))
            {
                var commandHandler = _commandCollection.GetHandler(commandIdentifier);

                return commandHandler.Run(matchCollection[2].Value, context);
            }

            _logger.LogError("Unable to find command : {0}", commandIdentifier);
            throw new CommandExecutionException("Unable to find command", "BAD_COMMAND");
        }

        private GroupCollection ParseCommand(string command)
        {
            _logger.LogDebug("Parsing command \"{0}\"", command);
            var matchCollection = ParseRegex.Matches(command);
            if (!matchCollection.Any())
            {
                _logger.LogError("Command \"{0}\" could not be parsed", command);
                throw new CommandException("Invalid given command");
            }

            return matchCollection[0].Groups;
        }
    }
}