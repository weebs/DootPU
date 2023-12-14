module Dootverse.Server

open System
open System.Text
open WatsonWebsocket
open Thoth.Json.Net

type Server() =
    let mutable clients = Map.empty
    let mutable lobbies = Map.empty
    let server = new WatsonWsServer("127.0.0.1", 8000)
    let agent = MailboxProcessor<Guid * string>.Start <| fun recv -> async {
        while true do
            let! id, msg = recv.Receive()
            let! _ = (server.SendAsync (id, msg) |> Async.AwaitTask)
            ()
    }
    let sendMsg id (msg: Network.Signaling.ServerMessage) =
        agent.Post (id, Encode.Auto.toString msg)
        // task {
        //     let! _ = server.SendAsync (id, Encode.Auto.toString msg)
        //     return ()
        // }
    let broadcastMsg id msg =
        match id with
        | Some id ->
            let clientIds = clients.Keys
            for clientId in clientIds do
                if id <> clientId then
                    sendMsg clientId msg
        | None ->
            let clientIds = clients.Keys
            for clientId in clientIds do
                sendMsg clientId msg
    let getLobbies () =
        lobbies
        |> Seq.map (fun kv -> {| id = kv.Key; name = fst kv.Value; playerCount = 0 |})
        |> Seq.toArray
    let onClientConnected (event: ConnectionEventArgs) =
        printfn $"{event.Client.Guid} connected"
        clients <- clients.Add (event.Client.Guid, event.Client)
        sendMsg event.Client.Guid (Network.Signaling.Lobbies (getLobbies ()))
        |> ignore
        
    let onClientDisconnected (event: DisconnectionEventArgs) =
        // todo: Remove lobby from list
        printfn $"{event.Client.Guid} disconnected"
        clients <- clients.Remove event.Client.Guid
        lobbies <- lobbies.Remove event.Client.Guid
        // for id in clients.Keys do
        //     server.SendAsync (id, Encode.Auto.toString (Network.PlayerDisconnected event.Client.Guid))
        //     |> ignore

    let onMessage (event: MessageReceivedEventArgs) =
        match Decode.Auto.fromString<Network.Signaling.ClientMessage> (Encoding.UTF8.GetString(event.Data)) with
        | Ok message ->
            match message with
            | Network.Signaling.Connect(lobbyId, webRtcRequest) ->
                sendMsg lobbyId (Network.Signaling.ConnectionRequest (event.Client.Guid, webRtcRequest))
                |> ignore
            | Network.Signaling.HostLobby name ->
                lobbies <- lobbies.Add(event.Client.Guid, (name, 0))
            | Network.Signaling.RefreshLobbies ->
                sendMsg event.Client.Guid (Network.Signaling.Lobbies (getLobbies ()))
                |> ignore
            | Network.Signaling.ConnectionResponse (clientId, webRtcRequest) ->
                sendMsg clientId (Network.Signaling.ServerMessage.ConnectionResponse webRtcRequest)
                |> ignore
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