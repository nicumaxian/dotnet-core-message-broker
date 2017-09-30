using System;

namespace Broker.Server.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message)
        {
            
        }
    }
}