public class SocketEvent
{
    public readonly string Type;
    public readonly string SubType = null;
    public readonly dynamic Value;

    public SocketEvent(string type, dynamic value)
    {
        Type = type;
        Value = value;
    }
    
    public SocketEvent(string type, string subType, dynamic value)
    {
        Type = type;
        SubType = subType;
        Value = value;
    }
}
