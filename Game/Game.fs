module Dootverse.Client.Game
open System
open Doot.Maths.Voxel.Traversal
open Dootverse.Engine
open Dootverse.Models
open Fable.Core
open Dootverse
open Browser
open Feliz
open Fable.Core.JsInterop
let three: threejs.IExports = JsInterop.importAll "three"


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
let Game (r: threejs.__renderers_WebGLRenderer.WebGLRenderer, scene, camera: threejs.__renderers_WebGLRenderer.Camera, sprites: threejs.__objects_Sprite.Sprite[]) =
    let divRef = React.useRef<Types.HTMLDivElement option> None
    let timeRef = React.useRef 0.0
    let connectionRef = React.useRef<Types.WebSocket> null
    let peers = React.useRef Map.empty
    let rec render time =
        let dt = (time - timeRef.current) / 1000.0
        timeRef.current <- time
        r.render (scene, camera)
        
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
        camera.rotation.y <- camera.rotation.y - (dt * mouseSensitivity * float mouseX)
        
        // Apply input to position
        let speed = if Keys.isPressed "shift" then 25.0 else 5.0
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
            
        
        // todo: Reset this automatically? Look at Raycast.Game loop to see everything
        Engine.mouseX <- 0
        Engine.mouseY <- 0
        for kv in Keys.justPressed do
            Keys.justPressed[kv.Key] <- false
        
        // camera.rotation.y <- camera.rotation.y + 0.005
        // camera.position.z <- camera.position.z - (0.01 * dt)
        // ctx.font <- "12px serif"
        // ctx.fillStyle <- U3.Case1 "blue"
        window.requestAnimationFrame render |> ignore
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
                            let texture = loader.load "heart.png"
                            let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
                            let sprite = three.Sprite.Create m
                            scene.add sprite
                            |> ignore
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
                | _else ->
                    console.log _else
            | Error err -> console.log err
            console.log ev
        c.onopen <- fun ev ->
            connectionRef.current <- c
        c.onclose <- fun ev ->
            connectionRef.current <- null
        divRef.current.Value.appendChild r.domElement
        |> ignore
        window.requestAnimationFrame render
        |> ignore
    Html.div [
        prop.ref divRef
    ]

let start () =
    let spriteName = "rsc_sprite.png"
    let world = Raycast.Game.createWorld (AssetId "heart.png") (AssetId "textures/rs/yewtree.png")
    let screenWidth = window.innerWidth
    let screenHeight = window.innerHeight
    let scene = three.Scene.Create()
    // scene.up.set (0, -1, 0)
    let r = three.WebGLRenderer.Create(box {| antialias = false |} :?> _)
    let cameraPlaneHeight = 2.0
    // let observerHeight = cameraPlaneHeight / 2.0
    let observerHeight = 0.5
    let focalLength = 1.0
    let fov = Math.Atan((cameraPlaneHeight / 2.0) / focalLength) * (360.0 / Math.Tau)
    
    console.log ("fov = ", fov)
    let camera = three.PerspectiveCamera.Create(fov, screenWidth / screenHeight, 0.1, 10000)
    camera.position.y <- observerHeight
    camera.position.z <- 4
    
    let sprites = [|
        let loader = three.TextureLoader.Create()
        for (pos, AssetId url) in world.entities do
            let texture = loader.load url
            let sprite =
                let m = three.SpriteMaterial.Create(box {| map = texture |} :?> _)
                three.Sprite.Create m
            scene.add sprite
            sprite.position.x <- pos.X
            sprite.position.z <- pos.Y
            sprite.position.y <- 0.5
            // let spriteQuadish =
                // let g = three.BoxGeometry.Create(1, 1, 0.01)
                // let m = three.MeshBasicMaterial.Create(box {| map = texture |} :?> _)
                // three.Mesh.Create(g, m)
            // scene.add spriteQuadish
            // spriteQuadish.position.x <- pos.X
            // spriteQuadish.position.z <- pos.Y
            // spriteQuadish.position.y <- 0.2
            yield sprite
            // yield spriteQuadish
    |]
    let treesTexture = three.TextureLoader.Create().load "textures/ForestTrees.png"
    let rscStoneWall = three.TextureLoader.Create().load "textures/rs/wall.png"
    rscStoneWall.center <- three.Vector2.Create(0.5, 0.5)
    rscStoneWall.rotation <- Math.Tau / 4.0
    for wall in world.Walls do
        let (x, y) = wall.Key
        let (r, g, b) = wall.Value
        let cube =
            let color = $"rgb({r}, {g}, {b})"
            let g = three.BoxGeometry.Create(1, 1, 1)
            let m = three.MeshBasicMaterial.Create(box {| map = treesTexture; |} :?> _)
            three.Mesh.Create(g, m)
        // threejs draws cubes with their center point at the position
        cube.position.x <- float x - 0.5
        cube.position.y <- 0.5
        cube.position.z <- float y + 0.5
        scene.add(cube)
        |> ignore
    // Floor
    let floor =
        let g = three.BoxGeometry.Create(200, 1, 200)
        let m = three.MeshBasicMaterial.Create(box {| color = "grey" |} :?> _)
        three.Mesh.Create(g, m)
    floor.position.y <- -0.5
    // camera.up.y <- 1
    // camera.up.set (1, 1, -1)
    scene.add floor
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
    document.getElementById "root" |> ReactDOM.createRoot |> fun root -> root.render (Game (r, scene, camera, sprites))