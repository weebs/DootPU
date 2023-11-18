module Dootverse.Game.Server
open System
open Fable.Core
open Fable.Core.JsInterop
open Browser
open Dootverse.Client.JsImports
open Dootverse.Client.Game
open Dootverse
open Thoth.Json

// let WebSocket: Browser.Types.WebSocketType = JsInterop.importMember "ws"

type ServerCmd =
    | AddEnemy
    | AddPlayer
    | DamageEnemy
type GameServer(world: world.World, scene: Network.Scene) =
    let mutable scene = scene
    let mutable clientConnection: Map<Guid, Browser.Types.RTCDataChannel> = Map.empty
    let sendMsg id (msg: Network.ServerMessage) =
        try
            let c = clientConnection[id]
            let mutable i = 0
            let msg = Encode.Auto.toString msg
            while i < msg.Length do
                c.send !^ (msg.Substring(i, Math.Min(msg.Length - i, 1024)))
                i <- i + 1024
            c.send !^ "\r\n"
        with error ->
            JS.debugger ()
            console.log error
    let broadcastMsg id msg =
        match id with
        | Some id ->
            let clientIds = clientConnection.Keys
            for clientId in clientIds do
                if id <> clientId then
                    sendMsg clientId msg
        | None ->
            let clientIds = clientConnection.Keys
            for clientId in clientIds do
                sendMsg clientId msg
    let onMessage (clientGuid: Guid) (message: Network.ClientMessage) =
        match message with
        | Network.Update state ->
            let clients = clientConnection.Keys
            for clientId in clients do
                if clientId <> clientGuid then
                    sendMsg clientId (Network.UpdatePlayer (clientGuid, state))
        | Network.DestroyedEntity id ->
            console.log ("destroy entity id", id)
            scene <- { scene with Entities = scene.Entities.Remove(id) }
            broadcastMsg None (Network.EntityRemoved id) |> ignore
    // let entities = Map<int, Component list>
    member this.AddClient id client =
        clientConnection <- clientConnection.Add (id, client)
    member this.AnswerRequest request = promise {
        let! c, channel, response = RTC.JS.answerRequest request
        let id = Guid.NewGuid()
        // clientConnection <- clientConnection.Add (id, channel)
        channel.onmessage <- fun ev ->
            try
                onMessage id (Decode.Auto.unsafeFromString (string ev.data))
            with error ->
                console.log error
        // c.ondatachannel <- fun ev ->
        // channel.onerror <- fun ev -> console.log ev
        c.ondatachannel <- fun ev -> console.log ev
        c.onconnectionstatechange <- fun ev ->
            if ev.target?connectionState = "disconnected" then
                clientConnection <- clientConnection.Remove id
                broadcastMsg (Some id) (Network.PlayerDisconnected id)
            console.log ev
        channel.onopen <- fun ev ->
            clientConnection <- clientConnection.Add (id, channel)
            // JS.debugger ()
            if ev.currentTarget?readyState = "open" then
                sendMsg id (Network.WorldState scene)
        return response
    }
    member this.AddEnemy() =
        ()
    member this.AddPlayer() = ()
    member this.DamageEnemy() =
        ()
    member this.Step() =
        []

let start () = promise {
    console.log "init"
    do! RAPIER.init ()
    let endpoint =
        document.baseURI
            .Replace("http:", "ws:")
            .Replace("https:", "wss:")
            .Replace("dedicated_server.html", "ws")
            // .Replace("5173", "8000")
    let c = WebSocket.Create endpoint
    c.onopen <- fun ev ->
        let server = GameServer(
            RAPIER.World.Create(RAPIER.Vector3.Create(0, -9.81, 0)),
            Network.Scene.createScene ()
        )
        c.onmessage <- fun ev ->
            match Decode.Auto.fromString<Network.LobbyConnection.ServerMessage> (string ev.data) with
            | Ok message ->
                match message with
                | Network.LobbyConnection.ServerMessage.ConnectionRequest (clientId, webRtcRequest) ->
                    promise {
                        let! response = server.AnswerRequest webRtcRequest
                        c.send (Encode.Auto.toString (Network.LobbyConnection.ClientMessage.ConnectionResponse (clientId, response)))
                    } |> ignore
                | _else ->
                    printfn "%A" _else
            | error -> console.log error
            
        c.send (Encode.Auto.toString (Network.LobbyConnection.ClientMessage.HostLobby "w00tcamp"))
}
