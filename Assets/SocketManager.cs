using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketManager
{
    private static Dictionary<string, SocketManager> _instances = new Dictionary<string, SocketManager>();
    private readonly string _uri;
    private readonly WebSocket _socket;
    private bool _isConnected;
    private readonly bool _isSingleton;

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
            _isConnected = true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to connect to WebSocket '{uri}'");
            Debug.LogException(e);
            _isConnected = false;
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

    protected void OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log($"WebSocket message received: ${e.Data}");
    }

    protected void OnOpen(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connected");
        
        // todo request game id
        _socket.Send("Hi, there!");
    }

    protected void OnClose(object sender, CloseEventArgs e)
    {
        _isConnected = false;
        
        Debug.Log("WebSocket closed");
    }

    protected void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError($"WebSocket error: {e.Message}");
        Debug.LogError(e);
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
