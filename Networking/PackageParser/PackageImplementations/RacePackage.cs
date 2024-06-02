namespace Networking.PackageParser.PackageImplementations;

public class RaceRequestPackage() : PackageBase(CommuncationPackage.RACE_REQUEST)
{
    public int RaceId { get; set; }
    public override void SerializeToStream(BinaryWriter writer)
    {
        writer.Write((uint)Id);
        writer.Write((int)RaceId);
    }

    public override void DeserializeFromStream(BinaryReader reader)
    {
        RaceId = reader.ReadInt32();
    }
}

public class RaceResponsePackage() : PackageBase(CommuncationPackage.RACE_RESPONSE)
{
    public int RaceId { get; set; }
    public override void SerializeToStream(BinaryWriter writer)
    {
        writer.Write((uint)Id);
        writer.Write((int)RaceId);
    }

    public override void DeserializeFromStream(BinaryReader reader)
    {
        RaceId = reader.ReadInt32();
    }
}