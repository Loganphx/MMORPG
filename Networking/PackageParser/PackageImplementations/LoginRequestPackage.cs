using System.IO;

namespace Networking.PackageParser.PackageImplementations
{
    [PackageType(CommuncationPackage.LOGIN_REQUEST)]
    public class LoginRequestPackage : PackageBase {

        public LoginRequestPackage() : base(CommuncationPackage.LOGIN_REQUEST)
        {
            Username = string.Empty;
            Password = string.Empty;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    
        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write(Username!);
            writer.Write(Password!);
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            Username = reader.ReadString();
            Password = reader.ReadString();
        }
    } 
}

