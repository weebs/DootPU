module Dootverse.Server

// open Dootverse
open System
open System.Text
open System.Threading
open WatsonWebsocket
open Thoth.Json.Net

let mutable scene = Network.Scene.createScene ()
type Server() =
    let mutable clients = Map.empty
    let server = new WatsonWsServer("127.0.0.1", 8000)
    let sendMsg id (msg: Network.ServerMessage) = task {
        let! _ = server.SendAsync (id, Encode.Auto.toString msg)
        return ()
    }
    let broadcastMsg id msg = task {
        match id with
        | Some id ->
            let clientIds = clients.Keys
            for clientId in clientIds do
                if id <> clientId then
                    do! sendMsg clientId msg
        | None ->
            let clientIds = clients.Keys
            for clientId in clientIds do
                do! sendMsg clientId msg
    }
    let onClientConnected (event: ConnectionEventArgs) =
        printfn $"{event.Client.Guid} connected"
        clients <- clients.Add (event.Client.Guid, event.Client)
        sendMsg event.Client.Guid (Network.WorldState scene)
        |> ignore
        
    let onClientDisconnected (event: DisconnectionEventArgs) =
        printfn $"{event.Client.Guid} disconnected"
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
            | Network.DestroyedEntity id ->
                scene <- { scene with Entities = scene.Entities.Remove(id) }
                broadcastMsg None (Network.EntityRemoved id) |> ignore
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