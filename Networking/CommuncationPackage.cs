namespace Networking;

public enum CommuncationPackage : uint
{
    LOGIN_REQUEST = 0x001,
    LOGIN_RESPONSE = 0x002,
    
    RACE_REQUEST = 0x003,
    RACE_RESPONSE = 0x004,
    
    CHARACTER_REQUEST = 0x005,
    CHARACTER_RESPONSE = 0x006,
    
    KEEPALIVE = 0xFFFE,
    ERROR = 0xFFFF,

}