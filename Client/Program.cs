using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Utils.Packets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid arguments given");
                Console.WriteLine("Please use 2 arguments when running application : hostname port");
                return;
            }
            TcpClient client = new TcpClient();
            var hostname = args[0];
            var port = Convert.ToInt32(args[1]);
            
            Console.WriteLine($"Connecting to {hostname}:{port}");
            client.Connect(IPAddress.Parse(hostname),port);

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