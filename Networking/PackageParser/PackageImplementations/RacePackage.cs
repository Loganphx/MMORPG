using System.IO;

namespace Networking.PackageParser.PackageImplementations
{
    [PackageType(CommuncationPackage.REALM_REQUEST)]
    public class RealmRequestPackage : PackageBase
    {
        public int RealmId { get; set; }

        public RealmRequestPackage() : base(CommuncationPackage.REALM_REQUEST)
        {
        
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write((int)RealmId);
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            RealmId = reader.ReadInt32();
        }
    }

    [PackageType(CommuncationPackage.REALM_RESPONSE)]
    public class RealmResponsePackage : PackageBase
    {
        public RealmResponsePackage() : base(CommuncationPackage.REALM_RESPONSE)
        {
            
        }
        public int RealmId { get; set; }
        public int CharacterCount { get; set; }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write(RealmId);
            writer.Write(CharacterCount);
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            RealmId = reader.ReadInt32();
            CharacterCount = reader.ReadInt32();
        }
    }
}