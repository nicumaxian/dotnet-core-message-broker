using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Broker.Topics.Entities;
using Broker.Topics.Events;
using Utils.Extensions;

namespace Broker.Server
{
    public class ClientContext
    {
        private string _subscription;
        private ConcurrentQueue<TopicMessage> _messages = new ConcurrentQueue<TopicMessage>();

        public string Subscription => _subscription;

        public ConcurrentQueue<TopicMessage> Messages => _messages;

        public void Subscribe(string newSubscription)
        {
            Messages.Clear();
            _subscription = newSubscription;
        }
        
        public void OnNewPublishedTopicMessage(object sender, TopicMessagePublishedEvent messagePublishedEvent)
        {
            if (!string.IsNullOrEmpty(Subscription))
            {
                var topicMessage = messagePublishedEvent.Message;
                if (topicMessage.Topic.Identifier.MatchesGlob(_subscription))
                {
                    Messages.Enqueue(topicMessage);
                }
            }
        }
    }
}