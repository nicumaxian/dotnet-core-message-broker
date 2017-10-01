using System;

namespace Utils.Packets.Exceptions
{
    public class NoPacketException : Exception
    {
        public NoPacketException(string message) : base(message)
        {
        }
    }
}