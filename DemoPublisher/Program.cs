using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Utils.Packets;

namespace DemoPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"),31013);

            Task.Run(() => Listen(client));

            var packetStreamWriter = new PacketStreamWriter(client.GetStream());
            var text = string.Empty;
            while (!text.Equals("exit"))
            {
                var command = Console.ReadLine();
                if (command.Length > 1)
                {
                    packetStreamWriter.Write(command);
                }
            }
        }

        public static void Listen(TcpClient client)
        {
            var packetStreamReader = new PacketStreamReader(client.GetStream());

            while (true)
            {
                if (packetStreamReader.HasPacket())
                {
                    var nextPacketCommand = packetStreamReader.GetNextPacketCommand();
                    Console.WriteLine(nextPacketCommand);
                }
                Thread.Sleep(100);
            }
        }
    }
}