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
            client.Connect(IPAddress.Parse(hostname), port);

            Task.Run(() => Listen(client));

            var packetStreamWriter = new PacketStreamWriter(client.GetStream());
            var command = string.Empty;
            do
            {
                if (!String.IsNullOrEmpty(command))
                {
                    packetStreamWriter.Write(command);
                }
                command = Console.ReadLine();
            } while (client.Connected);

            client.Close();
        }

        public static void Listen(TcpClient client)
        {
            var packetStreamReader = new PacketStreamReader(client.GetStream());

            while (client.Connected)
            {
                if (packetStreamReader.HasPacket())
                {
                    var nextPacketCommand = packetStreamReader.GetNextPacketCommand();

                    Console.WriteLine($"Server:\n{nextPacketCommand}");

                    if (nextPacketCommand.StartsWith("Disconnect"))
                    {
                        Console.WriteLine("Disconnected. Press enter to exit.");
                        client.Close();
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}