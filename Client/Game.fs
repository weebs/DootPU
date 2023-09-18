module Dootverse.Game

open System
open System.Collections.Generic
open Browser.Types
open Dootverse.Client.Svg
open Browser
open Fable.Core
open Feliz
open Doot.Maths
open Dootverse.Models
open Doot.Maths.Voxel.Traversal
open Dootverse

// open Doot.Maths.System.Numerics

        // |> Seq.take 10
        // |> Seq.iter (fun (x, y) ->
        //     if grid.ContainsKey (x, y) then
        //         console.log ("x = ", x, "y = ", y)
        // )
let mapWidth, mapHeight = 20, 20
let mapSize = 10
let mapData = Map.ofArray [|
    // (-10, 10), "pink"
    for i in -mapSize..mapSize do
        (i, mapSize), "pink"
        (i, -mapSize), "pink"
        (-mapSize , i), "pink"
        (mapSize, i), "pink"
    // (-2, 2), "pink"
    (8, 8), "blue"
    (2, 3), "green"
    (-4, 4), "orange"
|]
// type Screen = {
    // voxel: int -> int -> int -> string -> ReactElement
    // width: int
// } with
    // member this.voxel size x y color = Svg.rect []
let drawMinimap (screen: Screen) = [|
    for kv in mapData do
        let (x, y) = kv.Key
        yield screen.voxel 40 x y kv.Value
|]
    
let toRgb color =
    match color with
    | "green" -> (0uy, 255uy, 0uy)
    | "pink" -> (255uy, 200uy, 200uy)
    | "orange" -> (255uy, 120uy, 50uy)
    | "blue" -> (0uy, 0uy, 255uy)
    | _ -> (0uy, 0uy, 0uy)
let level =
    let d = Dictionary()
    mapData |> Map.iter (fun key value -> d[key] <- toRgb value)
    d
let useRefState (state: 'a) =
    let (currentState, setCurrentState) = React.useState state
    let stateRef = React.useRef currentState
    stateRef, (fun state ->
        setCurrentState state
        stateRef.current <- state)
let mutable playerPosition = Vector2(2., 0.)
let keysPressed = Dictionary<string, bool>()
let keysJustPressed = Dictionary<string, bool>()
let isKeyPressed key =
    if keysPressed.ContainsKey key then keysPressed[key] else false
let isKeyJustPressed key =
    if keysJustPressed.ContainsKey key then keysJustPressed[key] else false
window.onkeydown <- fun key ->
    // console.log key
    let c = key.key.ToLower()
    if not <| isKeyPressed c then
        keysJustPressed[c] <- true
    keysPressed[key.key.ToLower()] <- true
window.onkeyup <- fun key ->
    keysPressed[key.key.ToLower()] <- false
window.onblur <- fun ev ->
    for kv in keysPressed do
        keysPressed[kv.Key] <- false
// let interval = 7f
let mutable lastTime = 0.0
let mutable lastRenderTime = 0.0
let mutable renderFrameInterval = 25.0
[<ReactComponent>]
let GameWindow wallTextureData =
    let screen = Screen(880, 1000)
    let windowHeight = 480.
    let windowWidth = 640.
    let screenWidth = 0.5
    let screenHeight = 0.375
    let blockSize = 0.3f
    // todo: edge case in voxel traversal with cube at (-4, 4)
    // let playerPosition, setPlayerPosition = React.useStateWithUpdater(Vector2((-4.0000007450581f, -2.9802322387695312e-8f)))
    // todo: another edge case at (-5.000000022351742, -1.0000000447034836)
    
    // todo: screenWidth = 1f blockSize = 1f screenHeight = 0.375f
    // let windowHeight = 480.
    // let windowWidth = 640.
    // todo: -1, 7 also has errors (looks neat tho)
    // todo: -0, 7 also has errors second cube at (2, 3))
    // todo: 1, 7 also has errors
    // todo: (0.8999999985098839, -7) with cube at (8, 8) (1 right from 1, -7)
    let mutable playerRotationInRadians = 0. 
    let focalLength = 0.5
    let screenCasts = seq {
        let rayCount = 320
        for i in -(rayCount / 2) + 1..rayCount / 2 do
            yield (Vector2((float i / float rayCount) * (screenWidth / 2.), focalLength))
            // yield (Vector2(float i / float rayCount * screenSize, 1f))
        // yield Vector2(float 8 / float rayCount * screenSize, 1f)
    }
    let canvasRef = React.useRef<HTMLCanvasElement> null
    let gamePausedRef, setGamePaused = useRefState true
    let gameState, setGameState = useRefState { playerPosition = { X = 0.; Y = 0. }; playerRotation = 0. }
    let renderSingleFrame = React.useRef true
    
    
    // React.useEffectOnce <| fun () ->
    // todo: Duplicating canvas to save state
    // do
        // let canvas = document.createElement "canvas" :?> HTMLCanvasElement
        // canvas.width <- 640
        // canvas.height <- 480
        // document.body.appendChild canvas |> ignore
        // canvasRef.current <- canvas
    let update (deltaTime: float) =
        if isKeyPressed "r" then
            renderSingleFrame.current <- true
        let speed = 2.8
        // setInterval
        
    // Track start position in case new position collides with walls and we reset the player to the last position
        let originalPosition = playerPosition
        
        let mutable velocityDirection = Vector2(0., 0.)
        if isKeyPressed "d" || isKeyPressed "ArrowRight" then
            velocityDirection <- velocityDirection + Vector2(speed, 0.)
        if isKeyPressed "w" || isKeyPressed "ArrowUp" then
            velocityDirection <- velocityDirection + Vector2(0., speed)
        if isKeyPressed "a" || isKeyPressed "ArrowLeft" then
            velocityDirection <- velocityDirection + Vector2(-speed, 0.)
        if isKeyPressed "s" || isKeyPressed "ArrowDown" then
            velocityDirection <- velocityDirection + Vector2(0., -speed)
            
        
        if isKeyPressed "q" then
            playerRotationInRadians <- playerRotationInRadians + (Math.Tau * deltaTime * 0.2)
        if isKeyPressed "e" then
            playerRotationInRadians <- playerRotationInRadians - (Math.Tau * deltaTime * 0.2)
            
        // velocityDirection <- velocityDirection / velocityDirection.Length()
        velocityDirection <- (Render.rotateVector playerRotationInRadians velocityDirection).Normalized * speed * deltaTime
        
        playerPosition <- playerPosition + velocityDirection
        
        if mapData.ContainsKey (int (Math.Floor playerPosition.X), int (Math.Floor playerPosition.Y)) then
            // playerPosition <- Vector2(0., 0.)
            playerPosition <- originalPosition
        
        if isKeyJustPressed "p" then
            setGamePaused (not gamePausedRef.current)
    let render (time: float) : unit =
    // window.setInterval ((fun () ->
        let canvas = canvasRef.current
        let ctx = canvas.getContext_2d ()
        let img =
            Render.drawCamera wallTextureData (int canvas.width) (int canvas.height) level playerPosition playerRotationInRadians
        let imgData = ImageData.Create (img :> obj :?> _, int canvas.width, int canvas.height)
        // console.log imgData
        ctx.putImageData (imgData, 0, 0)
        // if not gamePausedRef.current then
        //     window.requestAnimationFrame render
        //     |> ignore
    let rec loop (time: float) =
        // let deltaTime = interval / 1000.
        // requestAnimationFrame
        // console.log ("time = ", time)
        let deltaTime = float (time - lastTime) / 1000.
        // console.log ("delta time = ", deltaTime * 1000.)
        lastTime <- time
        
        update deltaTime
        // Needs to be called after every update
        for kv in keysJustPressed do
            keysJustPressed[kv.Key] <- false
        
        if not gamePausedRef.current || renderSingleFrame.current then
            if time - lastRenderTime > renderFrameInterval then
                // console.log ("last render time = ", time - lastRenderTime)
                render time
                renderSingleFrame.current <- false
                lastRenderTime <- time
        window.requestAnimationFrame loop
        |> ignore
    React.useEffectOnce <| fun () ->
            // if not gameStateRef.current then
            window.requestAnimationFrame loop
            |> ignore
            // ), int interval)
            // |> ignore
    let playerPosition, setPlayerPosition =
        // React.useStateWithUpdater(Vector2(-4.2f, 3f))
        React.useStateWithUpdater(Vector2(2.1, 0.))
    Html.div [
        Html.div [
            Html.p $"{playerPosition}"
            Html.button [
                prop.text "Render Single Frame"
                prop.onClick (fun _ -> renderSingleFrame.current <- true)
            ]
            Html.button [
                prop.onClick (fun _ ->
                    if not gamePausedRef.current then
                        setGamePaused true
                    else
                        window.requestAnimationFrame render
                        |> ignore
                        setGamePaused false
                )
                prop.text (if gamePausedRef.current then "Play" else "Pause")
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(-0.1, 0.)))
                prop.text "Left"
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(0.1, 0.)))
                prop.text "Right"
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(0., 0.1)))
                prop.text "Forward"
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(0., -0.1)))
                prop.text "Back"
            ]
        ]
        Html.h4 $"Rotation: {playerRotationInRadians}"
        Html.canvas [
            prop.width 640
            prop.height 480
            prop.ref (fun e -> canvasRef.current <- e :?> _)
            prop.onClick (fun ev ->
                let rect = canvasRef.current.getBoundingClientRect ()
                let x, y = ev.clientX - rect.left, ev.clientY - rect.top
                let x = x - (canvasRef.current.width / 2.)
                let width = canvasRef.current.width |> int
                // let px, py = x / canvasRef.current.width, y / canvasRef.current.height
                console.log x
                console.log playerRotationInRadians
                let rayDirection = Render.getRaycastAtColumn width playerRotationInRadians (int (JS.Math.round x))
                console.log ("Ray direction = ", rayDirection)
                let raycast = findIntersection playerPosition rayDirection level.ContainsKey
                match raycast with
                | Some ((voxelX, voxelY), (pointX, pointY)) ->
                    console.log ("hit voxel", voxelX, ",", voxelY)
                    console.log ("hit voxel at point", pointX, ",", pointY)
                | _ -> ()
                // console.log y
                // console.log px
                // console.log py
            )
            prop.style [
                style.border (1, borderStyle.solid, "blue")
            ]
        ]
    ]
// try drawMap width height map with error -> console.log ("error = ", error)

type IO() =
    static let mutable img = document.createElement "img" :?> HTMLImageElement
    static let mutable canvas = document.createElement "canvas" :?> HTMLCanvasElement
    static member loadImage (filePath: string) imageSize : JS.Promise<ImageData> =
        Promise.create (fun resolve reject ->
            img.setAttribute("src", filePath)
            img.onload <- fun _ ->
                canvas.width <- img.width
                canvas.height <- img.height
                let context2d = canvas.getContext_2d ()
                // Load wall texture
                context2d.drawImage(U3.Case1 img, 0, 0)
                let wallTextureData = context2d.getImageData(0, 0, imageSize, imageSize)
                
                resolve wallTextureData
            img.onerror <- fun err ->
                reject (Exception(string err))
        )
        
    
let createGameRoot () = promise {
    let! wallTextureData = IO.loadImage "image.png" 64
    ReactDOM.createRoot (document.getElementById "root")
    |> fun root -> root.render(GameWindow wallTextureData)
}