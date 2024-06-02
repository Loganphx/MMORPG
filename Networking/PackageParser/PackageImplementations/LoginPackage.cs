namespace Networking.PackageParser.PackageImplementations;

[PackageType(CommuncationPackage.LOGIN_REQUEST)]
public class LoginRequestPackage() : PackageBase(CommuncationPackage.LOGIN_REQUEST) {

    public string Username { get; set; }
    public string Password { get; set; }
    
    public override void SerializeToStream(BinaryWriter writer)
    {
        writer.Write((uint)Id);
        writer.Write(Username);
        writer.Write(Password);
    }

    public override void DeserializeFromStream(BinaryReader reader)
    {
        Username = reader.ReadString();
        Password = reader.ReadString();
    }
}


public class LoginResponsePackage() : PackageBase(CommuncationPackage.LOGIN_RESPONSE)
{
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