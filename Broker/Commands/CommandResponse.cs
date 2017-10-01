namespace Broker.Commands
{
    public class CommandResponse
    {
        private readonly string _status;
        private readonly string _data;

        private CommandResponse(string status, string data)
        {
            _status = status;
            _data = data;
        }

        public override string ToString()
        {
            return $"{_status} {_data}";
        }

        public static CommandResponse Ok(string data = "")
        {
            return new CommandResponse("OK", data);
        }

        public static CommandResponse Error(string data = "")
        {
            return new CommandResponse("ERROR", data);
        }
    }
}