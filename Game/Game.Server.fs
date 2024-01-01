module Dootverse.Game.Server
open System
open System.Collections.Generic
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
    let mutable frame = 0
    // let nextClientId =
    //     let mutable id = 0
    //     fun () ->
    //         id <- id + 1
    //         id - 1
    //         game.Scene.GameObjects.Keys |> Seq.max |> (+) 1
    let mutable clientConnection: Map<int, Browser.Types.RTCDataChannel> = Map.empty
    /// Return an array of events that occur due to the message
    let onClientMessage (clientGuid: int) (message: ClientMessage) = [|
        match message with
        | ClientMessage.PlayerMoved (pos, rotation) ->
            PlayerMoved (clientGuid, pos, rotation) 
        | ClientMessage.DestroyedEntity id ->
            EntityDestroyed id
        | ClientMessage.ShotEntity id ->
            console.log ("shot entity", id)
            match game.Entities.TryGetValue id with
            | true, entity ->
                match entity.entity.data with
                | Enemy enemy ->
                    let state = { enemy with health = enemy.health - 50.0 }
                    EnemyDamaged id
                    if state.health <= 0 then
                        EntityDestroyed id
                | _else ->
                    console.log _else
            | _ ->
                ()
        | ClientMessage.Update playerState ->
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
        // clientConnection <- clientConnection.Add (id, channel)
        channel.onopen <- fun ev ->
            // JS.debugger ()
            let scene = game.Scene ()
            let state = { Health = 100; Name = ""; Status = true; Rotation = 0 }
            let id = game.Create { position = { x = 0; y = 0; z = 0; }; data = Player state; sprite = Some "textures/doom/guy.png"; }
            let peers = clientConnection.Keys
            clientConnection <- clientConnection.Add (id, channel)
            if ev.currentTarget?readyState = "open" then
                sendMsg id (ServerMessage.WorldState scene)
                for peerId in peers do
                //     sendMsg id (ServerMessage.GameEvent (PlayerJoined (peerId, "")))
                    sendMsg peerId (ServerMessage.GameEvent (GameEvent.PlayerJoined (id, "")))
                game.ApplyEvent (PlayerJoined (id, ""))
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
        return response
    }
    member this.AddEnemy() =
        ()
    member this.AddPlayer() = ()
    member this.DamageEnemy() =
        ()
    member this.Simulate n = [|
        // let actions = List()
        for kv in game.Entities do
        // for kv in game.Scene.GameObjects do
            match kv.Value.entity.data with
            | EntityType.Enemy enemy ->
                match enemy.typ with
                | Zombie zombie ->
                    match zombie.state with
                    | Idle ->
                        let pos = kv.Value.entity.position
                        let r = JS.Math.random()
                        if r > 0.5 then
                            // JS.console.log ("ope = " + (string r))
                            // JS.console.log (sprintf "%A" (fst kv.Value), snd kv.Value)
                            // todo: check if can see player
                            let playerToChase = game.Entities |> Seq.tryFind (fun o -> match o.Value.entity.data with Player _ -> true | _ -> false )
                            match playerToChase with
                            | Some player ->
                                // let state = { enemy with typ = Zombie { state = ChasingPlayer player.Key } }
                                // EntityUpdated (kv.Key, { kv.Value.entity with data = Enemy state } )
                                UpdateZombie (kv.Key, ChasingPlayer player.Key)
                            | None -> ()
                            // let p' = { pos with x = pos.x + 0.01 }
                            // EntityMoved (kv.Key, p')
                            // scene <- { scene with GameObjects = scene.GameObjects.Add(kv.Key, { kv.Value with position = p' }) }
                            ()
                    | ChasingPlayer id ->
                        // todo: check if can see player
                        if (n + id) % 5 = 0 then
                            match game.Entities.TryGetValue id with
                            | true, player ->
                                let p = player.entity.position
                                let dir = p - kv.Value.entity.position
                                if dir.Length < 0.5 then
                                    console.log "ope"
                                    // EntityUpdated (kv.Key, { kv.Value.entity with data = Enemy { enemy with typ = Zombie { state = AttackingPlayer (id, 20) } } })
                                    UpdateZombie (kv.Key, AttackingPlayer (id, 20))
                                else
                                    EntityMoved (kv.Key, kv.Value.entity.position + (dir.Normal * 0.1))
                                // EntityMoved (kv.Key, kv.Value.position + { x = 1.0; y = 0.0; z = 0.0 })
                                // todo: move towards player
                                // EntityMoved kv.Key
                            | _ -> ()
                    | AttackingPlayer(id, framesToAttack) ->
                        if framesToAttack = 1 then
                            match game.Entities.TryGetValue id with
                            | true, player ->
                                match player.entity.data with
                                | Player state ->
                                    let p = player.entity.position
                                    let dir = p - kv.Value.entity.position
                                    if dir.Length < 0.7 then
                                        PlayerDamaged (id, 10)
                                | _ -> ()
                            | _ -> ()
                            UpdateZombie (kv.Key, Idle)
                            // EntityUpdated (kv.Key, { kv.Value.entity with data = Enemy { enemy with typ = Zombie { state = Idle } } })
                        else
                            UpdateZombie (kv.Key, AttackingPlayer (id, framesToAttack - 1))
                            // EntityUpdated (kv.Key, { kv.Value.entity with data = Enemy { enemy with typ = Zombie { state = AttackingPlayer (id, framesToAttack - 1) } } })
                    
                    // let player =
            | Player state ->
                if state.Health <= 0 && state.Status then
                    console.log "player down!"
                    PlayerDown kv.Key
                elif state.Status = false then
                    PlayerSpawned (kv.Key, { x = 0; y = 0; z = 0 })
            | _ ->
                ()
        // actions
    |]
    member this.Step() =
        let events = this.Simulate frame
        for event in events do
            game.ApplyEvent event
            broadcastMsg None (ServerMessage.GameEvent event)
        frame <- frame + 1

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
