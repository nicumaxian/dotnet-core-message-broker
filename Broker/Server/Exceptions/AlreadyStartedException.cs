namespace Broker.Server.Exceptions
{
    public class AlreadyStartedException : ServerException
    {
        public AlreadyStartedException(string message) : base(message)
        {
        }
    }
}