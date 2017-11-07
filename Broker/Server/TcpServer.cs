using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Broker.Server.Exceptions;
using Broker.Server.Handlers;
using Broker.Server.Pool;
using Microsoft.Extensions.Logging;

namespace Broker.Server
{
    internal class TcpServer : IServer
    {
        private readonly ILogger<TcpServer> _logger;
        private readonly IClientPool _clientPool;
        private CancellationTokenSource _cancellationTokenSource;

        public TcpServer(ILogger<TcpServer> logger, IClientPool clientPool)
        {
            _logger = logger;
            _clientPool = clientPool;
        }

        public void Start(string ipAddress, int port)
        {
            ThrowIfStarted();
            var address = IPAddress.Parse(ipAddress);
            var tcpListener = new TcpListener(address, port);
            
            _cancellationTokenSource = new CancellationTokenSource();
            
            Task.Run(() => Listen(_cancellationTokenSource.Token, tcpListener), _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            ThrowIfNotStarted();
            _clientPool.Stop();
            Thread.Sleep(1000); // give a second to send all disconnect messages to clients before stopping tcp server. 
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }

        private async Task Listen(CancellationToken cancellationToken, TcpListener tcpListener)
        {
            tcpListener.Start();
            _clientPool.Start();
            _logger.LogInformation("Listening on : {0}", tcpListener.LocalEndpoint);
            while (true)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    tcpListener.Stop();
                    throw;
                }

                _logger.LogDebug("Waiting for client to connect");
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                _logger.LogDebug("Client connected : {0}", tcpClient.Client.RemoteEndPoint);
                _clientPool.AddClient(tcpClient);
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