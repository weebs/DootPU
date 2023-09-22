module Dootverse.Game
open System
open System.Collections.Generic
open Browser.Types
open Dootverse.Client.Svg
open Browser
open Fable.Core
open Feliz

// open Doot.Maths

open Dootverse.Models
open Doot.Maths.Voxel.Traversal
open Dootverse
open PGA

type Dimensions =
    abstract member top: int
    abstract member left: int
    abstract member width: int
    abstract member height: int
type Gif = interface end
// type GifFrame =
//     abstract member pixels: byte[]
//     abstract member colorTable: JS.Uint8Array[]
//     abstract member dims: Dimensions
type GifFrame =
    { pixels: byte[]; colorTable: JS.Uint8Array[]; dims: Dimensions }
    member this.AsImage =
        let data = this.pixels |> Array.map (int >> Array.get this.colorTable >> fun items -> (items[0], items[1], items[2]))
        let img = JS.Constructors.Uint8ClampedArray.Create (data.Length * 4)
        data |> Array.iteri (fun index (r, g, b) ->
            let offset = index * 4
            img[offset] <- r
            img[offset + 1] <- g
            img[offset + 2] <- b
            img[offset + 3] <- 255uy
        )
        ImageData.Create(img :> obj :?> _, this.dims.width, this.dims.height)
type GifJs =
    abstract member parseGIF: JS.ArrayBuffer -> Gif
    abstract member decompressFrames: Gif -> bool -> bool -> GifFrame[]
let gif: GifJs = JsInterop.importAll "gifuct-js"    

module Engine =
    let mutable debugKeys = true
    let keysPressed = Dictionary<string, bool>()
    let keysJustPressed = Dictionary<string, bool>()
    let isKeyPressed (key: string) =
        let key = key.ToLower()
        if keysPressed.ContainsKey key then keysPressed[key] else false
    let isKeyJustPressed key =
        if keysJustPressed.ContainsKey key then keysJustPressed[key] else false
    window.onkeydown <- fun key ->
        if debugKeys then
            console.log key
        let c = key.key.ToLower()
        // if c = "escape" then
        //     key.preventDefault()
        if c = "tab" then key.preventDefault()
        if not <| isKeyPressed c then
            keysJustPressed[c] <- true
        keysPressed[c] <- true
    window.onkeyup <- fun key ->
        keysPressed[key.key.ToLower()] <- false
    window.onblur <- fun _ ->
        for kv in keysPressed do
            keysPressed[kv.Key.ToLower()] <- false
    let mutable mouseX = 0
    let mutable mouseY = 0
    document.body.onmousemove <- fun ev ->
        if document.pointerLockElement <> null then
            mouseX <- mouseX + int ev.movementX
            mouseY <- mouseY + int ev.movementY
        else
            mouseX <- 0
            mouseY <- 0
        
    let mutable pointerState = document.pointerLockElement = null
    document.onpointerlockchange <-
        fun ev ->
            console.log ("element = ", document.pointerLockElement)
            pointerState <- document.pointerLockElement = null
            console.log ev
open Engine // todo
type IO() =
    static let mutable img = document.createElement "img" :?> HTMLImageElement
    static let mutable canvas = document.createElement "canvas" :?> HTMLCanvasElement
    
    static member loadImage (filePath: string) : JS.Promise<ImageData> =
        Promise.create (fun resolve reject ->
            img.setAttribute("src", filePath)
            img.onload <- fun _ ->
                canvas.width <- img.width
                canvas.height <- img.height
                
                let context2d = canvas.getContext_2d ()
                context2d.drawImage(U3.Case1 img, 0, 0)
                let wallTextureData = context2d.getImageData(0, 0, img.width, img.height)
                img.onload <- fun _ -> ()
                
                resolve wallTextureData
            img.onerror <- fun err ->
                img.onerror <- fun _ -> ()
                reject (Exception(string err))
        )
    // https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Pixel_manipulation_with_canvas
    static member loadImage (filePath: string, imageSize, (offsetX, offsetY)) : JS.Promise<ImageData> =
        Promise.create (fun resolve reject ->
            img.setAttribute("src", filePath)
            img.onload <- fun _ ->
                canvas.width <- img.width
                canvas.height <- img.height
                
                let context2d = canvas.getContext_2d ()
                context2d.drawImage(U3.Case1 img, 0, 0)
                let wallTextureData = context2d.getImageData(offsetX, offsetY, imageSize, imageSize)
                img.onload <- fun _ -> ()
                
                resolve wallTextureData
            img.onerror <- fun err ->
                img.onerror <- fun _ -> ()
                reject (Exception(string err))
        )
    static member loadImage (filePath: string, imageSize) : JS.Promise<ImageData> =
        IO.loadImage (filePath, imageSize, (0, 0))

let useRefState (state: 'a) =
    let (currentState, setCurrentState) = React.useState state
    let stateRef = React.useRef currentState
    stateRef, (fun state ->
        setCurrentState state
        stateRef.current <- state)

let mapWidth, mapHeight = 20, 20
let mapSize = 10
let mapData' = Map.ofArray [|
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


// let r = System.Random() TODO didn't produce random numbers?
let nextInt max = JS.Math.round(JS.Math.random() * float max) |> int
let mapData = Map.ofArray [|
    let mapSize = mapSize * 10
    for i in -mapSize..mapSize do
        (i, mapSize), "pink"
        (i, -mapSize), "pink"
        (-mapSize , i), "pink"
        (mapSize, i), "pink"
    for _ in 1..80 do
        let x = nextInt (mapSize * 2) - mapSize
        let y = nextInt (mapSize * 2) - mapSize
        (x, y), "green"
    for _ in 1..80 do
        let x = nextInt (mapSize * 2) - mapSize
        let y = nextInt (mapSize * 2) - mapSize
        (x, y), "blue"
    for _ in 1..80 do
        let x = nextInt (mapSize * 2) - mapSize
        let y = nextInt (mapSize * 2) - mapSize
        (x, y), "orange"
|]
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
// let interval = 7f
let mutable lastTime = 0.0
let mutable lastRenderTime = 0.0
let mutable renderFrameInterval = 25.0
let mutable lastUiUpdate = 0.0
let uiUpdateInterval = 100.0
open type PGA3D
let [<Emit("document.body.requestPointerLock($0)")>] requestPointerLock args = jsNative
[<ReactComponent>]
let GameWindow (game: Models.Game, entities: (float2f * Asset)[]) =
    let (Image wallTexture) = game.Assets["wall_texture"]
    let (Image itemTexture) = game.Assets["item"]
    // let screen = Screen(880, 1000)
    // let windowHeight = 480.
    // let windowWidth = 640.
    // let screenWidth = 0.5
    // let screenHeight = 0.375
    // let blockSize = 0.3f
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
    // let mutable playerRotationInRadians = 0.
    let playerRotation, setPlayerRotation = useRefState (Math.Tau / 2.0) //(Math.Tau / 4.0)
    // let focalLength = 0.5
    // let screenCasts = seq {
    //     let rayCount = 320
    //     for i in -(rayCount / 2) + 1..rayCount / 2 do
    //         yield (Vector2((float i / float rayCount) * (screenWidth / 2.), focalLength))
    //         // yield (Vector2(float i / float rayCount * screenSize, 1f))
    //     // yield Vector2(float 8 / float rayCount * screenSize, 1f)
    // }
    let canvasRef = React.useRef<HTMLCanvasElement> null
    let gamePausedRef, setGamePaused = useRefState true
    let menuOpen, setMenuOpen = useRefState true
    let gameState, setGameState = useRefState {
        playerPosition = { X = 0.; Y = 0. }
        playerRotation = 0.
        entities = entities
    }
    let renderSingleFrame = React.useRef true
    
    let playerPosition, setPlayerPosition =
        useRefState (Vector2(2.0, 8.0))
    
    
    // todo: Duplicating canvas to save state
    // do
        // let canvas = document.createElement "canvas" :?> HTMLCanvasElement
        // canvas.width <- 640
        // canvas.height <- 480
        // document.body.appendChild canvas |> ignore
        // canvasRef.current <- canvas
    let update (deltaTime: float) =
        if isKeyJustPressed "r" then
            renderSingleFrame.current <- true
        if isKeyJustPressed "p" then
            setGamePaused (not gamePausedRef.current)
            if document.pointerLockElement <> null then
                document.exitPointerLock()
                setMenuOpen true
        if isKeyJustPressed "enter" || isKeyJustPressed "tab" then
            if gamePausedRef.current then
                setGamePaused false
            if document.pointerLockElement = null then
                // requestPointerLock {| unadjustedMovement = true |}
                requestPointerLock ()
                setMenuOpen false
            else
                document.exitPointerLock()
                setMenuOpen true
        let speed = if isKeyPressed "shift" then 22.0 else 4.20
        // setInterval
        
    // Track start position in case new position collides with walls and we reset the player to the last position
        let originalPosition = playerPosition.current
        
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
            playerRotation.current <- playerRotation.current + (Math.Tau * deltaTime * 0.2)
        if isKeyPressed "e" then
            playerRotation.current <- playerRotation.current - (Math.Tau * deltaTime * 0.2)
        playerRotation.current <- playerRotation.current - (Math.Tau * deltaTime * float Engine.mouseX * 0.01)
            
        // velocityDirection <- velocityDirection / velocityDirection.Length()
        velocityDirection <- (Render.rotateVector playerRotation.current velocityDirection).Normalized * speed * deltaTime
        
        playerPosition.current <- playerPosition.current + velocityDirection
        
        // Revert position when colliding with a wall
        if mapData.ContainsKey (int (Math.Floor playerPosition.current.X), int (Math.Floor playerPosition.current.Y)) then
            playerPosition.current <- originalPosition
            
    let render (time: float) : unit =
        let canvas = canvasRef.current
        let ctx = canvas.getContext_2d ()
        let img, raycastDistances =
            Render.drawCamera
                wallTexture
                (int canvas.width)
                (int canvas.height)
                level
                playerPosition.current
                playerRotation.current
        let canvasData = ImageData.Create (img :> obj :?> _, int canvas.width, int canvas.height)
        
        // Draw item in bottom right of screen
        // Render.drawImage itemTexture { X = -itemTexture.width; Y = -itemTexture.height } canvasData
        gameState.current.entities |> Array.sortInPlaceBy (fun (p, _) -> -1.0 * (Math.Sqrt <| (p.X - playerPosition.current.X) ** 2.0 + (p.Y - playerPosition.current.Y) ** 2.0))
        for (position, asset) in gameState.current.entities do
            let (Image sprite) = asset
            let entityPoint = { X = position.X; Y = 1.4; Z = position.Y }
            let n =
                rotate(point(0f, 0f, 1f), rotor(float32 playerRotation.current, point(0f, 1f, 0f) &&& point(0f, 0f, 0f)))
                    .Vector
                    |> fun (x, y, z) -> { X = float x; Y = float y; Z = float z }
            let playerPt = { X = playerPosition.current.X; Y = playerPosition.current.Y }
            let cameraEyePt = { X = playerPt.X - n.X; Y = playerPt.Y - n.Z }
            // todo: Do camera plane calculation outside of this method since all iterations will have the same value
            let canvasOffsetFromCenter, distanceFromPlane = Render.worldCoordinatesToScreenCoordinates null playerPt (float32 playerRotation.current) entityPoint
            // let canvasOffsetFromCenter, distanceFromPlane = Render.worldCoordinatesToScreenCoordinates null cameraEyePt (float32 playerRotation.current) entityPoint
            let (offsetX, offsetY, offsetZ) = canvasOffsetFromCenter.Vector
            if renderSingleFrame.current then
                JS.debugger ()
            // TODO move normal calculation and isInFront to Render.world function
            // let n = { X = Math.Cos playerRotation.current; Y = 0.; Z = Math.Sin playerRotation.current }
            let dotProduct = // n * (a - p)
                n.Dot { X = entityPoint.X - (cameraEyePt.X + n.X); Y = entityPoint.Y - 0.5; Z = entityPoint.Z - (cameraEyePt.Y + n.Z) }
            console.log ("normal = ", n)
            console.log ("dotProduct =", dotProduct)
            let isInFront = dotProduct > 0
            // console.log "dot product ="
            let distanceFromPlayer = (playerPosition.current - Vector2(entityPoint.X, entityPoint.Z)).Length()
            console.log ("distance from player = ", distanceFromPlayer)
            if isInFront && distanceFromPlayer >= 1. && MathF.Abs(offsetZ) < 0.0001f then
                console.log ("offset = ", offsetX, offsetY)
                let positionX, positionY =
                    Render.cartesianToScreen
                        (int canvasData.width) (int canvasData.height)
                        (int (JS.Math.round (float offsetX * canvas.width)))
                        (int (JS.Math.round (float offsetY * canvas.height)))
                        
                let size = int (JS.Math.round (200. / (0.0 + float distanceFromPlane)))
                let scale = (sprite.width / (0.0 + float distanceFromPlane)) / sprite.width
                // todo: Drawing test rectangle
                // Render.drawRectangle (positionX - (size / 2)) (positionY + (size / 2)) size size (fun _ -> 0uy, 120uy, 255uy, 255uy) canvasData
                
                let size = sprite.width * scale |> JS.Math.round |> int
                // Render.drawScaledSprite sprite (positionX - (size / 2), (positionY + (size / 2))) scale canvasData
                Render.drawScaledSprite sprite (positionX - (size / 2), (positionY + (size / 2))) scale raycastDistances (float distanceFromPlane) canvasData
        ctx.putImageData (canvasData, 0, 0)
    // window.setInterval ((fun () ->
    let rec loop (time: float) =
        // console.log ("time = ", time)
        let deltaTime = float (time - lastTime) / 1000.
        // console.log ("delta time = ", deltaTime * 1000.)
        lastTime <- time
        
        // todo: use setInterval for the update loop ? avoid long waits from requestAnimationFrame when tab is not focused
        update deltaTime
        Engine.mouseX <- 0
        Engine.mouseY <- 0
        
        // Update react elements
        if time - lastUiUpdate > uiUpdateInterval then
            lastUiUpdate <- time
            setPlayerPosition playerPosition.current
            setPlayerRotation playerRotation.current
            
        // Needs to be called after every update
        for kv in keysJustPressed do
            keysJustPressed[kv.Key] <- false
        
        // todo: Alternatively, use setTimeout + call requestAnimationFrame to limit frame rate
        if not gamePausedRef.current || renderSingleFrame.current then
            if time - lastRenderTime > renderFrameInterval then
                // console.log ("last render time = ", time - lastRenderTime)
                render (time - lastRenderTime)
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
    let setPlayerPosition = fun f -> setPlayerPosition (f playerPosition.current)
    Html.div [
        Html.div [
            Html.p $"{playerPosition.current}"
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
        Html.h4 $"Rotation: {playerRotation.current}"
        Html.div [
            Html.div [
                prop.style [
                    style.position.absolute
                    style.zIndex 2
                ]
                prop.children [
                    Svg.svg [
                        svg.children [
                            Svg.circle [
                                svg.r 10
                                svg.fill "blue"
                            ]
                            if menuOpen.current then
                                Svg.text [
                                    svg.text "User Interface"
                                    svg.stroke "green"
                                    svg.fill "grey"
                                    svg.className "large"
                                    svg.x 100
                                    svg.y 100
                                    svg.fontSize 12
                                ]
                        ]
                    ]
                ]
            ]
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
                    console.log playerRotation.current
                    let rayDirection = Render.getRaycastAtColumn width playerRotation.current (int (JS.Math.round x))
                    console.log ("Ray direction = ", rayDirection)
                    let raycast = findIntersection playerPosition.current rayDirection level.ContainsKey
                    match raycast with
                    | Some ((voxelX, voxelY), (pointX, pointY)) ->
                        console.log ("hit voxel", voxelX, ",", voxelY)
                        console.log ("hit voxel at point", pointX, ",", pointY)
                    | _ -> ()
                    try
                        canvasRef.current.requestPointerLock ()
                    with error -> console.log error
                )
                prop.style [
                    style.border (1, borderStyle.solid, "blue")
                    style.position.relative
                    style.top 0
                    style.zIndex -1
                    // style.display.none
                ]
            ]
        ]
    ]
let createGameRoot () = promise {
    console.log gif
    // https://github.com/matt-way/gifuct-js
    let! response = Fetch.fetch "sword_character.gif" []
    // todo: Gif
    let! response = Fetch.fetch "character.gif" []
    let! buffer = response.arrayBuffer()
    let gifData = gif.parseGIF buffer
    let frames = gif.decompressFrames gifData false false
    console.log gifData
    console.log frames
    console.log frames[12].AsImage
    let! wallTextureData = IO.loadImage ("image.png", 64)
    let! heartTextureData =
        IO.loadImage "heart.png"
        |> Promise.map (Render.scaleImage 8)
    let transparency = wallTextureData.data[0], wallTextureData.data[1], wallTextureData.data[2]
    console.log transparency
    let gameEntities = [| //[|
         { X = 2.; Y = 4. }, Image heartTexture
         for i in 1..200 do
             { X = JS.Math.random() * 80.0; Y = JS.Math.random() * 80.0; }, Image heartTexture
             // { X = JS.Math.random() * 80.0; Y = JS.Math.random() * 80.0; }, Image blueHeartTexture
             // { X = JS.Math.random() * 20.0; Y = JS.Math.random() * 20.0; }, Image characterTexture
     |]
    let game = {
        Assets = Map.ofArray [|
            "wall_texture", Image wallTextureData
            "item", Image heartTextureData
        |]
    }
    (ReactDOM.createRoot (document.getElementById "root")).render(GameWindow (game, gameEntities))
}