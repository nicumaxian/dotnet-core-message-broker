namespace Broker.Server.Exceptions
{
    public class NotStartedException : ServerException
    {
        public NotStartedException(string message) : base(message)
        {
        }
    }
}