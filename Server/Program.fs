module Dootverse.Server

// open Dootverse
open System
open System.Text
open System.Threading
open WatsonWebsocket
open Thoth.Json.Net

type Server() =
    let mutable clients = Map.empty
    let server = new WatsonWsServer("127.0.0.1", 8000)
    let sendMsg id (msg: Network.ServerMessage) = task {
        let! _ = server.SendAsync (id, Encode.Auto.toString msg)
        return ()
    }
    let onClientConnected (event: ConnectionEventArgs) =
        clients <- clients.Add (event.Client.Guid, event.Client)
        
    let onClientDisconnected (event: DisconnectionEventArgs) =
        clients <- clients.Remove event.Client.Guid
        for id in clients.Keys do
            server.SendAsync (id, Encode.Auto.toString (Network.PlayerDisconnected event.Client.Guid))
            |> ignore

    let onMessage (event: MessageReceivedEventArgs) =
        match Decode.Auto.fromString<Network.ClientMessage> (Encoding.UTF8.GetString(event.Data)) with
        | Ok message ->
            match message with
            | Network.Update state ->
                task {
                    let clients = clients.Keys
                    for clientId in clients do
                        if clientId <> event.Client.Guid then
                            do! sendMsg clientId (Network.UpdatePlayer (event.Client.Guid, state))
                } |> ignore
        | Error err ->
            printfn "%A" err
    do
        server.ClientConnected.Add onClientConnected
        server.MessageReceived.Add onMessage
        server.ClientDisconnected.Add onClientDisconnected
        server.Start()
let server = Server()
let mutable input = Console.ReadLine()
while input <> "q" do
    input <- Console.ReadLine()