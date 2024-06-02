namespace Networking.PackageParser;

public abstract class PackageBase
{
    public CommuncationPackage Id { get; }

    public PackageBase(CommuncationPackage id)
    {
        Id = id;
    }

    public abstract void SerializeToStream(BinaryWriter writer);

    public abstract void DeserializeFromStream(BinaryReader reader);
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PackageTypeAttribute: Attribute
{
    public CommuncationPackage PackageType { get; }

    public PackageTypeAttribute(CommuncationPackage type)
    {
        PackageType = type;
    }
}