public class SocketEvent
{
    public readonly string clientType = "Server";
    public readonly string type;
    public readonly string subType = null;
    public readonly dynamic value;

    public SocketEvent(string type, dynamic value)
    {
        this.type = type;
        this.value = value;
    }
    
    public SocketEvent(string type, string subType, dynamic value)
    {
        this.type = type;
        this.subType = subType;
        this.value = value;
    }
}
