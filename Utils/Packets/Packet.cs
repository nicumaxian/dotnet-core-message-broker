using System.Text;

namespace Utils.Packets
{
    public class Packet
    {
        private readonly StringBuilder _string = new StringBuilder();

        private Packet(Status status)
        {
            _string.AppendFormat("{0}", status);
        }

        public static Packet Disconnect()
        {
            return new Packet(Status.Disconnect);
        }
        
        public static Packet Ok()
        {
            return new Packet(Status.Ok);
        }

        public static Packet Ok(string data)
        {
            return new Packet(Status.Ok)
                .Append(data);
        }

        public static Packet Message(string queue, string message)
        {
            return new Packet(Status.QueueMessage)
                .Append(queue)
                .Append(message);
        }

        public static Packet Error(string reason)
        {
            return new Packet(Status.Error)
                .Append(reason);
        }

        public Packet Append(object value)
        {
            _string.AppendFormat(" {0}", value);
            return this;
        }

        public override string ToString()
        {
            return _string.ToString();
        }

        private enum Status
        {
            Ok = 0,
            Error = 1,
            QueueMessage = 2,
            Disconnect = 3,
        }
    }
}