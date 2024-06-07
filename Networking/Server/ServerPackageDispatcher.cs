using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Networking.PackageParser;

namespace Networking.Server
{
    public class ServerPackageDispatcher : IServerPackageDispatcher
    {
        private readonly ILogger<ServerPackageDispatcher> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPackageParser _packageParser;

        private readonly BlockingCollection<Tuple<NetworkConnection, PackageBase>> _queue =
            new BlockingCollection<Tuple<NetworkConnection, PackageBase>>();

        public bool Running { get; private set; }

        private Thread? _dispatcherThread;

        public ServerPackageDispatcher(ILogger<ServerPackageDispatcher> logger, IServiceProvider serviceProvider,
            IPackageParser packageParser)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
            this._packageParser = packageParser;

        }

        public void Start()
        {
            _dispatcherThread = new Thread(DispatcherLoop)
            {
                IsBackground = true,
            };
            _dispatcherThread.Start();
        }

        private void DispatcherLoop()
        {
            _logger.LogInformation("Dispatcher started...");
            Running = true;
            try
            {
                while (Running)
                {
                    if (_queue.TryTake(out var item))
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var (connection, package) = item;
                            var connHandler = scope.ServiceProvider
                                .GetRequiredService<ConnectionHandlerBase<NetworkConnection>>();
                            connHandler.InvokeAction(connection, package, package.Id);
                        }
                    }
                }
            }
            finally
            {
                _logger.LogError("Dispatcher stopped");
            }
        }

        public void DispatchPackage(PackageBase package, NetworkConnection connection)
        {
            _queue.Add(new Tuple<NetworkConnection, PackageBase>(connection, package));
        }
    }
}