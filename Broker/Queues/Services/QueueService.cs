using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Broker.Queues.Entities;
using Microsoft.Extensions.Logging;
using Utils.Extensions;

namespace Broker.Queues.Services
{
    public class QueueService : IQueueService
    {
        private readonly ConcurrentBag<MbQueue> _topics;
        private readonly ILogger<QueueService> _logger;

        public QueueService(ILogger<QueueService> logger)
        {
            _logger = logger;
            _topics = new ConcurrentBag<MbQueue>();
        }

        public void Register(MbQueue mbQueue)
        {
            _topics.Add(mbQueue);
        }

        public IEnumerable<MbQueue> GetQueues()
        {
            return _topics;
        }

        public IEnumerable<MbQueue> GetQueues(string subscription)
        {
            var regex = new Regex(subscription.GlobToRegex(), RegexOptions.Compiled);

            return _topics
                .Where(topic => regex.IsMatch(topic.Identifier));
        }

        public MbMessage GetNextMessage(string subscription)
        {
            var queueOrDefault = GetQueues(subscription)
                .Where(queue => queue.Messages.Any())
                .Select(queue => queue.Messages)
                .FirstOrDefault();

            MbMessage message = null;
            if (queueOrDefault?.TryDequeue(out message) ?? false)
            {
                return message;
            }
            
            return null;
        }

        public void Publish(MbMessage mbMessage)
        {
            _topics.First(queue => queue.Identifier.Equals(mbMessage.QueueIdentifier))
                .Messages
                .Enqueue(mbMessage);
        }

    }
}