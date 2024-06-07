namespace Networking
{
    public enum CommuncationPackage : uint
    {
        LOGIN_REQUEST = 0x001,
        LOGIN_RESPONSE = 0x002,
    
        REALM_LIST_REQUEST = 0x003,
        REALM_LIST_RESPONSE = 0x004,
        
        REALM_REQUEST = 0x005,
        REALM_RESPONSE = 0x006,
    
        CHARACTER_CREATION_REQUEST = 0x007,
        CHARACTER_CREATION_RESPONSE = 0x008,
        
        CHARACTER_REQUEST = 0x009,
        CHARACTER_RESPONSE = 0x0010,
    
        KEEPALIVE = 0xFFFE,
        ERROR = 0xFFFF,

    }
}

