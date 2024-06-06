using System;
using Microsoft.Extensions.DependencyInjection;
using Networking.PackageParser;

namespace DefaultNamespace
{
    public class GameContext
    {
        static GameContext()
        {
            ConfigService = ConfigurationService.CreateInstance(s =>
            {
                s.AddSingleton<IPackageParser, PackageParser>();
                s.AddSingleton<ClientConnection>();
                s.AddSingleton<ClientConnectionDispatcher>();
            });
        }
        
        public static ConfigurationService ConfigService { get; }

        public static IServiceProvider ServiceProvider => ConfigService.ServiceProvider;
        
    }
}