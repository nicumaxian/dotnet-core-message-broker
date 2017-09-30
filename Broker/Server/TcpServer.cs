using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Broker.Server.Exceptions;
using Broker.Server.Handlers;
using Microsoft.Extensions.Logging;

namespace Broker.Server
{
    internal class TcpServer : IServer
    {
        private readonly ILogger<TcpServer> _logger;
        private CancellationTokenSource _cancellationTokenSource;

        public TcpServer(ILogger<TcpServer> logger)
        {
            _logger = logger;
        }

        public void Start(string ipAddress, int port)
        {
            ThrowIfStarted();
            _cancellationTokenSource = new CancellationTokenSource();

            var address = IPAddress.Parse(ipAddress);
            var tcpListener = new TcpListener(address, port);
            Task.Run(() => Listen(tcpListener), _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            ThrowIfNotStarted();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }

        private async Task Listen(TcpListener tcpListener)
        {
            tcpListener.Start();
            _logger.LogInformation("Listening on : {0}", tcpListener.LocalEndpoint);
            while (true)
            {
                try
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    tcpListener.Stop();
                    throw;
                }
                
                _logger.LogDebug("Waiting for client to connect");
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                _logger.LogDebug("Client connected : {0}", tcpClient.Client.RemoteEndPoint);
                var connectionHandler = new ConnectionHandler(tcpClient);
                Task.Run(() => connectionHandler.Listen());
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