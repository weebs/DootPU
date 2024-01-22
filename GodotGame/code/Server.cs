using Godot;
using System;
using Dootverse.Godot3.FSharp.Models;

public class Server : Network.Server
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    // private WebSocketServer _server;
    [Export] public PackedScene Character;
    private Node _character;
    // private Network.Server _server;
    //
    // // Called when the node enters the scene tree for the first time.
    // public override void _Ready()
    // {
    //     var server = new WebSocketServer();
    //     server.Connect("client_connected", this, "ClientConnected");
    //     server.Connect("data_received", this, nameof(DataReceived));
    //     server.Listen(8008);
    //     _character = Character.Instance();
    //     // var root = (Viewport)GetNode("/root");
    //     // var sceneRoot = root.GetChild(0);
    //     
    //     // this.AddChild(_character);
    //     this.AddChild(_character);
    //     // GetTree().Root.GetChild(0).AddChild(_character);
    // }
    //
    // public override void _PhysicsProcess(float delta)
    // {
    //     _server.PhysicsProcess();
    // }
    //
    // public void DataReceived(int id)
    // {
    //     _server.DataReceived(id);
    // }
    //
    // public void ClientConnected(int id, string proto)
    // {
    //     _server.ClientConnected(id, proto);
    //     GD.Print(id);
    //     GD.Print(proto);
    // }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
