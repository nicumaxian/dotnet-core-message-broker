using System.Collections.Generic;
using Broker.Topics.Entities;

namespace Broker.Server
{
    public class ClientContext
    {
        public string Subscription { get; set; }
        
        public Queue<TopicMessage> Messages { get; set; }
    }
}