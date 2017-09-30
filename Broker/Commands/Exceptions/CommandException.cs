using System;

namespace Broker.Commands.Exceptions
{
    public class CommandException : Exception
    {
        public CommandException(string message) : base(message)
        {
            
        }
    }
}