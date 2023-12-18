module Dootverse.Game.Server
open System
open Dootverse.Network.Signaling
open Fable.Core
open Fable.Core.JsInterop
open Browser
open Dootverse.Client.JsImports
open Dootverse
open Dootverse.Network
open Thoth.Json

// let WebSocket: Browser.Types.WebSocketType = JsInterop.importMember "ws"
type GameServer(world: world.World, scene: Scene) =
    // let mutable scene = scene
    let game = Game.Game(world, 800, 600, true)
    do
        game.Load_Scene scene
    let scene = ()
    let nextClientId =
        let mutable id = 0
        fun () ->
            id <- id + 1
            id - 1
    let mutable clientConnection: Map<int, Browser.Types.RTCDataChannel> = Map.empty
    /// Return an array of events that occur due to the message
    let onClientMessage (clientGuid: int) (message: ClientMessage) = [|
        match message with
        | ClientMessage.Update state ->
            PlayerUpdated (clientGuid, state) 
        | ClientMessage.DestroyedEntity id ->
            EntityDestroyed id
        | ClientMessage.ShotEntity id ->
            console.log ("shot entity", id)
            match game.Scene.GameObjects.TryFind id with
            | Some entity ->
                match entity.data with
                | Enemy (sprite, enemy) ->
                    match enemy with
                    | Zombie zombie ->
                        console.log zombie.health
                        let state = { zombie with health = zombie.health - 50.0 }
                        if state.health <= 0 then
                            EntityDestroyed id
                            // scene <- { scene with GameObjects = scene.GameObjects.Remove(id) }
                            // broadcastMsg None (ServerMessage.EntityRemoved id)
                        else
                            // let state = { entity with data = Enemy (sprite, Zombie state) }
                            // scene <- { scene with GameObjects = scene.GameObjects.Add (id, state) }
                            EnemyDamaged id
                | _else ->
                    console.log _else
            | None ->
                ()
        |]
    let sendMsg id (msg: ServerMessage) =
        try
            let c = clientConnection[id]
            let mutable i = 0
            let msg = Encode.Auto.toString msg
            while i < msg.Length do
                c.send !^ (msg.Substring(i, Math.Min(msg.Length - i, 1024)))
                i <- i + 1024
            c.send !^ "\r\n"
        with error ->
            ()
            // JS.debugger ()
            // console.log error
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
    // let entities = Map<int, Component list>
    member this.AddClient id client =
        clientConnection <- clientConnection.Add (id, client)
    /// Create answer to WebRTC request, initialize connection, and add player connection to list of lobby connections
    member this.ConnectClient request = promise {
        let! c, channel, response = RTC.JS.answerRequest request
        let id = nextClientId ()
        // clientConnection <- clientConnection.Add (id, channel)
        channel.onmessage <- fun ev ->
            try
                let events = onClientMessage id (Decode.Auto.unsafeFromString (string ev.data))
                for event in events do
                    broadcastMsg None (ServerMessage.GameEvent event)
                    game.ApplyEvent event
                    // match event with
                    // | EnemyDamaged id ->
                    //     // let ((Enemy (sprite, enemy)), pos) =
                    //     let entity = scene.GameObjects[id]
                    //     match scene.GameObjects.TryFind id |> Option.map (fun o -> o.data) with
                    //     | Some (Enemy (sprite, enemy)) ->
                    //         match enemy with
                    //         | Zombie zombie ->
                    //             let state = { zombie with health = zombie.health - 50.0 }
                    //             if state.health <= 0 then
                    //                 scene <- { scene with GameObjects = scene.GameObjects.Remove(id) }
                    //                 broadcastMsg None (ServerMessage.EntityRemoved id)
                    //             else
                    //                 let state = { entity with data = Enemy (sprite, Zombie state) }
                    //                 scene <- { scene with GameObjects = scene.GameObjects.Add (id, state) }
                    //     | _ -> ()
                    // | EntityDestroyed id ->
                        // console.log ("destroy entity id", id)
                        // scene <- { scene with GameObjects = scene.GameObjects.Remove(id) }
                        // broadcastMsg None (ServerMessage.EntityRemoved id)
                    // | _else -> console.log _else
                ()
            with error ->
                console.log error
        // c.ondatachannel <- fun ev ->
        // channel.onerror <- fun ev -> console.log ev
        c.ondatachannel <- fun ev -> console.log ev
        c.onconnectionstatechange <- fun ev ->
            if ev.target?connectionState = "disconnected" then
                clientConnection <- clientConnection.Remove id
                broadcastMsg (Some id) (ServerMessage.GameEvent (GameEvent.PlayerDisconnected id))
            console.log ev
        channel.onopen <- fun ev ->
            // JS.debugger ()
            let peers = clientConnection.Keys
            clientConnection <- clientConnection.Add (id, channel)
            game.ApplyEvent (PlayerJoined (id, ""))
            if ev.currentTarget?readyState = "open" then
                sendMsg id (ServerMessage.WorldState game.Scene)
                for peerId in peers do
                    sendMsg id (ServerMessage.GameEvent (PlayerJoined (peerId, "")))
                    sendMsg peerId (ServerMessage.GameEvent (GameEvent.PlayerJoined (id, "")))
        return response
    }
    member this.AddEnemy() =
        ()
    member this.AddPlayer() = ()
    member this.DamageEnemy() =
        ()
    member this.Simulate() = [|
        for kv in game.Scene.GameObjects do
            match kv.Value.data with
            | EntityType.Enemy _ ->
                let pos = kv.Value.position
                let r = JS.Math.random()
                if r > 0.1 then
                    // JS.console.log ("ope = " + (string r))
                    // JS.console.log (sprintf "%A" (fst kv.Value), snd kv.Value)
                    let p' = { pos with x = pos.x + 0.01 }
                    EntityMoved (kv.Key, p')
                    // scene <- { scene with GameObjects = scene.GameObjects.Add(kv.Key, { kv.Value with position = p' }) }
                    ()
            | _ ->
                ()
    |]
    member this.Step() =
        let events = this.Simulate()
        for event in events do
            game.ApplyEvent event
            broadcastMsg None (ServerMessage.GameEvent event)

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
    c.onopen <- fun _ev ->
        let server = GameServer(
            RAPIER.World.Create(RAPIER.Vector3.Create(0, -9.81, 0)),
            Scene.createScene ()
        )
        let gameLoop () =
            server.Step()
        let _gameLoopId = JS.setInterval gameLoop 20
        c.onmessage <- fun ev ->
            match Decode.Auto.fromString<Signaling.ServerMessage> (string ev.data) with
            | Ok message ->
                match message with
                | Signaling.ServerMessage.ConnectionRequest (clientId, webRtcRequest) ->
                    promise {
                        let! response = server.ConnectClient webRtcRequest
                        c.send (Encode.Auto.toString (Signaling.ClientMessage.ConnectionResponse (clientId, response)))
                    } |> ignore
                | _else ->
                    printfn "%A" _else
            | error -> console.log error
            
        c.send (Encode.Auto.toString (Signaling.ClientMessage.HostLobby "w00tcamp"))
}
