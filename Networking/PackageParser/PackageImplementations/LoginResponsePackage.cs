using System.IO;

namespace Networking.PackageParser.PackageImplementations
{
    [PackageType(CommuncationPackage.LOGIN_RESPONSE)]
    public class LoginResponsePackage : PackageBase
    {
        public LoginResponsePackage() : base(CommuncationPackage.LOGIN_RESPONSE)
        {
            
        }
        
        public bool IsValid { get; set; }
        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write(IsValid);
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            IsValid = reader.ReadBoolean();
        }
    }
}

