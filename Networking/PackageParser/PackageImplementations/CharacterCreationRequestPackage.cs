using System.IO;

namespace Networking.PackageParser.PackageImplementations
{
    [PackageType(CommuncationPackage.CHARACTER_CREATION_REQUEST)]
    public class CharacterCreationRequestPackage : PackageBase
    {
        public string CharacterName { get; set; }
        public string UmaRecipe { get; set; }
        
        public CharacterCreationRequestPackage() : base(CommuncationPackage.CHARACTER_CREATION_REQUEST)
        {
            
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write(CharacterName);
            writer.Write(UmaRecipe);
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            CharacterName = reader.ReadString();
            UmaRecipe = reader.ReadString();
        }
    }
    
    [PackageType(CommuncationPackage.CHARACTER_CREATION_RESPONSE)]
    public class CharacterCreationResponsePackage : PackageBase
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        
        public CharacterCreationResponsePackage() : base(CommuncationPackage.CHARACTER_CREATION_RESPONSE)
        {
            
        }

        public override void SerializeToStream(BinaryWriter writer)
        {
            writer.Write((uint)Id);
            writer.Write(IsValid);
            writer.Write(ErrorMessage);
        }

        public override void DeserializeFromStream(BinaryReader reader)
        {
            IsValid = reader.ReadBoolean();
            ErrorMessage = reader.ReadString();
        }
    }
}