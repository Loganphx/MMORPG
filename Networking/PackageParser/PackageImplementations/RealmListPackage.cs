using System.Collections.Generic;
using System.IO;

namespace Networking.PackageParser.PackageImplementations
{
    [PackageType(CommuncationPackage.REALM_LIST_REQUEST)]
    public class RealmListRequestPackage : PackageBase
    {
        public string Region { get; set; } = "US";

        public RealmListRequestPackage() : base(CommuncationPackage.REALM_LIST_REQUEST)
        {
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write(Region);
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            Region = reader.ReadString();
        }
    }

    [PackageType(CommuncationPackage.REALM_LIST_RESPONSE)]
    public class RealmListResponsePackage : PackageBase
    {
        public RealmListResponsePackage() : base(CommuncationPackage.REALM_LIST_RESPONSE)
        {
        }

        public List<RealmInfo> Realms { get; set; } = new List<RealmInfo>();

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write(Realms.Count);
            foreach (var realmInfo in Realms)
            {
                writer.Write(realmInfo.RealmId);
                writer.Write(realmInfo.RealmName);
            }
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            var realmCount = reader.ReadInt32();
            Realms = new List<RealmInfo>(realmCount);

            for (int i = 0; i < realmCount; i++)
            {
                var realmId = reader.ReadInt32();
                var realmName = reader.ReadString();

                Realms.Add(new RealmInfo()
                {
                    RealmId = realmId,
                    RealmName = realmName
                });
            }
        }
    }

    public class RealmInfo
    {
        public int RealmId;
        public string RealmName;
    }
}