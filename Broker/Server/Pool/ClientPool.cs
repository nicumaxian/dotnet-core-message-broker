using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Broker.Commands.Services;
using Broker.Core;
using Broker.Queues.Entities;
using Broker.Queues.Services;
using Broker.Server.Exceptions;
using Broker.Server.Handlers;
using Microsoft.Extensions.Logging;
using Utils.Extensions;

namespace Broker.Server.Pool
{
    public class ClientPool : IClientPool
    {
        private readonly ConcurrentBag<ConnectionHandler> _connectionHandlers = new ConcurrentBag<ConnectionHandler>();
        private readonly ILogger<ClientPool> _logger;
        private readonly IQueueService _queueService;
        private CancellationTokenSource _cancellationTokenSource;

        public ClientPool(ILogger<ClientPool> logger, IQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
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
            _connectionHandlers.ForEach(handler => handler.SendDisconnectNotification());
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _logger.LogDebug("Stoping client pool");
        }

        private void Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var instanceMessages = new HashSet<MbMessage>();
                
                _connectionHandlers.ForEach(connectionHandler => connectionHandler.CheckStream());
                RemoveDisconnectedClients();
                _connectionHandlers.ForEach(handler => SendAvailableMessages(handler, instanceMessages));

                Thread.Sleep(10);
            }
        }


        private void RemoveDisconnectedClients()
        {
            var disconnectedHandlers = _connectionHandlers.Where(h => h.Context.Disconnected);
            var connectedHandlers = _connectionHandlers.Where(h => !h.Context.Disconnected)
                .ToList();

            var count = disconnectedHandlers.Count();
            if (count > 0)
            {
                _logger.LogInformation("Disconnecting {0} clients after requesting", count);
            }
            _connectionHandlers.Clear();
            connectedHandlers.ForEach(_connectionHandlers.Add);
        }

        private void SendAvailableMessages(ConnectionHandler connectionHandler, HashSet<MbMessage> cycleMessagesSent)
        {
            var connectionHandlerContext = connectionHandler.Context;
            if (!string.IsNullOrEmpty(connectionHandlerContext.Subscription))
            {
                var dequeuedMessage = cycleMessagesSent.FirstOrDefault(message =>
                    message.QueueIdentifier.MatchesGlob(connectionHandlerContext.Subscription));
                if (dequeuedMessage != null)
                {
                    connectionHandler.SendMessage(dequeuedMessage);
                }
                else
                {
                    var message = _queueService.GetNextMessage(connectionHandlerContext.Subscription);
                    if (message != null)
                    {
                        connectionHandler.SendMessage(message);
                        cycleMessagesSent.Add(message);
                    }
                }
            }
        }

        private ConnectionHandler BuildConnectionHandler(TcpClient client)
        {
            return new ConnectionHandler(client, Container.Resolve<ICommandService>(),
                Container.Resolve<ILogger<ConnectionHandler>>());
        }
    }
}