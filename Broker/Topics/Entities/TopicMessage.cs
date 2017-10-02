namespace Broker.Topics.Entities
{
    public class TopicMessage
    {
        public TopicMessage(Topic topic, string content)
        {
            Topic = topic;
            Content = content;
        }

        public Topic Topic { get; }
        public string Content { get; }
    }
}