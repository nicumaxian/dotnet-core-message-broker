using System.Net.Sockets;

namespace Broker.Server.Pool
{
    public interface IClientPool
    {
        void AddClient(TcpClient client);

        void Start();

        void Stop();
    }
}