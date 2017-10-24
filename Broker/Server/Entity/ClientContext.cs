namespace Broker.Server.Entity
{
    public class ClientContext
    {
        private string _subscription;
        private bool _disconnected;

        public string Subscription => _subscription;

        public bool Disconnected => _disconnected;

        public void Subscribe(string newSubscription)
        {
            _subscription = newSubscription;
        }

        public void Disconnect()
        {
            _disconnected = true;
        }
    }
    
}