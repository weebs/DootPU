module Dootverse.Client.Game

open System
open System.Collections.Generic
open Browser.Types
open Doot.Maths.Voxel.Traversal
open Dootverse
open Dootverse.Models
open Fable.Core
open Browser
open Feliz
open Fable.Core.JsInterop
open Thoth.Json
open query_pipeline

type Component = struct end
// type Entity = struct end
// type GameWorld(world: world.World) =
//     let entities = Map<Entity, Component list>

type Scene(world: world.World, scene: threejs.__scenes_Scene.Scene) =
    // let world = GameWorld(world)
    member this.AddCube(staticPos, size: Network.float3, pos: Network.float3, ?meshProps: obj) =
        let desc = if staticPos then RAPIER.RigidBodyDesc.newStatic() else RAPIER.RigidBodyDesc.newDynamic()
        let rigidBody = world.createRigidBody(desc.setTranslation(pos.Tuple))
        let rapierSize = size / 2.0
        let geometry = three.BoxGeometry.Create(size.x, size.y, size.z)
        let material = three.MeshBasicMaterial.Create(box (if meshProps.IsSome then meshProps.Value else {| color = "blue" |}) :?> _)
        let collider = world.createCollider(RAPIER.ColliderDesc.cuboid(rapierSize.Tuple), rigidBody)
        let mesh = three.Mesh.Create(geometry, material)
        mesh.position.x <- pos.x
        mesh.position.y <- pos.y
        mesh.position.z <- pos.z
        scene.add mesh |> ignore
        rigidBody, collider, mesh
type Game(world: world.World, width, height) =
    let gfxScene =
        let scene = three.Scene.Create()
        scene.background <- box (three.Color.Create(1, 1, 1)) :?> _
        scene
    let scene = Scene(world, gfxScene)
    let fov, nearClip, farClip = 50, 0.1, 10000
    let camera = three.PerspectiveCamera.Create(fov, width / height, nearClip, farClip)
    let renderer = three.WebGLRenderer.Create()
    let geometry = three.BufferGeometry.Create()
    let lines =
        let material = three.LineBasicMaterial.Create(box {| color = 0x888888; vertexColors = true |} :?> _)
        let lines = three.Line.Create(geometry, material)
        gfxScene.add lines |> ignore
        lines.visible <- true
        lines
    member val DebugPhysics = false with get, set
    member this.Renderer = renderer
    member this.Camera = camera
    member this.ThreeJsScene = gfxScene
    member this.World = world
    member this.Scene = scene
    member this.LoadScene (scene: Network.Scene) =
        let treesTexture = three.TextureLoader.Create().load "textures/ForestTrees.png"
        let rsTreeTexture = three.TextureLoader.Create().load "textures/rs/yewtree.png"
        for wall in scene.Walls do
            let x, y = wall.Key
            this.Scene.AddCube(
                true,
                { x = 1
                  y = if Math.Abs x = 100 || Math.Abs y = 100 then 40 else 1
                  z = 1 },
                { x = float x - 0.5
                  y = if Math.Abs x = 100 || Math.Abs y = 100 then 20 else 0.5
                  z = float y + 0.5 },
                {| map = treesTexture |}
            ) |> ignore
        for kv in scene.Entities do
            let m = three.SpriteMaterial.Create(box {| map = rsTreeTexture |} :?> _)
            let pos = snd kv.Value
            let sprite = three.Sprite.Create m
            let collider =
                this.World.createCollider(
                    RAPIER.ColliderDesc.cuboid(0.1, 1, 0.1),
                    this.World.createRigidBody(
                        RAPIER.RigidBodyDesc.newStatic()
                            .setTranslation(pos.x, pos.y, pos.z)))
            sprite.position.x <- pos.x
            sprite.position.y <- pos.y
            sprite.position.z <- pos.z
            let instance = this.ThreeJsScene.add sprite
            // todo
            ()
            // spriteMap.current.Add(kv.Key, sprite)
            // colliderMap.current.Add(kv.Key, collider)
            // idMap.current.Add(collider, kv.Key)
    
    
    member this.RenderPhysics () =
        let debugInfo = world.debugRender()
        geometry.setAttribute(!^ "position", U2.Case1 (three.BufferAttribute.Create(box debugInfo.vertices :?> _, 3))) |> ignore
        // geometry.setAttribute(!^ "position", U2.Case1 (three.BufferAttribute.Create(box [| 0f; 0f; 0f; 10f; 10f; 10f |] :?> _, 3))) |> ignore
        geometry.setAttribute(!^ "color", !^ three.BufferAttribute.Create(box (debugInfo.colors.map(fun c -> c)) :?> _, 4)) |> ignore
    member this.Step (time, physicsUpdate, gfxUpdate) =
        world.step()
        physicsUpdate time
        gfxUpdate time
        if this.DebugPhysics then this.RenderPhysics()
        renderer.render(gfxScene, camera)
        
        Engine.endInputFrame ()

let inputVector () =
    let mutable velocityDirection = Vector2(0, 0)
    if Keys.isPressed "d" || Keys.isPressed "ArrowRight" then
        velocityDirection <- velocityDirection + Vector2(1, 0)
    if Keys.isPressed "w" || Keys.isPressed "ArrowUp" then
        velocityDirection <- velocityDirection + Vector2(0, -1)
    if Keys.isPressed "a" || Keys.isPressed "ArrowLeft" then
        velocityDirection <- velocityDirection + Vector2(-1, 0)
    if Keys.isPressed "s" || Keys.isPressed "ArrowDown" then
        velocityDirection <- velocityDirection + Vector2(0, 1)
    velocityDirection.Normalized
[<ReactComponent>]
let GameView (game: Game) =
    let audio: HTMLAudioElement = emitJsExpr null "new Audio()"
    audio.src <- "shotgun.wav"
    audio.volume <- 0.2
    let divRef = React.useRef<Types.HTMLDivElement option> None
    let timeRef = React.useRef 0.0
    let connectionRef = React.useRef<Types.RTCDataChannel option> None
    let peers = React.useRef Map.empty
    let spriteMap = React.useRef (Dictionary<int, threejs.__objects_Sprite.Sprite>())
    let colliderMap = React.useRef (Dictionary<int, Rapier.collider.Collider>())
    let idMap = React.useRef (Dictionary<Rapier.collider.Collider, int>())
    
    let ccRef = React.useRef<character_controller.KinematicCharacterController> null
    let ccCollider = React.useRef<Rapier.collider.Collider> null
    // let ccRigidbody = React.useRef null
    let camera = game.Camera
    let scene = game.ThreeJsScene
    let r = game.Renderer
    // game.DebugPhysics <- false
    let rec update (dt: float) =
        // for sprite in sprites do
        //     let height = sprite.material.map.Value.image?height
        
        // Mouse capture
        if Keys.isJustPressed "tab" then
            if document.pointerLockElement = null then
                document.body.requestPointerLock ()
            else
                document.exitPointerLock ()
                
        if Engine.mouse1 && document.pointerLockElement <> null then
            let x = -1.0 * Math.Sin camera.rotation.y
            let z = -1.0 * Math.Cos camera.rotation.y
            let dir = RAPIER.Vector3.Create(x, 0, z)
            let ray = RAPIER.Ray.Create(RAPIER.Vector3.Create(camera.position.x, camera.position.y, camera.position.z), dir)
            let raycast = game.World.castRay(ray, 10000, true, QueryFilterFlags.ALL_SHAPES)
            match raycast with
            | Some raycast ->
                let hitPoint = ray.pointAt(raycast.toi)
                game.Scene.AddCube(true, { x = 0.02; y = 0.02; z = 0.02 }, { x = hitPoint.x; y = hitPoint.y; z = hitPoint.z }, {| color = "blue" |})
                |> ignore
                console.log ("camera position =", camera.position.x, camera.position.y, camera.position.z)
                console.log ("hit point =", hitPoint)
                console.log ("dir = ", x, z)
                console.log raycast
                if idMap.current.ContainsKey raycast.collider then
                    let id = idMap.current[raycast.collider]
                    connectionRef.current |> Option.iter (fun c ->
                        let msg = Encode.Auto.toString (Network.DestroyedEntity id)
                        console.log msg
                        console.log c
                        c.send !^ msg)
                    scene.remove spriteMap.current[id]
                    |> ignore
                    // spriteMap.current.Remove id
                    // |> ignore
            | None -> ()
            
            audio.play()
                
        if Keys.isPressed "q" then
            camera.rotation.y <- camera.rotation.y + (2.0 * dt)
        if Keys.isPressed "e" then
            camera.rotation.y <- camera.rotation.y - (2.0 * dt)
            
        let mouseSensitivity = 0.5
        camera.rotation.y <- camera.rotation.y - (dt * mouseSensitivity * float Engine.mouseX)
        
        // Apply input to position
        let speed = if Keys.isPressed "shift" then 15.0 else 2.5
        let input = inputVector ()
        let playerRotation = -camera.rotation.y
        let input = (Render.rotateVector playerRotation input).Normalized * dt * speed
        // Character controller update
        let initialPos = ccCollider.current.translation()
        let destination = RAPIER.Vector3.Create(initialPos.x + input.X, 0, initialPos.z + input.Y)
        let desiredInput = RAPIER.Vector3.Create(input.X, 0, input.Y)
        ccRef.current.computeColliderMovement(ccCollider.current, desiredInput)
        let correctedMovement = ccRef.current.computedMovement()
        // let correctedDestination = RAPIER.Vector3.Create(initialPos.x + (correctedMovement.x * dt), 0, initialPos.z + (correctedMovement.z * dt))
        let correctedDestination = RAPIER.Vector3.Create(initialPos.x + correctedMovement.x, 1, initialPos.z + correctedMovement.z)
        ccCollider.current.setTranslation(correctedDestination)
        for i in 0..Math.Min(1, int (ccRef.current.numComputedCollisions())) - 1 do
            let collision = ccRef.current.computedCollision(i)
            let normal = collision.Value.normal1
            let v =
                RAPIER.Vector3.Create(input.X - collision.Value.normal1.x, 0.0 - collision.Value.normal1.y, input.Y - collision.Value.normal1.z)
            let dot = input.X * collision.Value.normal1.x + input.Y * collision.Value.normal1.z
            let updatedInput = RAPIER.Vector3.Create(v.x * dot, v.y * dot, v.z * dot)
            console.log dot
            
            // let dotCorrection = input.X * updatedInput.z + input.Y * updatedInput.z
            // console.log ("dot = ", dotCorrection)
            // From stackoverflow
            // https://gamedev.stackexchange.com/questions/4059/how-to-make-the-player-slide-smoothly-against-terrain
            let undesiredMotion = RAPIER.Vector3.Create(normal.x * dot, normal.y * dot, normal.z * dot)
            let updatedInput = RAPIER.Vector3.Create(input.X - undesiredMotion.x, 0.0 - undesiredMotion.y, input.Y - undesiredMotion.z)
            // if dotCorrection > 0 then
            ccCollider.current.setTranslation(RAPIER.Vector3.Create(
                initialPos.x + collision.Value.translationApplied.x + updatedInput.x,
                1,
                initialPos.z + collision.Value.translationApplied.z + updatedInput.z))
            console.log (ccRef.current.computedCollision i)
            console.log updatedInput
        let pos = ccCollider.current.translation()
        camera.position.x <- pos.x
        // camera.position.y <- pos.y
        camera.position.z <- pos.z
        
        try
            //if connectionRef.current.IsSome && connectionRef.current.Value.readyState = RTCDataChannelState.Open then
            if connectionRef.current.IsSome then
                connectionRef.current.Value.send !^ (
                    Encode.Auto.toString<Network.ClientMessage> (
                         Network.Update {
                              Position = { x = camera.position.x; y = camera.position.y; z = camera.position.z; }
                              Rotation = camera.rotation.y
                        }
                    )
                )
        with error -> console.log error
    let rec loop time =
        try
            let dt = (time - timeRef.current) / 1000.0
            timeRef.current <- time
            game.Step(dt, ignore, update)
            // let context = game.Renderer.domElement.getContext_experimental_webgl()
            window.requestAnimationFrame loop |> ignore
        with error -> console.log error
    React.useEffect <| fun () ->
        // let floorBody, floorCollider, floorMesh =
            // game.Scene.AddCube(true, { x = 200.0; y = 1.0; z = 200.0 }, { x = 0; y = -0.5; z = 0 }, {| color = "blue" |})
        // Setup player controller
        ccRef.current <- game.World.createCharacterController(0.01)
        // ccRigidbody.current <- game.World.createRigidBody(RAPIER.RigidBodyDesc.dynamic())
        ccCollider.current <-
            game.World.createCollider(
                RAPIER.ColliderDesc.capsule(0.3, 0.22)) //,
                // ccRigidbody.current)
        let mutable msgQueue = []
        
        // Setup connection to server and respond to messages
        promise {
            let! clientConnection, clientDataChannel, request = RTC.JS.createConnection ()
            let endpoint =
                document.baseURI
                    .Replace("http:", "ws:")
                    .Replace("https:", "wss:")
                    .Replace("game.html", "ws")
                    .Replace("5173", "8000")
            //let websocket = WebSocket.Create("ws://127.0.0.1:8000/ws")
            let websocket = WebSocket.Create endpoint
            websocket.onmessage <- fun ev ->
                match Decode.Auto.fromString<Network.LobbyConnection.ServerMessage> (string ev.data) with
                | Ok message ->
                    match message with
                    | Network.LobbyConnection.ServerMessage.ConnectionResponse webRtcResponse ->
                        console.log "got response"
                        console.log webRtcResponse
                        promise {
                            do! clientConnection.setRemoteDescription (toPlainJsObj {| ``type`` = "answer"; sdp = webRtcResponse.Answer |} :?> _)
                            for (candidate, sdpMid) in webRtcResponse.Candidates do
                                do! clientConnection.addIceCandidate (toPlainJsObj {| candidate = candidate; sdpMid = sdpMid |} :?> _)
                            clientConnection.ondatachannel <- fun ev -> console.log ev
                            clientDataChannel.onopen <- fun ev ->
                                console.log "client data channel open"
                                console.log ev
                                connectionRef.current <- Some clientDataChannel
                        } |> ignore
                    | Network.LobbyConnection.ConnectionRequest _ -> failwith "todo"
                    | Network.LobbyConnection.Lobbies foo ->
                        if foo.Length > 0 then
                            websocket.send (Encode.Auto.toString (Network.LobbyConnection.Connect (foo[0].id, request)))
                        else
                            promise {
                                do! Promise.sleep 500
                                console.log "No lobbies found, refreshing"
                                websocket.send (Encode.Auto.toString Network.LobbyConnection.RefreshLobbies)
                            } |> ignore
                | error ->
                    JS.debugger ()
                    console.log error
                // todo: sometimes the messages get split
            let onMsg msg =
                match Decode.Auto.fromString<Network.ServerMessage> msg with
                | Ok message ->
                    match message with
                    | Network.UpdatePlayer (id, state) ->
                        let sprite =
                            if not (peers.current.ContainsKey id) then
                                let loader = three.TextureLoader.Create()
                                let texture = loader.load "textures/doom/guy.png"
                                let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
                                let sprite = three.Sprite.Create m
                                sprite.scale.set(0.5, 0.5, 0.5) |> ignore
                                scene.add sprite |> ignore
                                peers.current <- peers.current.Add (id, sprite)
                                sprite
                            else
                                peers.current[id]
                        sprite.position.x <- state.Position.x
                        sprite.position.y <- state.Position.y
                        sprite.position.z <- state.Position.z
                        sprite.rotation.y <- state.Rotation
                    | Network.PlayerDisconnected id ->
                        if peers.current.ContainsKey id then
                            scene.remove peers.current[id]
                            |> ignore
                            peers.current <- peers.current.Remove id
                    | Network.WorldState world ->
                        let loader = three.TextureLoader.Create()
                        let treesTexture = loader.load "textures/ForestTrees.png"
                        // let rsTreeTexture = loader.load "textures/rs/yewtree.png"
                        let zombieStanding = loader.load "textures/rs/zombie_standing.png"
                        for wall in world.Walls do
                            let x, y = wall.Key
                            game.Scene.AddCube(
                                true,
                                { x = 1
                                  y = if Math.Abs x = 100 || Math.Abs y = 100 then 40 else 1
                                  z = 1 },
                                { x = float x - 0.5
                                  y = if Math.Abs x = 100 || Math.Abs y = 100 then 20 else 0.5
                                  z = float y + 0.5 },
                                {| map = treesTexture |}
                            ) |> ignore
                        for (asset, entities) in world.Entities |> Map.toArray |> Array.groupBy (snd >> fst) do
                            let texture = loader.load asset
                            for id, (_, position) in entities do
                                let kv = {| Key = id, asset; Value = position |}
                                let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
                                let sprite = three.Sprite.Create m
                                let collider =
                                    game.World.createCollider(
                                        RAPIER.ColliderDesc.cuboid(0.1, 1, 0.1),
                                        game.World.createRigidBody(RAPIER.RigidBodyDesc.newStatic().setTranslation(
                                            kv.Value.x, kv.Value.y, kv.Value.z)))
                                sprite.position.x <- kv.Value.x
                                sprite.position.y <- kv.Value.y
                                sprite.position.z <- kv.Value.z
                                let instance = scene.add sprite
                                spriteMap.current.Add(fst kv.Key, sprite)
                                colliderMap.current.Add(fst kv.Key, collider)
                                idMap.current.Add(collider, fst kv.Key)
                    | Network.EntityRemoved id ->
                        let collider = colliderMap.current[id]
                        let sprite = spriteMap.current[id]
                        colliderMap.current.Remove id |> ignore
                        spriteMap.current.Remove id |> ignore
                        scene.remove sprite |> ignore
                        game.World.removeCollider (collider, false)
                    | _else ->
                        console.log _else
                | Error err ->
                    console.log msg
                    console.log err
                // console.log ev
            clientDataChannel.onmessage <- fun ev ->
                let msg = string ev.data
                if msg.EndsWith "\r\n" then
                    let fullMessage = (msgQueue |> String.concat "") + msg
                    msgQueue <- []
                    onMsg fullMessage
                else
                    msgQueue <- msgQueue @ [ msg ]
            clientDataChannel.onopen <- fun ev ->
                connectionRef.current <- Some clientDataChannel
            clientDataChannel.onclose <- fun ev ->
                connectionRef.current <- Unchecked.defaultof<_>
            divRef.current.Value.appendChild r.domElement
            |> ignore
            window.requestAnimationFrame loop
            |> ignore
        } |> ignore
    Html.div [
        prop.ref divRef
        prop.style [
            style.width (length.vw 100)
            style.height (length.vh 100)
            style.overflow.hidden
        ]
        prop.children [
            Html.div [
                prop.style [
                    style.position.absolute
                    style.width (length.vw 100)
                    style.height (length.vh 100)
                ]
                prop.children [
                    Svg.svg [
                        svg.width r.domElement.width
                        svg.height r.domElement.height
                        svg.children [
                            Svg.rect [
                                svg.x (r.domElement.width / 2.0 - 25.0)
                                svg.y (r.domElement.height / 2.0 - 1.0)
                                svg.width 50
                                svg.height 2
                                svg.stroke "blue"
                            ]
                            Svg.rect [
                                svg.x (r.domElement.width / 2.0 - 1.0)
                                svg.y (r.domElement.height / 2.0 - 25.0)
                                svg.width 2
                                svg.height 50
                                svg.stroke "blue"
                            ]
                        ]
                    ]
                    Html.img [
                        prop.src "textures/double_barrel_shotgun.png"
                        prop.width (length.percent 20)
                        prop.height length.auto
                        prop.style [
                            style.position.absolute
                            style.right (length.percent 25)
                            style.bottom (length.percent -10)
                        ]
                    ]
                ]
            ]
        ]
    ]
let start () = promise {
    do! RAPIER.init ()
    // let! response = server.AnswerRequest request
    // console.log response
    let spriteName = AssetId "rsc_sprite.png"
    let treeSprite = AssetId "textures/rs/yewtree.png"
    let heartSprite = AssetId "heart.png"
    let screenWidth = window.innerWidth
    let screenHeight = window.innerHeight
    // let scene = three.Scene.Create()
    // scene.up.set (0, -1, 0)
    // let r = three.WebGLRenderer.Create(box {| antialias = false |} :?> _)
    let cameraPlaneHeight = 2.0
    // let observerHeight = cameraPlaneHeight / 2.0
    let observerHeight = 0.25
    let focalLength = 1.0
    let fov = Math.Atan((cameraPlaneHeight / 2.0) / focalLength) * (360.0 / Math.Tau)
    let game = Game(RAPIER.World.Create(RAPIER.Vector3.Create(0, -9.81, 0)), screenWidth, screenHeight)
    let camera = game.Camera
    let r = game.Renderer
    let scene = game.ThreeJsScene
    
    // console.log ("fov = ", fov)
    // let camera = three.PerspectiveCamera.Create(fov, screenWidth / screenHeight, 0.1, 10000)
    camera.position.y <- observerHeight
    camera.position.z <- 4
    
    // let treesTexture = three.TextureLoader.Create().load "textures/ForestTrees.png"
    // let rscStoneWall = three.TextureLoader.Create().load "textures/rs/wall.png"
    // rscStoneWall.center <- three.Vector2.Create(0.5, 0.5)
    // rscStoneWall.rotation <- Math.Tau / 4.0
    r.setSize (screenWidth, screenHeight + 1.0)
    window.onresize <- fun _ ->
        console.log "resize"
        r.setSize(window.innerWidth, window.innerHeight)
        camera.aspect <- window.innerWidth / window.innerHeight
        camera.updateProjectionMatrix()
    console.log three
    document.getElementById "root" |> ReactDOM.createRoot |> fun root -> root.render (GameView game)
}