using System.Linq;
using System.Text.RegularExpressions;
using Broker.Server;
using Broker.Server.Entity;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    public abstract class AbstractRegexHandler : ICommandHandler
    {
        private readonly Regex _regex;

        public AbstractRegexHandler(string regex)
        {
            _regex = new Regex(regex,RegexOptions.Compiled);
        }
        
        public Packet Run(string data, ClientContext context)
        {
            if (_regex.IsMatch(data))
            {
                return GetData(GetParametersFromCommand(data), context);
            }
            
            return Packet.Error(Errors.InvalidArguments);
        }

        public abstract Packet GetData(string[] data, ClientContext context);
        
        private string[] GetParametersFromCommand(string data)
        {
            return _regex.Matches(data)[0]
                .Groups
                .Select(match => match.Value)
                .Skip(1)
                .ToArray();
        }
    }
}