using System;
using System.Linq;
using System.Net.Sockets;
using Broker.Commands;
using Broker.Commands.Exceptions;
using Broker.Commands.Services;
using Broker.Core;
using Broker.Queues.Entities;
using Broker.Queues.Services;
using Microsoft.Extensions.Logging;
using Utils.Packets;

namespace Broker.Server.Handlers
{
    public class ConnectionHandler
    {
        private readonly TcpClient _tcpClient;
        private readonly ICommandService _commandService;
        private readonly ILogger<ConnectionHandler> _logger;
        private readonly PacketStreamReader _packetStreamReader;
        private readonly PacketStreamWriter _packetStreamWriter;
        private readonly ClientContext _clientContext;

        public ConnectionHandler(TcpClient tcpClient, ICommandService commandService, ILogger<ConnectionHandler> logger)
        {
            _tcpClient = tcpClient;
            _commandService = commandService;
            _logger = logger;
            _packetStreamReader = new PacketStreamReader(_tcpClient.GetStream());
            _packetStreamWriter = new PacketStreamWriter(_tcpClient.GetStream());
            _clientContext = new ClientContext();
        }

        public void CheckStream()
        {
            if (_packetStreamReader.HasPacket())
            {
                var command = _packetStreamReader.GetNextPacketCommand();
                _logger.LogDebug("Client {0} sent command \"{1}\"", _tcpClient.Client.RemoteEndPoint, command);
                Packet result;
                try
                {
                    result = _commandService.Execute(command, _clientContext);
                }
                catch (CommandExecutionException exception)
                {
                    _logger.LogError(exception, "Error on executing command");
                    result = Packet.Error(exception.ProtocolError);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error on executing command");
                    result = Packet.Error(Errors.ServerError);
                }

                _packetStreamWriter.Write(result);
            }
        }

        public void SendMessage(MbMessage message)
        {
            _packetStreamWriter.Write(Packet.Message(message.QueueIdentifier,message.Content));
        }

        public ClientContext Context => _clientContext;
    }
}