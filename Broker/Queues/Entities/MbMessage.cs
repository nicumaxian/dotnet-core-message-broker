namespace Broker.Queues.Entities
{
    public class MbMessage
    {
        public MbMessage(string queueIdentifier, string content)
        {
            QueueIdentifier = queueIdentifier;
            Content = content;
        }

        public string Content { get; }
        
        public string QueueIdentifier { get; }
    }
}