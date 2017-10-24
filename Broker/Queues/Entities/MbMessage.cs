using System;

namespace Broker.Queues.Entities
{
    public class MbMessage
    {
        public MbMessage(string queueIdentifier, string content, DateTime createdDateTime)
        {
            QueueIdentifier = queueIdentifier;
            Content = content;
            CreatedDateTime = createdDateTime;
        }

        public string Content { get; }
        
        public string QueueIdentifier { get; }
        
        public DateTime CreatedDateTime { get; }
    }
}