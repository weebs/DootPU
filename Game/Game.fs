module Dootverse.Client.Game

open System
open Doot.Maths.Voxel.Traversal
open Dootverse
open Dootverse.Models
open Fable.Core
open Browser
open Feliz
open Fable.Core.JsInterop
open Thoth.Json

type Scene(world: world.World, scene: threejs.__scenes_Scene.Scene) =
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
    member val DebugPhysics = true with get, set
    member this.Renderer = renderer
    member this.Camera = camera
    member this.ThreeJsScene = gfxScene
    member this.Scene = scene
    member this.LoadScene (scene: Network.Scene) =
        for kv in scene.Walls do
            let x, z = kv.Key
            this.Scene.AddCube(true, { x = 1; y = 1; z = 1; }, { x = float x - 0.5; y = 0.5; z = float z - 0.5 })
            |> ignore
        for kv in scene.Entities do
            ()
    
    
    member this.RenderPhysics () =
        let debugInfo = world.debugRender()
        console.log debugInfo
        geometry.setAttribute(!^ "position", U2.Case1 (three.BufferAttribute.Create(box debugInfo.vertices :?> _, 3))) |> ignore
        // geometry.setAttribute(!^ "position", U2.Case1 (three.BufferAttribute.Create(box [| 0f; 0f; 0f; 10f; 10f; 10f |] :?> _, 3))) |> ignore
        geometry.setAttribute(!^ "color", !^ three.BufferAttribute.Create(box (debugInfo.colors.map(fun c -> c)) :?> _, 4)) |> ignore
    member this.Step (time, physicsUpdate, gfxUpdate) =
        world.step()
        physicsUpdate time
        gfxUpdate time
        if this.DebugPhysics then this.RenderPhysics()
        renderer.render(gfxScene, camera)
        // todo: Reset this automatically? Look at Raycast.Game loop to see everything
        Engine.mouseX <- 0
        Engine.mouseY <- 0
        for kv in Keys.justPressed do
            Keys.justPressed[kv.Key] <- false
        

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
    let divRef = React.useRef<Types.HTMLDivElement option> None
    let timeRef = React.useRef 0.0
    let connectionRef = React.useRef<Types.WebSocket> null
    let peers = React.useRef Map.empty
    let camera = game.Camera
    let scene = game.ThreeJsScene
    let r = game.Renderer
    game.DebugPhysics <- false
    let rec update (dt: float) =
        // for sprite in sprites do
        //     let height = sprite.material.map.Value.image?height
        
        // Mouse capture
        if Keys.isJustPressed "tab" then
            if document.pointerLockElement = null then
                document.body.requestPointerLock ()
            else
                document.exitPointerLock ()
                
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
        camera.position.x <- camera.position.x + input.X
        camera.position.z <- camera.position.z + input.Y
        
        if connectionRef.current <> null then
            connectionRef.current.send (
                Encode.Auto.toString<Network.ClientMessage> (
                     Network.Update {
                          Position = { x = camera.position.x; y = camera.position.y; z = camera.position.z; }
                          Rotation = camera.rotation.y
                    }
                )
            )
    let rec loop time =
        let dt = (time - timeRef.current) / 1000.0
        timeRef.current <- time
        game.Step(dt, ignore, update)
        let context = game.Renderer.domElement.getContext_experimental_webgl()
        window.requestAnimationFrame loop |> ignore
    React.useEffect <| fun () ->
        let c = WebSocket.Create("ws://127.0.0.1:8000/ws")
        c.onmessage <- fun ev ->
            match Decode.Auto.fromString<Network.ServerMessage> (string ev.data) with
            | Ok message ->
                match message with
                | Network.UpdatePlayer (id, state) ->
                    let sprite =
                        if not (peers.current.ContainsKey id) then
                            let loader = three.TextureLoader.Create()
                            let texture = loader.load "textures/doom/guy.png"
                            let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
                            let sprite = three.Sprite.Create m
                            sprite.scale.set(0.5, 0.5, 0.5)
                            scene.add sprite
                            |> ignore
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
                    let treesTexture = three.TextureLoader.Create().load "textures/ForestTrees.png"
                    for wall in world.Walls do
                        let x, y = wall.Key
                        game.Scene.AddCube(true, { x = 1; y = 1; z = 1 }, { x = float x - 0.5; y = 0.5; z = float y + 0.5 }, {| map = treesTexture |})
                        |> ignore
                | _else ->
                    console.log _else
            | Error err -> console.log err
            // console.log ev
        c.onopen <- fun ev ->
            connectionRef.current <- c
        c.onclose <- fun ev ->
            connectionRef.current <- null
        divRef.current.Value.appendChild r.domElement
        |> ignore
        window.requestAnimationFrame loop
        |> ignore
    Html.div [
        prop.ref divRef
    ]
let start () = promise {
    do! RAPIER.init ()
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
    
    // let world = Raycast.Game.createScene treeSprite treeSprite
    // let sprites = [|
    //     let loader = three.TextureLoader.Create()
    //     for (pos, AssetId url) in world.entities do
    //         let texture = loader.load url
    //         let sprite =
    //             let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
    //             three.Sprite.Create m
    //         scene.add sprite
    //         sprite.position.x <- pos.X
    //         sprite.position.z <- pos.Y
    //         sprite.position.y <- 0.5
    //         // let spriteQuadish =
    //             // let g = three.BoxGeometry.Create(1, 1, 0.01)
    //             // let m = three.MeshBasicMaterial.Create(box {| map = texture |} :?> _)
    //             // three.Mesh.Create(g, m)
    //         // scene.add spriteQuadish
    //         // spriteQuadish.position.x <- pos.X
    //         // spriteQuadish.position.z <- pos.Y
    //         // spriteQuadish.position.y <- 0.2
    //         yield sprite
    //         // yield spriteQuadish
    // |]
    // let treesTexture = three.TextureLoader.Create().load "textures/ForestTrees.png"
    // let rscStoneWall = three.TextureLoader.Create().load "textures/rs/wall.png"
    // rscStoneWall.center <- three.Vector2.Create(0.5, 0.5)
    // rscStoneWall.rotation <- Math.Tau / 4.0
    // for wall in world.Walls do
    //     let (x, y) = wall.Key
    //     let (r, g, b) = wall.Value
    //     let color = $"rgb({r}, {g}, {b})"
    //     game.Scene.AddCube(true, { x = 1; y = 1; z = 1 }, { x = float x - 0.5; y = 0.5; z = float y + 0.5 }, {| map = treesTexture |})
    //     |> ignore
        
        // let cube =
        //     let color = $"rgb({r}, {g}, {b})"
        //     let g = three.BoxGeometry.Create(1, 1, 1)
        //     let m = three.MeshBasicMaterial.Create(box {| map = treesTexture; |} :?> _)
        //     three.Mesh.Create(g, m)
        // // threejs draws cubes with their center point at the position
        // cube.position.x <- float x - 0.5
        // cube.position.y <- 0.5
        // cube.position.z <- float y + 0.5
        // scene.add(cube)
        // |> ignore
    // Floor
    // let floor =
        // let g = three.BoxGeometry.Create(200, 1, 200)
        // let m = three.MeshBasicMaterial.Create(box {| color = "grey" |} :?> _)
        // three.Mesh.Create(g, m)
    // floor.position.y <- -0.5
    // camera.up.y <- 1
    // camera.up.set (1, 1, -1)
    // scene.add floor
    let floorBody, floorCollider, floorMesh =
        game.Scene.AddCube(true, { x = 200.0; y = 1.0; z = 200.0 }, { x = 0; y = -0.5; z = 0 }, {| color = "blue" |})
    // let cube =
    //     let g = three.BoxGeometry.Create(1, 1, 1)
    //     let m = three.MeshBasicMaterial.Create(box {| color = "blue" |} :?> _)
    //     three.Mesh.Create(g, m)
    // scene.add(cube)
    // cube.position.x <- 0.5
    // cube.position.y <- 0.5
    // cube.position.z <- 0
    // let cubeObj = scene.add(cube)
    // cubeObj.rot
    // cube.rotation.z <- 0.2
    r.setSize (screenWidth, screenHeight)
    window.onresize <- fun _ ->
        console.log "resize"
        r.setSize(window.innerWidth, window.innerHeight)
        camera.aspect <- window.innerWidth / window.innerHeight
        camera.updateProjectionMatrix()
    // document.body.appendChild r.domElement
    // |> ignore
    // r.render (scene, camera)
    // console.log scene
    console.log three
    document.getElementById "root" |> ReactDOM.createRoot |> fun root -> root.render (GameView game)
}