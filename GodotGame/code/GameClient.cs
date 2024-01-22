using Godot;
using System;
using Dootverse.Godot3.FSharp.Models;

public class GameClient : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private WebSocketClient _client;
    [Export] public string ServerEndpoint = "ws://localhost:8008";
    private float pos = 0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _client = new WebSocketClient();
        _client.Connect("connection_established", this, nameof(ConnectionEstablished));
        _client.Connect("data_received", this, nameof(DataReceived));
        _client.ConnectToUrl(ServerEndpoint);
    }

    public void ConnectionEstablished(string proto)
    {
        // _client.GetPeer(1).PutPacket("Hello godot!".ToUTF8());
        var msg = Network.ClientMsg.NewPosition(new Network.float3(0, 0, 0)).Serialize();
        _client.GetPeer(1).PutPacket(msg.ToUTF8());
    }

    public void DataReceived()
    {
        // Godot docs say to always use GetPeer(id)
        var packet = _client.GetPeer(1).GetPacket();
        var data = packet.GetStringFromUTF8();
        GD.Print("message from server:");
        GD.Print(data);
    }

    public override void _PhysicsProcess(float delta)
    {
        pos += 1f * delta;
        var msg = Network.ClientMsg.NewPosition(new Network.float3(pos, pos, pos));
        var packet = msg.Serialize().ToUTF8();
        _client.GetPeer(1).PutPacket(packet);
        _client.Poll();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
