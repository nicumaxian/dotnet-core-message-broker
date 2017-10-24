using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Broker.Application;
using Broker.Commands;
using Broker.Commands.Attributes;
using Broker.Commands.Handlers;
using Broker.Commands.Services;
using Broker.Queues.Services;
using Broker.Server;
using Broker.Server.Pool;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utils.Extensions;
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
                var key = Console.ReadKey();

                if (key.KeyChar == 'q')
                {
                    break;
                }
            }
            brokerApp.Stop();
        }

        private static IServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            RegisterCommands(serviceCollection);
            serviceCollection.AddLogging(ConfigureLogging);
            serviceCollection.AddSingleton<IBrokerApp, BrokerApp>();
            serviceCollection.AddSingleton<IQueueService, QueueService>();
            serviceCollection.AddSingleton<IServer, TcpServer>();
            serviceCollection.AddSingleton<ICommandService, CommandService>();
            serviceCollection.AddSingleton<IClientPool, ClientPool>();
            serviceCollection.AddSingleton<IPersistanceService, PersistanceService>();
            serviceCollection.AddSingleton<ICommandCollection>(serviceProvider =>
            {
                var commandCollection = new CommandCollection();

                GetCommandHandlers()
                    .ForEach(type =>
                    {
                        var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
                        commandCollection.Register(commandAttribute.Identifier,
                            (ICommandHandler) serviceProvider.GetService(type));
                    });

                return commandCollection;
            });

            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole()
                .AddDebug();
        }

        private static void RegisterCommands(IServiceCollection serviceCollection)
        {
            GetCommandHandlers()
                .ForEach(type => serviceCollection.AddSingleton(type));
        }

        private static IEnumerable<Type> GetCommandHandlers()
        {
            return Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(type => typeof(ICommandHandler).IsAssignableFrom(type))
                .Where(type => type.GetCustomAttributes<CommandAttribute>().Any());
        }
    }
}