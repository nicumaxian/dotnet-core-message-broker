using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Utils.Packets
{
    public class PacketStreamWriter
    {
        private const byte Delimiter = 23; // ETB Character
        private readonly NetworkStream _networkStream;

        public PacketStreamWriter(NetworkStream networkStream)
        {
            _networkStream = networkStream;
        }

        public void Write(Packet packet)
        {
            Write(packet.ToString());
        }
        
        public void Write(string message)
        {
            var buffer = Encoding.ASCII.GetBytes(message);
            
            _networkStream.Write(buffer, 0, buffer.Length);
            _networkStream.WriteByte(Delimiter);
        }
    }
}