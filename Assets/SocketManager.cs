using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json;
using WebSocketSharp;

public class SocketManager
{
    private static Dictionary<string, SocketManager> _instances = new Dictionary<string, SocketManager>();
    private readonly string _uri;
    private readonly WebSocket _socket;
    private bool _isConnected = false;
    private readonly bool _isSingleton;
    
    private Dictionary<string, SocketEvent> events = new Dictionary<string, SocketEvent>();

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
        if (_socket != null && _isConnected)
        {
            try
            {
                _socket.Close();
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

        events[e.type] = e;
    }

    public void Flush()
    {
        if (!IsConnected || events.Count == 0) return;
        string data = toJson(events.Values);
        events.Clear();
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
        Debug.Log($"WebSocket message received: ${e.Data}");
    }

    protected void OnOpen(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connected");

        _isConnected = true;
    }

    protected void OnClose(object sender, CloseEventArgs e)
    {
        _isConnected = false;
        events.Clear();
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
