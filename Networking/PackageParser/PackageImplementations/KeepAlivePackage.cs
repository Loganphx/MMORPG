using System.IO;

namespace Networking.PackageParser.PackageImplementations
{
    [PackageType(CommuncationPackage.KEEPALIVE)]
    public class KeepAlivePackage : PackageBase
    {
        public KeepAlivePackage() : base(CommuncationPackage.KEEPALIVE)
        {
            
        }
        public override void DeserializeFromStream(BinaryReader reader)
        {
        
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
        }
    }
}
