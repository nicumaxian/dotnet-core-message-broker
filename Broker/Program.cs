using System;
using System.ComponentModel;
using System.Threading;
using Broker.Application;
using Broker.Commands.Services;
using Broker.Server;
using Broker.Topics.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Container = Broker.Core.Container;

namespace Broker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = GetServiceProvider();
            Container.SetServiceProvider(serviceProvider);
            var brokerApp = serviceProvider.GetService<IBrokerApp>();
            
            brokerApp.Start();
            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private static IServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(ConfigureLogging);
            serviceCollection.AddTransient<IBrokerApp, BrokerApp>();
            serviceCollection.AddTransient<ITopicService, TopicService>();
            serviceCollection.AddTransient<IServer, TcpServer>();
            serviceCollection.AddSingleton<ICommandService, CommandService>();

            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole()
                .AddDebug();
        }
        
    }
}