using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json;
using Valve.Newtonsoft.Json.Linq;
using WebSocketSharp;

public class SocketManager
{
    private static Dictionary<string, SocketManager> _instances = new Dictionary<string, SocketManager>();
    private readonly string _uri;
    private readonly WebSocket _socket;
    private bool _isConnected = false;
    private readonly bool _isSingleton;
    
    private Dictionary<string, SocketEvent> clientEvents = new Dictionary<string, SocketEvent>();
    private SocketEvent hostEvent;

    public SocketManager(string uri, bool isSingleton = false)
    {
        _uri = uri;
        
        _socket = new WebSocket(_uri) { Log = {Level = LogLevel.Trace } };
        
        _socket.OnMessage += OnMessage;
        _socket.OnOpen += OnOpen;
        _socket.OnClose += OnClose;
        _socket.OnError += OnError;

        try
        {
            _socket.Connect();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to connect to WebSocket '{uri}'");
            Debug.LogException(e);
        }

        _isSingleton = isSingleton;
    }

    ~SocketManager()
    {
        Close();
    }

    public void Close()
    {
        if (_socket != null && _isConnected)
        {
            try
            {
                _socket.CloseAsync();
            }
            finally
            {
                // ignore the error
            }
        }

        if (_isSingleton)
        {
            _instances.Remove(_uri);
        }
    } 

    public bool IsConnected => _isConnected;

    public void AddEvent(SocketEvent e)
    {
        if (!IsConnected)
        {
            return;
        }

        clientEvents[e.type] = e;
    }

    public SocketEvent ConsumeHostEvent()
    {
        if (hostEvent != null)
        {
            var e = hostEvent;
            hostEvent = null;
            return e;
        }

        return null;
    }

    public async void Flush()
    {
        if (!IsConnected || clientEvents.Count == 0) return;
        string data = toJson(clientEvents.Values);
        clientEvents.Clear();
        try
        {
            _socket.SendAsync(data, completed =>
            {
                if (!completed)
                {
                    Debug.LogError($"Failed to send to WebSocket data");
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send to WebSocket data");
            Debug.LogException(e);
        }
    }
    
    protected void OnMessage(object sender, MessageEventArgs e)
    {
        var messages = JsonConvert.DeserializeObject(e.Data);
        
        if (messages == null || messages.GetType() != typeof(JArray) || ((JArray)messages).Count == 0)
        {
            Debug.LogError("Message received is not a non-empty array");
            return;
        }

        SocketEvent gameEvent = null;

        foreach (var item in (JArray)messages)
        {
            try
            {
                gameEvent = item.ToObject<SocketEvent>();
                Debug.Log($"WebSocket message received: ${item}");
                break;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to decode message received item");
                Debug.LogException(ex);
            }
        }

        if (gameEvent != null)
        {
            hostEvent = gameEvent;
        }
    }

    protected void OnOpen(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connected");
        _isConnected = true;
        AddEvent(new SocketEvent("Identify", true));
        Flush();
    }

    protected void OnClose(object sender, CloseEventArgs e)
    {
        _isConnected = false;
        clientEvents.Clear();
        Debug.Log("WebSocket closed");
    }

    protected void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError($"WebSocket error: {e.Message}");
        Debug.LogError(e);
    }

    protected string toJson(dynamic data)
    {
        return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
    }

    public static SocketManager getInstance(string uri = "wss://ldss.xyz") {
        if (!_instances.ContainsKey(uri))
        {
            var instance = new SocketManager(uri, true);
            _instances.Add(uri, instance);
        }
        return _instances[uri];
    }
}
