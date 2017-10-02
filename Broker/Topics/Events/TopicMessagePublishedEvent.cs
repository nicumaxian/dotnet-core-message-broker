using System;
using Broker.Topics.Entities;

namespace Broker.Topics.Events
{
    public class TopicMessagePublishedEvent : EventArgs
    {
        public TopicMessagePublishedEvent(TopicMessage message)
        {
            Message = message;
        }

        public TopicMessage Message { get; }
    }
}