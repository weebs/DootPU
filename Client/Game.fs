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
open Engine
open Thoth.Json // todo

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

type IO() =
    static let mutable img = document.createElement "img" :?> HTMLImageElement
    static let mutable canvas = document.createElement "canvas" :?> HTMLCanvasElement
    
    static member loadImage (filePath: string) : JS.Promise<AssetId * ImageData> =
        Promise.create (fun resolve reject ->
            img.setAttribute("src", filePath)
            img.onload <- fun _ ->
                canvas.width <- img.width
                canvas.height <- img.height
                
                let context2d = canvas.getContext_2d ()
                context2d.drawImage(U3.Case1 img, 0, 0)
                let wallTextureData = context2d.getImageData(0, 0, img.width, img.height)
                img.onload <- fun _ -> ()
                
                resolve (AssetId filePath, wallTextureData)
            img.onerror <- fun err ->
                img.onerror <- fun _ -> ()
                reject (Exception(string err))
        )
    // https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API/Tutorial/Pixel_manipulation_with_canvas
    static member loadImage (filePath: string, imageSize, (offsetX, offsetY)) : JS.Promise<AssetId * ImageData> =
        Promise.create (fun resolve reject ->
            img.setAttribute("src", filePath)
            img.onload <- fun _ ->
                canvas.width <- img.width
                canvas.height <- img.height
                
                let context2d = canvas.getContext_2d ()
                context2d.drawImage(U3.Case1 img, 0, 0)
                let wallTextureData = context2d.getImageData(offsetX, offsetY, imageSize, imageSize)
                img.onload <- fun _ -> ()
                
                resolve (AssetId filePath, wallTextureData)
            img.onerror <- fun err ->
                img.onerror <- fun _ -> ()
                reject (Exception(string err))
        )
    static member loadImage (filePath: string, imageSize) : JS.Promise<AssetId * ImageData> =
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
// let drawMinimap (screen: Screen) = [|
//     for kv in mapData do
//         let (x, y) = kv.Key
//         yield screen.voxel 40 x y kv.Value
// |]
    
let toRgb color =
    match color with
    | "green" -> (0uy, 255uy, 0uy)
    | "pink" -> (255uy, 200uy, 200uy)
    | "orange" -> (255uy, 120uy, 50uy)
    | "blue" -> (0uy, 0uy, 255uy)
    | _ -> (0uy, 0uy, 0uy)
// let interval = 7f
let mutable lastTime = 0.0
let mutable lastRenderTime = 0.0
let mutable renderFrameInterval = 25.0
let mutable lastUiUpdate = 0.0
let uiUpdateInterval = 100.0
open type PGA3D
let [<Emit("document.body.requestPointerLock($0)")>] requestPointerLock args = jsNative
    
// |]
let wallId = AssetId "image.png"
let mutable frameCount = 0
let cameraPlane = point(0f, 0.5f, 1f) &&& point(0f, 1f, 1f) &&& point(1f, 0.5f, 1f)
let cameraEye = point(0f, 0.5f, 0f)
let worldToScreen displacement inverseRotation (worldPt: float3f) =
    let (fx, fy, fz) = rotate(point(0f, 0f, 1f), ~~~inverseRotation).Vector
    let localPt = rotate(point(float32 worldPt.X + fx, float32 worldPt.Y + fy, float32 worldPt.Z + fz) - displacement, inverseRotation)
    let dir = localPt &&& cameraEye
    let pointOnCameraPlane = cameraPlane ^^^ dir
    let (px, py, pz) = pointOnCameraPlane.Vector
    let distanceFromPlane = distance(localPt, cameraPlane)
    (px, py - 0.5f, pz), distanceFromPlane
let createWorld heart blueHeart =
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
    // let level =
    //     let d = Dictionary()
    //     mapData |> Map.iter (fun key value -> d[key] <- toRgb value)
    //     d
    {
        playerPosition = { X = 0.; Y = 0. }
        playerRotation = 0.
        entities = [|
            { X = 2.; Y = 4. }, heart
            for i in 1..40 do
                { X = JS.Math.random() * 80.0; Y = JS.Math.random() * 80.0; }, heart
                { X = JS.Math.random() * 80.0; Y = JS.Math.random() * 80.0; }, blueHeart
        |]
        Walls = mapData |> Map.map (fun _ value -> toRgb value)
    }
[<ReactComponent>]
let GameWindow (scene: Models.Scene) =
    let (Image wallTexture) = scene.Assets[wallId]
    // let (Image itemTexture) = game.Assets["item"]
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
    // let playerRotation, setPlayerRotation = useRefState (Math.Tau / 2.0) //(Math.Tau / 4.0)
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
    let frameTime, setFrameTime = useRefState 0.
    // World is the initial state of the scene
    let gameState, setGameState = useRefState scene.World
    let renderSingleFrame = React.useRef true
    let userInterfaceFocused, setUserInterfaceFocused = React.useState false
    let level = scene.World.Walls
    
    
    // todo: Duplicating canvas to save state
    // do
        // let canvas = document.createElement "canvas" :?> HTMLCanvasElement
        // canvas.width <- 640
        // canvas.height <- 480
        // document.body.appendChild canvas |> ignore
        // canvasRef.current <- canvas
    let update (deltaTime: float) gameState =
        if Keys.isJustPressed "r" then
            renderSingleFrame.current <- true
        if Keys.isJustPressed "p" then
            setGamePaused (not gamePausedRef.current)
            if document.pointerLockElement <> null then
                document.exitPointerLock()
                setMenuOpen true
        if Keys.isJustPressed "enter" || Keys.isJustPressed "tab" then
            if gamePausedRef.current then
                setGamePaused false
            if document.pointerLockElement = null then
                // requestPointerLock {| unadjustedMovement = true |}
                requestPointerLock ()
                setMenuOpen false
            else
                document.exitPointerLock()
                setMenuOpen true
        let speed = if Keys.isPressed "shift" then 22.0 else 4.20
        // setInterval
        
    // Track start position in case new position collides with walls and we reset the player to the last position
        
        let mutable velocityDirection = Vector2(0., 0.)
        if Keys.isPressed "d" || Keys.isPressed "ArrowRight" then
            velocityDirection <- velocityDirection + Vector2(speed, 0.)
        if Keys.isPressed "w" || Keys.isPressed "ArrowUp" then
            velocityDirection <- velocityDirection + Vector2(0., speed)
        if Keys.isPressed "a" || Keys.isPressed "ArrowLeft" then
            velocityDirection <- velocityDirection + Vector2(-speed, 0.)
        if Keys.isPressed "s" || Keys.isPressed "ArrowDown" then
            velocityDirection <- velocityDirection + Vector2(0., -speed)
            
        
        let newRotation =
            let mutable playerRotation = gameState.playerRotation
            if Keys.isPressed "q" then
                playerRotation <- playerRotation + (Math.Tau * deltaTime * 0.2)
            if Keys.isPressed "e" then
                playerRotation <- playerRotation - (Math.Tau * deltaTime * 0.2)
            playerRotation <- playerRotation - (Math.Tau * deltaTime * float Engine.mouseX * 0.01)
            playerRotation
            
        // velocityDirection <- velocityDirection / velocityDirection.Length()
        velocityDirection <- (Render.rotateVector newRotation velocityDirection).Normalized * speed * deltaTime
        
        let newPosition =
            let newPosition = gameState.playerPosition + velocityDirection
            // Revert position when colliding with a wall
            let key = int (Math.Floor newPosition.X), int (Math.Floor newPosition.Y)
            console.log key
            if scene.World.Walls.ContainsKey (int (Math.Floor newPosition.X), int (Math.Floor newPosition.Y)) then
                console.log ("colliding with wall at new position", newPosition)
                gameState.playerPosition
            else
                newPosition
        
        { gameState with
            playerPosition = newPosition
            playerRotation = newRotation }
            
    let render (time: float) : unit =
        frameCount <- frameCount + 1
        if frameCount % 10 = 0 then
            setFrameTime time
        let canvas = canvasRef.current
        let ctx = canvas.getContext_2d ()
        let img, raycastDistances =
            Render.drawCamera
                wallTexture
                (int canvas.width)
                (int canvas.height)
                level
                gameState.current.playerPosition.Vector2
                gameState.current.playerRotation
        let canvasData = ImageData.Create (img :> obj :?> _, int canvas.width, int canvas.height)
        
        // Draw item in bottom right of screen
        // Render.drawImage itemTexture { X = -itemTexture.width; Y = -itemTexture.height } canvasData
        gameState.current.entities |> Array.sortInPlaceBy (fun (p, _) -> -1.0 * (Math.Sqrt <| (p.X - gameState.current.playerPosition.X) ** 2.0 + (p.Y - gameState.current.playerPosition.Y) ** 2.0))
        let playerPosition = gameState.current.playerPosition.Vector2
        let pgaDisplacement = direction(float32 playerPosition.X, 0f, float32 playerPosition.Y)
        let pgaRotationReversed = rotor(float32 -gameState.current.playerRotation, point(0f, 1f, 0f) &&& point(0f, 0f, 0f))
        let n =
            let rotationLine = point(0f, 1f, 0f) &&& point(0f, 0f, 0f)
            rotate(point(0f, 0f, 1f), rotor(float32 gameState.current.playerRotation, rotationLine))
                .Vector
                |> fun (x, y, z) -> { X = float x; Y = float y; Z = float z }
        for (position, assetId) in gameState.current.entities do
            let (Image sprite) = scene.Assets[assetId]
            let entityPoint = { X = position.X; Y = 1.4; Z = position.Y }
            let playerPt = { X = gameState.current.playerPosition.X; Y = gameState.current.playerPosition.Y }
            let cameraEyePt = { X = playerPt.X - n.X; Y = playerPt.Y - n.Z }
            // todo: Do camera plane calculation outside of this method since all iterations will have the same value
            let canvasOffsetFromCenter, distanceFromPlane = worldToScreen pgaDisplacement pgaRotationReversed entityPoint
            // let canvasOffsetFromCenter, distanceFromPlane =
            //     Render.worldCoordinatesToScreenCoordinates null playerPt (float32 gameState.current.playerRotation) entityPoint
            // let canvasOffsetFromCenter, distanceFromPlane = Render.worldCoordinatesToScreenCoordinates null cameraEyePt (float32 playerRotation.current) entityPoint
            
            // let (offsetX, offsetY, offsetZ) = canvasOffsetFromCenter.Vector
            let (offsetX, offsetY, offsetZ) = canvasOffsetFromCenter //.Vector
            // console.log ("offset = ", canvasOffsetFromCenter)
            
            // if renderSingleFrame.current then
                // JS.debugger ()
            // TODO move normal calculation and isInFront to Render.world function
            // let n = { X = Math.Cos playerRotation.current; Y = 0.; Z = Math.Sin playerRotation.current }
            let dotProduct = // n * (a - p)
                n.Dot { X = entityPoint.X - (cameraEyePt.X + n.X); Y = entityPoint.Y - 0.5; Z = entityPoint.Z - (cameraEyePt.Y + n.Z) }
            // console.log ("normal = ", n)
            // console.log ("dotProduct =", dotProduct)
            let isInFront = dotProduct > 0
            // console.log "dot product ="
            let distanceFromPlayer = (playerPosition - Vector2(entityPoint.X, entityPoint.Z)).Length()
            // console.log ("distance from player = ", distanceFromPlayer)
            if isInFront && distanceFromPlayer >= 1. then // && MathF.Abs(offsetZ) < 0.0001f then
                // console.log ("offset = ", offsetX, offsetY)
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
        
        // Update react elements
        if time - lastUiUpdate > uiUpdateInterval then
            lastUiUpdate <- time
            // todo : SetUiGameState
            setGameState gameState.current
            // setPlayerPosition playerPosition
            // setPlayerRotation playerRotation.current
        
        // todo: use setInterval for the update loop ? avoid long waits from requestAnimationFrame when tab is not focused
        let newState = update deltaTime gameState.current
        gameState.current <- newState
        localStorage.setItem("save/world.data", Encode.Auto.toString gameState.current)
        Engine.mouseX <- 0
        Engine.mouseY <- 0
            
        // Needs to be called after every update
        for kv in Keys.justPressed do
            Keys.justPressed[kv.Key] <- false
        
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
    let setPlayerPosition = fun f ->
        let p = f gameState.current.playerPosition
        setGameState { gameState.current with playerPosition = p }
    Html.div [
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
                                    svg.stroke (if userInterfaceFocused then "yellow" else "green")
                                    svg.fill "grey"
                                    svg.className "large"
                                    svg.x 10
                                    svg.y 20
                                    svg.fontSize 12
                                    svg.onMouseEnter (fun _ -> setUserInterfaceFocused true)
                                    svg.onMouseLeave (fun _ -> setUserInterfaceFocused false)
                                ]
                        ]
                    ]
                ]
            ]
            Html.canvas [
                prop.width 640
                prop.height 400
                prop.ref (fun e -> canvasRef.current <- e :?> _)
                prop.onClick (fun ev ->
                    let rect = canvasRef.current.getBoundingClientRect ()
                    let x, y = ev.clientX - rect.left, ev.clientY - rect.top
                    let x = x - (canvasRef.current.width / 2.)
                    let width = canvasRef.current.width |> int
                    // let px, py = x / canvasRef.current.width, y / canvasRef.current.height
                    console.log x
                    // console.log playerRotation.current
                    let rayDirection = Render.getRaycastAtColumn width gameState.current.playerRotation (int (JS.Math.round x))
                    console.log ("Ray direction = ", rayDirection)
                    let raycast = findIntersection gameState.current.playerPosition.Vector2 rayDirection level.ContainsKey
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
        Html.div [
            Html.h4 frameTime.current
            Html.span $"{gameState.current.playerPosition}"
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
                prop.onClick (fun _ -> setGameState (createWorld (AssetId "heart.png") (AssetId "blue_heart.png")))
                prop.text "Reset world"
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(0., -0.1)))
                prop.text "Back"
            ]
            Html.h4 $"Rotation: {gameState.current.playerRotation}"
        ]
    ]
// type [<Measure>] AssetId = class end
// type [<Measure>] ImagePath = class end
// type AssetId with
    // with
    // static member (+) (a: int<AssetId>, b: int) =
        // let n = (int a + b)
        // LanguagePrimitives.Int32WithMeasure<AssetId> n
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
    
    let! _, wallTexture  = IO.loadImage ("image.png", 64)
    let transparency = wallTexture.data[0], wallTexture.data[1], wallTexture.data[2]
    console.log transparency
    let wall = wallId, Image (Render.scaleImage 4 wallTexture)
    Assets.RegisterAsset (wallId, snd wall)
    
    let! heartId, heartTexture =
        IO.loadImage "heart.png"
    let heart =
        heartId,
        heartTexture |> Render.scaleImage 8 |> Image
    Assets.RegisterAsset heart
    
    let! _, blueHeartTexture =
        IO.loadImage "heart.png"
    for i in 0..int (blueHeartTexture.width * blueHeartTexture.height) - 1 do
        let i = i * 4
        let blue = blueHeartTexture.data[i + 2]
        blueHeartTexture.data[i + 1] <- blueHeartTexture.data[i]
        blueHeartTexture.data[i + 2] <- blueHeartTexture.data[i]
        blueHeartTexture.data[i] <- blue
    let blueHeart =
        AssetId "blue_heart.png",
        blueHeartTexture |> Render.scaleImage 8 |> Image
    Assets.RegisterAsset blueHeart
        
    let! characterTexture = IO.loadImage "sword_character.gif"
    JS.console.log heartId
    
    let world =
        match Decode.Auto.fromString<GameWorldState> localStorage["save/world.data"] with
        | Ok data -> data
        | Error err ->
            console.log err
            let world = createWorld (fst heart) (fst blueHeart)
            localStorage.setItem("save/world.data", Encode.Auto.toString world)
            world
    let game = {
        Assets = Assets.All
        World = world
    }
    (ReactDOM.createRoot (document.getElementById "root")).render(GameWindow game)
}