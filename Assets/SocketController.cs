using SocketData;
using WebSocketSharp;
using UnityEngine;
using Valve.Newtonsoft.Json;

public class SocketController : MonoBehaviour
{
    public static SocketController instance;
    public string WebSocketUrl = "ws://test";
    public WebSocket client;
    private string _gameId;
    
    // Start is called before the first frame update
    void Start()
    {
        using (var ws = new WebSocket(WebSocketUrl))
        {
            ws.OnOpen += (sender, e) => ws.Send("Hello, world");

            ws.Connect();
            ws.Send ("BALUS");
            Debug.Log("message sen");
        }
        
        
        // client.OnMessage += EventManger;
    }

    // private void EventManger(object sender, MessageEventArgs event)
    // {
    //     obj = JsonConvert.SerializeObject(event.Data, product);
    //
    //     if(true)
    // }
    
    public void SendGameState()
    {
        client.Send( JsonConvert.SerializeObject(BuildGameState()));
    }

    private GameState BuildGameState()
    {
        GameState gameStateStatus = new GameState
        {
            TotalEnergy = EnergyController.instance.energy, 
            HasGameComplete = EnergyController.instance.energy < 1
        };

        return gameStateStatus;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}