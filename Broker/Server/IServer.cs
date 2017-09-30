namespace Broker.Server
{
    public interface IServer
    {
        void Start(string ipAddress,int port);
        
        void Stop();
    }
}