using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Broker.Commands.Services;
using Broker.Core;
using Broker.Queues.Services;
using Broker.Server.Exceptions;
using Broker.Server.Handlers;
using Microsoft.Extensions.Logging;

namespace Broker.Server.Pool
{
    public class ClientPool : IClientPool
    {
        private readonly ConcurrentBag<ConnectionHandler> _connectionHandlers = new ConcurrentBag<ConnectionHandler>();
        private readonly ILogger<ClientPool> _logger;
        private readonly IQueueService _queueService;
        private CancellationTokenSource _cancellationTokenSource;

        public ClientPool(ILogger<ClientPool> logger)
        {
            _logger = logger;
        }

        public void AddClient(TcpClient client)
        {
            _connectionHandlers.Add(BuildConnectionHandler(client));
        }

        public void Start()
        {
            ThrowIfNotStarted();
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => Run(_cancellationTokenSource.Token));
            _logger.LogDebug("Starting client pool");
        }

        private void ThrowIfNotStarted()
        {
            if (_cancellationTokenSource != default(CancellationTokenSource))
            {
                throw new AlreadyStartedException("Client pool was started already");
            }
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _logger.LogDebug("Stoping client pool");
        }

        private void Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                foreach (var connectionHandler in _connectionHandlers)
                {
                    connectionHandler.CheckStream();
                    CheckAndSendMessage(connectionHandler);
                }
                
                Thread.Sleep(10);
            }
        }

        private void CheckAndSendMessage(ConnectionHandler connectionHandler)
        {
            var connectionHandlerContext = connectionHandler.Context;
            if (!string.IsNullOrEmpty(connectionHandlerContext.Subscription))
            {
                var message = _queueService.GetNextMessage(connectionHandlerContext.Subscription);
                if (message != null)
                {
                    connectionHandler.SendMessage(message);
                }
            }
        }

        private ConnectionHandler BuildConnectionHandler(TcpClient client)
        {
            return new ConnectionHandler(client,Container.Resolve<ICommandService>(),Container.Resolve<ILogger<ConnectionHandler>>());
        }
    }
}