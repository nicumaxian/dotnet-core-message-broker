using System;

namespace Broker.Commands.Attributes
{
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string identifier)
        {
            Identifier = identifier;
        }
        
        public string Identifier { get; }
    }
}