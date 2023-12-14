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


type Event =
    | EntityDestroyed of int
    | EnemyDamaged of int
    | PlayerJoined of int * string
    | PlayerDisconnected of int
type Scene(world: world.World) =
    let scene = Network.Scene.createScene ()
    do ()
        // for wall in scene.Walls do
        //     ()
        // for (entity, objects) in scene.GameObjects |> Map.toArray |> Array.groupBy (snd >> fst) do
        //     for id, (t, pos) in objects do
        //         ()
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
    let broadcastMsg idToIgnore msg =
        match idToIgnore with
        | Some id ->
            let clientIds = clientConnection.Keys
            for clientId in clientIds do
                if id <> clientId then
                    sendMsg clientId msg
        | None ->
            let clientIds = clientConnection.Keys
            for clientId in clientIds do
                sendMsg clientId msg
    let onClientMessage (clientGuid: Guid) (message: Network.ClientMessage) = [|
        match message with
        | Network.Update state ->
            let clients = clientConnection.Keys
            for clientId in clients do
                if clientId <> clientGuid then
                    sendMsg clientId (Network.UpdatePlayer (clientGuid, state))
        | Network.DestroyedEntity id ->
            EntityDestroyed id
        | Network.ShotEntity id ->
            console.log ("shot entity", id)
            match scene.GameObjects.TryFind id with
            | Some entity ->
                match entity.data with
                | Network.Enemy (sprite, enemy) ->
                    EnemyDamaged id
                | _else ->
                    console.log _else
            | None ->
                ()
        // todo: Instead of mutating the scene, yield a list of events
        |]
    // let entities = Map<int, Component list>
    member this.AddClient id client =
        clientConnection <- clientConnection.Add (id, client)
    /// Create answer to WebRTC request, initialize connection, and add player connection to list of lobby connections
    member this.ConnectClient request = promise {
        let! c, channel, response = RTC.JS.answerRequest request
        let id = Guid.NewGuid()
        // clientConnection <- clientConnection.Add (id, channel)
        channel.onmessage <- fun ev ->
            try
                let events = onClientMessage id (Decode.Auto.unsafeFromString (string ev.data))
                for event in events do
                    match event with
                    | EnemyDamaged id ->
                        // let ((Network.Enemy (sprite, enemy)), pos) =
                        let entity = scene.GameObjects[id]
                        let (Network.Enemy (sprite, enemy)) = entity.data
                        match enemy with
                        | Network.Zombie zombie ->
                            let state = { zombie with health = zombie.health - 50.0 }
                            if state.health <= 0 then
                                scene <- { scene with GameObjects = scene.GameObjects.Remove(id) }
                                broadcastMsg None (Network.EntityRemoved id) |> ignore
                            else
                                let state = { entity with data = Network.Enemy (sprite, Network.Zombie state) }
                                scene <- { scene with GameObjects = scene.GameObjects.Add (id, state) }
                    | EntityDestroyed id ->
                        console.log ("destroy entity id", id)
                        scene <- { scene with GameObjects = scene.GameObjects.Remove(id) }
                        broadcastMsg None (Network.EntityRemoved id) |> ignore
                    | _else -> console.log _else
                ()
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
        for kv in scene.GameObjects do
            match kv.Value.data with
            | Network.EntityType.Enemy _ ->
                let pos = kv.Value.position
                let r = JS.Math.random()
                if r > 0.1 then
                    // JS.console.log ("ope = " + (string r))
                    // JS.console.log (sprintf "%A" (fst kv.Value), snd kv.Value)
                    let p' = { pos with x = pos.x + 0.01 }
                    broadcastMsg None (Network.ServerMessage.EntityMoved (kv.Key, p'))
                    scene <- { scene with GameObjects = scene.GameObjects.Add(kv.Key, { kv.Value with position = p' }) }
            | _ ->
                ()
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
        let gameLoop () =
            server.Step()
            |> ignore
        let gameLoopId = JS.setInterval gameLoop 20
        c.onmessage <- fun ev ->
            match Decode.Auto.fromString<Network.Signaling.ServerMessage> (string ev.data) with
            | Ok message ->
                match message with
                | Network.Signaling.ServerMessage.ConnectionRequest (clientId, webRtcRequest) ->
                    promise {
                        let! response = server.ConnectClient webRtcRequest
                        c.send (Encode.Auto.toString (Network.Signaling.ClientMessage.ConnectionResponse (clientId, response)))
                    } |> ignore
                | _else ->
                    printfn "%A" _else
            | error -> console.log error
            
        c.send (Encode.Auto.toString (Network.Signaling.ClientMessage.HostLobby "w00tcamp"))
}
