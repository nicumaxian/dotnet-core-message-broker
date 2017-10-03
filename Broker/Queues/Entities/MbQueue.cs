using System.Collections.Concurrent;

namespace Broker.Queues.Entities
{
    public class MbQueue
    {
        public string Identifier { get; }

        public ConcurrentQueue<MbMessage> Messages { get; }
        
        public MbQueue(string identifier)
        {
            Identifier = identifier;
            Messages = new ConcurrentQueue<MbMessage>();
        }

        public override string ToString()
        {
            return $"Topic({Identifier})";
        }

        public override bool Equals(object obj)
        {
            if (obj is MbQueue queue)
            {
                return Identifier.Equals(queue.Identifier);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}