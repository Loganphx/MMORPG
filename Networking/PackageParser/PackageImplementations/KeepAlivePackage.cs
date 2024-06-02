namespace Networking.PackageParser.PackageImplementations;

[PackageType(CommuncationPackage.KEEPALIVE)]
public class KeepAlivePackage() : PackageBase(CommuncationPackage.KEEPALIVE)
{
    public override void DeserializeFromStream(BinaryReader reader)
    {
        
    }

    public override void SerializeToStream(BinaryWriter writer)
    {
        writer.Write((uint)Id);
    }
}