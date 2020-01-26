using Valve.Newtonsoft.Json;

public class SocketEvent
{
    public  string clientType = "Server";
    public  string type;
    public  string subType = null;
    public  dynamic value;

    [JsonConstructor]
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
