namespace Networking.PackageParser;

public interface IPackageParser
{
    PackageBase ParsePackageFromStream(BinaryReader reader);
    void ParsePackageToStream(PackageBase package, BinaryWriter writer);
}