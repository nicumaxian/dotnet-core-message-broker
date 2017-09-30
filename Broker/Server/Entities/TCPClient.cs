namespace Broker.Server.Entities
{
    internal class TcpClient : IClient
    {
        private readonly TcpClient _tcpClient;

        public TcpClient(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }
    }
}