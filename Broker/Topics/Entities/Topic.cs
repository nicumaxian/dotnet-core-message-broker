using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Broker.Topics.Entities
{
    public class Topic
    {
        public string Identifier { get; }

        public ConcurrentQueue<string> Messages { get; }
        
        public Topic(string identifier)
        {
            Identifier = identifier;
            Messages = new ConcurrentQueue<string>();
        }

        public override string ToString()
        {
            return $"Topic({Identifier})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Topic topic)
            {
                return Identifier.Equals(topic.Identifier);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}