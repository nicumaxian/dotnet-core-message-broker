using System;
using System.Collections.Generic;
using Broker.Topics.Entities;

namespace Broker.Topics.Services
{
    public interface ITopicService
    {
        /// <summary>
        /// Register the topic
        /// </summary>
        /// <param name="topic">Topic to be registered</param>
        void Register(Topic topic);

        /// <summary>
        /// Gets list of existing topics
        /// </summary>
        /// <returns>Gets the list of existing topics</returns>
        IEnumerable<Topic> GetTopics();

        /// <summary>
        /// Gets list of existing topics which matches a regex
        /// </summary>
        /// <param name="globPattern">Regex for topics match</param>
        /// <returns>Returns the list of topics matching a regex</returns>
        IEnumerable<Topic> GetTopics(string globPattern);
    }
}