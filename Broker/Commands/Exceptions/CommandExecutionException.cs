using System;

namespace Broker.Commands.Exceptions
{
    public class CommandExecutionException : Exception
    {
        public CommandExecutionException(string message, string protocolError) : base(message)
        {
            ProtocolError = protocolError;
        }

        public string ProtocolError { get; }
    }
}