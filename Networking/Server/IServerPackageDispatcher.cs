using Networking.PackageParser;

namespace Networking.Server
{
    public interface IServerPackageDispatcher
    {
        public bool Running { get; }
        public void DispatchPackage(PackageBase package, NetworkConnection networkConnection);
        public void Start();
    }
}