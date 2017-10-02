using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Broker.Commands;
using Broker.Commands.Exceptions;
using Broker.Commands.Services;
using Broker.Core;
using Broker.Topics.Services;
using Microsoft.Extensions.Logging;
using Utils.Extensions;
using Utils.Packets;

namespace Broker.Server.Handlers
{
    public class ConnectionHandler
    {
        private readonly TcpClient _tcpClient;
        private readonly ICommandService _commandService;
        private readonly ITopicService _topicService;
        private readonly ILogger<ConnectionHandler> _logger;
        private readonly PacketStreamReader _packetStreamReader;
        private readonly PacketStreamWriter _packetStreamWriter;
        private readonly ClientContext _clientContext;

        public ConnectionHandler(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _topicService = Container.Resolve<ITopicService>();
            _commandService = Container.Resolve<ICommandService>();
            _logger = Container.Resolve<ILogger<ConnectionHandler>>();
            _packetStreamReader = new PacketStreamReader(_tcpClient.GetStream());
            _packetStreamWriter = new PacketStreamWriter(_tcpClient.GetStream());
            _clientContext = new ClientContext();
            AppendMessageListener();
        }

        public void Listen()
        {
            while (true)
            {
                if (_packetStreamReader.HasPacket())
                {
                    var command = _packetStreamReader.GetNextPacketCommand();
                    _logger.LogDebug("Client {0} sent command \"{1}\"", _tcpClient.Client.RemoteEndPoint, command);
                    Packet result;
                    try
                    {
                        result = _commandService.Execute(command,_clientContext);
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
                CheckMessagesAndSend();
            }
        }

        private void CheckMessagesAndSend()
        {
            if (_clientContext.Messages.Any())
            {
                SendNextMessage();
            }
        }

        private void SendNextMessage()
        {
            if (_clientContext.Messages.TryDequeue(out var topicMessage))
            {
                var packet = Packet.TopicMessage(topicMessage.Topic.Identifier,topicMessage.Content);
            
                _packetStreamWriter.Write(packet);
            }
        }
        
        private void AppendMessageListener()
        {
            _topicService.MessagePublisedEventHandler += _clientContext.OnNewPublishedTopicMessage;
        }

    }
}