using System;
using Microsoft.Extensions.DependencyInjection;

namespace Broker.Core
{
    public static class Container
    {
        private static IServiceProvider _serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            if(_serviceProvider != null)
                throw new AccessViolationException("Service provider was set already");
            _serviceProvider = serviceProvider;
        }

        public static T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}