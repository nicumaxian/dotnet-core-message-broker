using System;
using Broker.Application;
using Broker.Server.Entities;
using Broker.Topics.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Broker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = GetServiceProvider();
            var brokerApp = serviceProvider.GetService<IBrokerApp>();
            
            brokerApp.Start();
        }

        private static IServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
            serviceCollection.AddTransient<IBrokerApp, BrokerApp>();
            serviceCollection.AddTransient<ITopicService, TopicService>();
            serviceCollection.AddTransient<IClient, TcpClient>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}