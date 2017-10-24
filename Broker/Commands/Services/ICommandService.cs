using Broker.Server;
using Broker.Server.Entity;
using Utils.Packets;

namespace Broker.Commands.Services
{
    public interface ICommandService
    {
        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="command">Command as a string</param>
        /// <param name="context">Executing context</param>
        /// <returns>Protocol response</returns>
        Packet Execute(string command,ClientContext context);
    }
}