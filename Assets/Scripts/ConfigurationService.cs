using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DefaultNamespace
{
    public class ConfigurationService
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public static ConfigurationService CreateInstance()
        {
            return CreateInstance((s) =>
            {

            });
        }

        public static ConfigurationService CreateInstance(Action<IServiceCollection> handler)
        {
            var instance = new ConfigurationService();
            var descriptors = CreateDefaultServiceDescriptors();
            handler(descriptors);

            instance.ServiceProvider = descriptors.BuildServiceProvider();
            return instance;
        }
        private static IServiceCollection CreateDefaultServiceDescriptors()
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddLogging(s => s.AddConsole());
            return serviceDescriptors;
        }
    }
}