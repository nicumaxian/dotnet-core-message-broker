using System.Text.RegularExpressions;
using Broker.Commands.Attributes;
using Broker.Server;
using Utils.Packets;

namespace Broker.Commands.Handlers
{
    [Command("subscribe")]
    public class SubscribeCommandHandler : AbstractRegexHandler
    {
        public SubscribeCommandHandler() : base(@"^([\w\d\.]*){1}$")
        {
        }

        public override Packet GetData(string[] data, ClientContext context)
        {
            context.Subscribe(data[0]);
            return Packet.Ok();
        }
    }
}