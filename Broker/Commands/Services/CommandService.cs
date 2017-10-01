using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Broker.Commands.Attributes;
using Broker.Commands.Exceptions;
using Microsoft.Extensions.Logging;
using Utils.Extensions;
using Utils.Packets;
using Container = Broker.Core.Container;

namespace Broker.Commands.Services
{
    public class CommandService : ICommandService
    {
        private static readonly Regex ParseRegex = new Regex(@"([^\s]+)", RegexOptions.Compiled);
        private readonly ICommandCollection _commandCollection;
        private readonly ILogger<CommandService> _logger;

        public CommandService(ILogger<CommandService> logger, ICommandCollection commandCollection)
        {
            _logger = logger;
            _commandCollection = commandCollection;
        }

        public Packet Execute(string command)
        {
            var matchCollection = ParseCommand(command);

            var commandIdentifier = matchCollection[0].Value;
            if (_commandCollection.HasCommand(commandIdentifier))
            {
                var commandHandler = _commandCollection.GetHandler(commandIdentifier);
                var arguments = matchCollection.Skip(1).Select(match => match.Value).ToArray();

                return commandHandler.Run(arguments);
            }
            
            _logger.LogError("Unable to find command : {0}", commandIdentifier);
            throw new CommandExecutionException("Unable to find command","BAD_COMMAND");
        }

        private MatchCollection ParseCommand(string command)
        {
            _logger.LogDebug("Parsing command \"{0}\"", command);
            var matchCollection = ParseRegex.Matches(command);
            if (!matchCollection.Any())
            {
                _logger.LogError("Command \"{0}\" could not be parsed", command);
                throw new CommandException("Invalid given command");
            }
            
            return matchCollection;
        }
    }
}