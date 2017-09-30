using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Broker.Server.Exceptions;

namespace Broker.Server
{
    internal class TcpServer : IServer
    {
        private CancellationTokenSource _cancellationTokenSource;

        public void Start(string ipAddress, int port)
        {
            ThrowIfStarted();
            _cancellationTokenSource = new CancellationTokenSource();
            
            var address = IPAddress.Parse(ipAddress);
            var tcpListener = new TcpListener(address,port);
            Task.Run(() => Listen(tcpListener), _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            ThrowIfNotStarted();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }

        private void Listen(TcpListener tcpListener)
        {
            while (true)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                var client = tcpListener.AcceptTcpClient();
            }
        }

        private void ThrowIfStarted()
        {
            if (_cancellationTokenSource != default(CancellationTokenSource))
            {
                throw new AlreadyStartedException("Server is already started");
            }
        }

        private void ThrowIfNotStarted()
        {
            if (_cancellationTokenSource == null)
            {
                throw new NotStartedException("Server was not started");
            }
        }
    }
}