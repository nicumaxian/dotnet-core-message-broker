using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Broker.Topics.Entities;
using Microsoft.Extensions.Logging;

namespace Broker.Topics.Services
{
    public class TopicService : ITopicService
    {
        private readonly ConcurrentBag<Topic> _topics;
        private readonly ILogger<TopicService> _logger;
        
        public TopicService(ILogger<TopicService> logger)
        {
            _logger = logger;
            _topics = new ConcurrentBag<Topic>();
        }

        public void Register(Topic topic)
        {
            _topics.Add(topic);
        }

        public IEnumerable<Topic> GetTopics()
        {
            return _topics;
        }

        public IEnumerable<Topic> GetTopics(string regEx)
        {
            var regex = new Regex(regEx,RegexOptions.Compiled);
            
            return _topics
                .Where(topic => regex.IsMatch(topic.Identifier));
        }
    }
}