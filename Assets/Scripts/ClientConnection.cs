using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class ClientConnection
{
    private TcpClient _client;
    
    public BinaryWriter Writer { get; private set; }
    public BinaryReader Reader { get; private set; }

    public ClientConnection()
    {
        Debug.Log("Client connection created.");
    }

    public bool IsConnected => _client?.Connected ?? false;

    public async Task Connect(string host, int port)
    {
        _client = new TcpClient();
        await _client.ConnectAsync(host, port);

        var stream = _client.GetStream();
        Writer = new BinaryWriter(stream);
        Reader = new BinaryReader(stream);
    }

    public Task Disconnect()
    {
        _client.Close();
        Writer = null;
        Reader = null;

        return Task.CompletedTask;
    }
}
