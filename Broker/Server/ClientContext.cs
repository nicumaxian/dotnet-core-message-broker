using System.Collections.Concurrent;
using Broker.Queues.Entities;

namespace Broker.Server
{
    public class ClientContext
    {
        private string _subscription;

        public string Subscription => _subscription;

        public void Subscribe(string newSubscription)
        {
            _subscription = newSubscription;
        }
    }
}