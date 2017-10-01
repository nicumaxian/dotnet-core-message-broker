using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Broker.Commands.Exceptions;
using Broker.Commands.Services;
using Broker.Core;
using Microsoft.Extensions.Logging;

namespace Broker.Server.Handlers
{
    public class ConnectionHandler
    {
        private readonly TcpClient _tcpClient;
        private readonly ICommandService _commandService;
        private readonly ILogger<ConnectionHandler> _logger;

        public ConnectionHandler(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _commandService = Container.Resolve<ICommandService>();
            _logger = Container.Resolve<ILogger<ConnectionHandler>>();
        }

        public void Listen()
        {
            var networkStream = _tcpClient.GetStream();

            while (true)
            {
                if (networkStream.DataAvailable)
                {
                    var command = Read(networkStream);
                    _logger.LogDebug("Client {0} sent command \"{1}\"", _tcpClient.Client.RemoteEndPoint, command);
                    try
                    {
                        Send(networkStream, _commandService.Execute(command));
                    }
                    catch (CommandExecutionException exception)
                    {
                        _logger.LogError(exception, "Error on executing command");
                        Send(networkStream, exception.Message);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "Error on executing");
                        Send(networkStream, "Broker error");
                    }
                }
            }
        }

        private static string Read(NetworkStream networkStream)
        {
            byte[] bytes = new byte[1024];
            var readCount = networkStream.Read(bytes, 0, bytes.Length);
            var command = Encoding.ASCII.GetString(bytes, 0, readCount);
            return command;
        }

        private static void Send(NetworkStream networkStream, string message)
        {
            var buffer = Encoding.ASCII.GetBytes(message);
            networkStream.Write(buffer, 0, buffer.Length);
        }
    }
}