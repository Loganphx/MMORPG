using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Networking.PackageParser;
using UnityEngine;

public class ClientConnectionDispatcher
{
    private readonly IPackageParser _packageParser;
    private readonly ClientConnection _clientConnection;

    private Thread _thread;

    private ConcurrentDictionary<string, Tuple<EventWaitHandle, _Holder>> waitingThreads =
        new ConcurrentDictionary<string, Tuple<EventWaitHandle, _Holder>>();

    private event EventHandler<IncomingPackageArgs> IncomingPackage;

    public ClientConnectionDispatcher(IPackageParser packageParser, ClientConnection clientConnection)
    {
        _packageParser = packageParser;
        _clientConnection = clientConnection;

        IncomingPackage += ClientConnectionDispatcher_IncomingPackage;
    }

    private void ClientConnectionDispatcher_IncomingPackage(object sender, IncomingPackageArgs e)
    {
        Debug.Log($"Incoming Package: {e.Package.GetType().Name}");
    }

    public void Start()
    {
        _thread = new Thread(HandlePackageInput);
        _thread.Start();
    }

    private void HandlePackageInput(object obj)
    {
        while (true)
        {
            // Debug.Log("Parsing Package From Stream.");
            var package = _packageParser.ParsePackageFromStream(_clientConnection.Reader);
            IncomingPackage?.Invoke(this, new IncomingPackageArgs(package));
            foreach (var item in waitingThreads)
            {
                if (item.Key.Equals(package.GetType().Name))
                {
                    item.Value.Item2.Value = package;
                    item.Value.Item1.Set();
                
                }
            } 
        }
    }

    public async Task<T> WaitForPackage<T>() where T : PackageBase
    {
        var eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        waitingThreads.TryAdd(typeof(T).Name, new Tuple<EventWaitHandle, _Holder>(eventWaitHandle, new _Holder()));
        var task = await Task.Run(() =>
        {
            eventWaitHandle.WaitOne(TimeSpan.FromSeconds(30));
            waitingThreads.TryRemove(typeof(T).Name, out var tuple);
            return (T)tuple.Item2.Value;

        });
        return task;
    }

    public void SendPackage(PackageBase package)
    {
        _packageParser.ParsePackageToStream(package, _clientConnection.Writer);
    }

    public class _Holder
    {
        public object Value { get; set; }
    }
}

public class IncomingPackageArgs : EventArgs
{
    public PackageBase Package { get; private set; }
    
    public IncomingPackageArgs()
    {
        
    }

    public IncomingPackageArgs(PackageBase package)
    {
        Package = package;
    }
}
