using System.Collections.Generic;
using Broker.Queues.Entities;

namespace Broker.Queues.Services
{
    public interface IPersistanceService
    {
        void Store(MbMessage message);
        void Remove(MbMessage message);
        ICollection<MbMessage> RestoreAllMessages();
    }
}