using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Networking;
using Networking.PackageParser;
using UnityEngine;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DefaultNamespace
{
    public class GameContext
    {
        static GameContext()
        {
            ConfigService = ConfigurationService.CreateInstance(s =>
            {
                s.AddSingleton<IPackageParser, PackageParser>();
                s.AddSingleton<NetworkConnection>();
                s.AddSingleton<ClientConnectionDispatcher>();
            });
        }
        
        public static ConfigurationService ConfigService { get; }

        public static IServiceProvider ServiceProvider => ConfigService.ServiceProvider;
        
    }
}