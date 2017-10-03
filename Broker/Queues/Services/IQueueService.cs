using System.Collections.Generic;
using Broker.Queues.Entities;

namespace Broker.Queues.Services
{
    public interface IQueueService
    {
        /// <summary>
        /// Register the topic
        /// </summary>
        /// <param name="mbQueue">Topic to be registered</param>
        void Register(MbQueue mbQueue);

        /// <summary>
        /// Gets list of existing queues
        /// </summary>
        /// <returns>Gets the list of existing topics</returns>
        IEnumerable<MbQueue> GetQueues();

        /// <summary>
        /// Gets list of existing queues which matches a regex
        /// </summary>
        /// <param name="subscription">Regex for topics match</param>
        /// <returns>Returns the list of topics matching a regex</returns>
        IEnumerable<MbQueue> GetQueues(string subscription);

        /// <summary>
        /// Returns new message which should be sent by a specific subscription
        /// </summary>
        /// <returns>Message</returns>
        MbMessage GetNextMessage(string subscription);

        void Publish(MbMessage mbMessage);
    }
}