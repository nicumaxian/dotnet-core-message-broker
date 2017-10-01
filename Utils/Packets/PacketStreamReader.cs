using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Utils.Packets.Exceptions;

namespace Utils.Packets
{
    public class PacketStreamReader
    {
        private const byte Delimiter = 23; // ETB Character
        private readonly NetworkStream _stream;
        private readonly Queue<byte> _buffer = new Queue<byte>();

        public PacketStreamReader(NetworkStream stream)
        {
            _stream = stream;
        }

        public bool HasPacket()
        {
            FlushToBuffer();
            return _buffer.Any(b => b == Delimiter);
        }

        public string GetNextPacketCommand()
        {
            return Encoding.ASCII.GetString(GetNextPacket());
        }

        public byte[] GetNextPacket()
        {
            FlushToBuffer();
            ThrowIfNoPacket();

            IList<byte> bytes = new List<byte>();
            var b = _buffer.Dequeue();
            while (b != Delimiter)
            {
                bytes.Add(b);
                b = _buffer.Dequeue();
            }
            
            return bytes.ToArray();
        }

        private void FlushToBuffer()
        {
            while (_stream.DataAvailable)
            {
                var readByte = _stream.ReadByte();
                _buffer.Enqueue(Convert.ToByte(readByte));
            }
        }

        private void ThrowIfNoPacket()
        {
            if (!HasPacket())
            {
                throw new NoPacketException("No packet in buffer");
            }
        }
    }
}