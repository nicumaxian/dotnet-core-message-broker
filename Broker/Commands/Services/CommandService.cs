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
using Container = Broker.Core.Container;

namespace Broker.Commands.Services
{
    public class CommandService : ICommandService
    {
        private static readonly Regex ParseRegex = new Regex(@"([^\s]+)", RegexOptions.Compiled);

        private readonly IDictionary<string, ICommandHandler> _commandHandlers =
            new Dictionary<string, ICommandHandler>();

        private readonly ILogger<CommandService> _logger;

        public CommandService(ILogger<CommandService> logger)
        {
            _logger = logger;
            ScanForCommands();
        }

        public string Execute(string command)
        {
            var matchCollection = ParseCommand(command);

            var commandIdentifier = matchCollection[0].Value;
            if (_commandHandlers.TryGetValue(commandIdentifier, out var handler))
            {
                return handler.Run(matchCollection.Skip(1).Select(match => match.Value).ToArray());
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

        private void ScanForCommands()
        {
            _logger.LogTrace("Start scanning commands");
            Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(type => typeof(ICommandHandler).IsAssignableFrom(type))
                .Where(type => type.GetCustomAttributes<CommandAttribute>().Any())
                .ForEach(RegisterType);
            
            _logger.LogDebug("Loaded {0} commands",_commandHandlers.Count);
        }

        private void RegisterType(Type type)
        {
            _logger.LogDebug("Adding CommandHandler \"{0}\"",type.FullName);
            type.GetCustomAttributes<CommandAttribute>()
                .ForEach(command => _commandHandlers[command.Identifier] = Container.Resolve<ICommandHandler>(type));
        }
    }
}