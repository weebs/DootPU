module Dootverse.Game

open System
open System.Collections.Generic
open Browser.Types
open Dootverse.Client.Svg
open Browser
open Fable.Core
open Feliz
open Doot.Maths
open Doot.Maths.Voxel.Traversal
// open Doot.Maths.System.Numerics

// todo: size scaling
let traverseRay size (start: Vector2) (ray: Vector2) =
    findVoxelsAlongRay (start / float32 size) (ray / float32 size)

let drawMap width height (grid: Map<int * int, string>) =
    let arr = JS.Constructors.Uint8ClampedArray.Create (width * height * 4)
    let start = Vector2(0f, 0f)
    console.log ("start = ")
    console.log start
    let dir = Vector2(1f, 1f)
    let rays = seq {
        for i in -320 / 2..-1 do
            yield (Vector2(0f, 0f), Vector2(float32 i / 160f, 1f))
        for i in 1..320 / 2 do
            yield (Vector2(0f, 0f), Vector2(float32 i / 160f, 1f))
    }
    seq {
        for (start, ray) in rays do
            let coordinates = Voxel.Traversal.traverseRay 1 start ray
            let steps =
                coordinates
                |> Seq.takeWhile (fun (x, y) -> x <= width && y <= height)
            steps
            |> Seq.tryFind grid.ContainsKey
            // |> Option.iter (fun coords ->
            //     console.log ("steps = ", Array.ofSeq steps)
            //     console.log ("ray = ", ray)
            //     console.log (coords, " => ", grid[coords])
            //     )
    }
        // |> Seq.take 10
        // |> Seq.iter (fun (x, y) ->
        //     if grid.ContainsKey (x, y) then
        //         console.log ("x = ", x, "y = ", y)
        // )
let width, height = 20, 20
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
let rotate pt radians =
    let (x, y) = pt
    (x * MathF.Cos radians - y * MathF.Sin radians,
     y * MathF.Cos radians + x * MathF.Sin radians)
let screenColumns width rotationRadians (offset: Vector2) =
    let x = 0f
    let y = 1f
    let x1 = 1f
    let y1 = 1f
    let (rx, ry) = rotate (x, y) rotationRadians
    let (rx1, ry1) = rotate (x1, y1) rotationRadians
    let (a, b) = (rx1 - rx, ry1 - ry)
    // console.log ("theta = ", rotationRadians)
    // console.log ("a, b = ", (a, b))
    let magnitude = MathF.Sqrt((a * a) + (b * b))
    let lineSlope = (a / magnitude, b / magnitude)
    let (run, rise) = lineSlope
    // console.log ("line slope = ", lineSlope)
    [|
    // seq {
        for i in -width / 2..width / 2 do
            let i = float32 i / (float32 width)
            // Vector2((rx + (run * float32 i)) + offset.X, (ry + (rise * float32 i)) + offset.Y)
            Vector2((rx + (run * float32 i)), (ry + (rise * float32 i)))
    // }
    |]
let distanceFromLine (lineA: Vector2) (lineB: Vector2) (pt: Vector2) =
    let numerator = MathF.Abs(((lineB.X - lineA.X) * (lineA.Y - pt.Y)) - ((lineA.X - pt.X) * (lineB.Y - lineA.Y)))
    let denom = MathF.Sqrt((lineB.X - lineA.X) ** 2f + (lineB.Y - lineA.Y) ** 2f)
    numerator / denom
let cameraLine rotation position =
    let x = 0f
    let y = 1f
    let x1 = 1f
    let y1 = 1f
    let cameraOrigin = Vector2 (rotate (x, y) rotation)
    let cameraFirstColumn = Vector2(rotate (x1, y1) rotation)
    cameraOrigin + position, cameraFirstColumn + position
    // let (a, b) = (rx1 - rx, ry1 - ry)
let rotateVector (rotation: float32) (dir: Vector2) =
    Vector2 (rotate (dir.X, dir.Y) rotation)
    
type BoxFace = Right | Left | Up | Down
type float2f = { X: float32; Y: float32 }
let columnRelativePosition voxel pt =
    if MathF.Abs(pt.X - voxel.X) > MathF.Abs(pt.Y - voxel.Y) then
        if pt.X > voxel.X + 0.5f then Left, 1f - (pt.Y - voxel.Y)
        // else Right, pt.Y - voxel.Y
        else Right, 0f //1f - (pt.Y - voxel.Y)
    else
        if pt.Y > voxel.Y + 0.5f then Up, 0f //1f - (pt.X - voxel.X)
        else Up, 0f //1f - (pt.X - voxel.X)
        Up, pt.X - voxel.X |> MathF.Abs
        Up, voxel.X - pt.X
    if (pt.X - voxel.X) > (pt.Y - voxel.Y) then
        Up, voxel.X - pt.X
        Up, 0.5f
    else
        Up, voxel.X - pt.X
        Up, 0.1f
        // else Down, pt.X - voxel.X
        
let pixelsForColumn (imageData: ImageData) n size =
    let x = MathF.Round(MathF.Min(float32 imageData.width * n, float32 imageData.width)) |> int
    [|
        for i in 0..size - 1 do
            let y = int (System.Math.Round imageData.height * (float i / float size))
            for n in 0..3 do
                yield imageData.data[(((y * int imageData.width) + x) * 4) + n]
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
let skyboxColor = (0uy, 24uy, 50uy)
let mutable buffer = Unchecked.defaultof<JS.Uint8ClampedArray>
type Asset =
    Image of ImageData
type Game = { Assets: Map<string, Asset> }
let drawCamera (wallTextureData: ImageData) width height _level position rotation =
    if buffer = Unchecked.defaultof<_> then
        buffer <- JS.Constructors.Uint8ClampedArray.Create (width * height * 4)
    let (r, g, b) = skyboxColor
    for y in 0..height - 1 do
        for x in 0..width - 1 do
            let offset = (y * width + x) * 4
            buffer[offset] <- r
            buffer[offset + 1] <- g
            buffer[offset + 2] <- b
            buffer[offset + 3] <- 255uy
    // let level = Map.map (fun _ -> toRgb) level
    
    // let level = Map.map (fun _ -> toRgb) level
    // let buffer: byte[] = Array.zeroCreate (width * height * 4)
    let mutable index = 0
    let focalLength = 1f
    // let screenCasts = seq {
    //     let rayCount = width
    //     for i in -(rayCount / 2) + 1..rayCount / 2 do
    //         yield Vector2((float32 i / float32 rayCount), focalLength)
    // }
    let screenCasts = screenColumns width rotation position
    let (cameraPlaneOrigin, cameraPlaneFirstColumn) = cameraLine rotation position
    for ray in screenCasts do
        // let ray = ray * 1000f
        // console.log ("traversing ray ", ray)
        // let voxelsIntersected =
        //     findVoxelsAlongRay position ray
        //     |> Seq.takeWhile (fun ((x, y), _) ->
        //         Math.Abs(x) < 50 && Math.Abs(y) < 50)
        // let traversed =
        //     voxelsIntersected
        //     |> Seq.takeWhile (fun ((x, y), _) ->
        //         Math.Abs(x) < 50 && Math.Abs(y) < 50 &&
        //         not (Map.containsKey (x, y) map)) |> Seq.toArray
        // let intersection =
        //     voxelsIntersected
        //     |> Seq.tryPick (fun ((x, y), pt) ->
        //         Map.tryFind (x, y) level |> Option.map (fun color -> color, (x, y), pt))
        let intersection = findIntersection position ray level.ContainsKey |> Option.map (fun (voxel, pt) -> level[voxel], voxel, pt)
        // let intersection = Some ((255uy, 255uy, 255uy), (0, 0), (1.1, 1.1))
        match intersection with
        | Some ((r, g, b), (x, y), pt) ->
            // let (r, g, b) = toRgb color
            let distance = (Vector2(fst pt, snd pt) - (position)).Length()
            // let distance = distance * (1f - (Math.Abs(ray.X / 2f) / distance))
            // todo: Getting just the y component works
            // todo: the distance from the point to the plane
            // https://www.permadi.com/tutorial/raycast/rayc8.html
            let distance = (Vector2(0f, snd pt) - Vector2(0f, position.Y)).Length()
            let distance = 1f + (distanceFromLine cameraPlaneOrigin cameraPlaneFirstColumn (Vector2 pt))
            // let distance = MathF.Max(MathF.Min(1f, distance), distance)
            let columnHeight = int (float32 height / distance) * 2
            let yStart = (height - columnHeight) / 2
            let yEnd = yStart + columnHeight
            let _, n = columnRelativePosition { X = float32 x; Y = float32 y } { X = fst pt; Y = snd pt }
            // let pixels = pixelsForColumn wallTextureData n columnHeight
            for y in yStart..yEnd do
                let relativePositionVertical = float (y - yStart) / float columnHeight 
                let x = MathF.Round(MathF.Min(float32 wallTextureData.width * n, float32 wallTextureData.width)) |> int
                let yOffset = int (Math.Round(relativePositionVertical * wallTextureData.height))
                let textureDataOffset = (yOffset * int32 wallTextureData.width + x) * 4
                // [|
                //     for i in 0..size - 1 do
                //         let y = int (System.Math.Round imageData.height * (float i / float size))
                //         for n in 0..3 do
                //             yield imageData.data[(((y * int imageData.width) + x) * 4) + n]
                // |]
                let offset = 4 * (index + (y * width))
                buffer[offset] <- r
                buffer[offset + 1] <- g
                buffer[offset + 2] <- b
                buffer[offset + 3] <- 255uy
                
                let (r, g, b, a) = 
                    wallTextureData.data[textureDataOffset],
                    wallTextureData.data[textureDataOffset + 1],
                    wallTextureData.data[textureDataOffset + 2],
                    wallTextureData.data[textureDataOffset + 3]
                if (r, g, b) <> (255uy, 255uy, 255uy) then
                    buffer[offset] <- wallTextureData.data[textureDataOffset]
                    buffer[offset + 1] <- wallTextureData.data[textureDataOffset + 1]
                    buffer[offset + 2] <- wallTextureData.data[textureDataOffset + 2]
                    buffer[offset + 3] <- wallTextureData.data[textureDataOffset + 3]
            // console.log ("rendered column ", (fst pt), (snd pt), index, " rgb = ", r, g, b, "with height", columnHeight, "from distance", distance)
        | None ->
            // console.log (voxelsIntersected |> Array.ofSeq)
            ()
        index <- index + 1
        
    // for y in 0..height - 1 do
    //     for x in 0..width - 1 do
    //         if x % 4 = 0 then
                   // todo: always multiply y by width
    //             let offset = (x + (y * width)) * 4
    //             buffer[offset] <- 0uy
    //             buffer[offset + 1] <- 255uy
    //             buffer[offset + 2] <- 255uy
    //             buffer[offset + 3] <- 255uy // byte (Random().Next(0, 255))
    // console.log ("end of draw camera for", position)
    buffer
[<ReactComponent>]
let RaycastDemo2 () =
    let screen = Screen(880, 1000)
    let windowHeight = 480f
    let windowWidth = 640f
    let screenWidth = 0.5f
    let screenHeight = 0.375f
    let blockSize = 0.3f
    // todo: edge case in voxel traversal with cube at (-4, 4)
    // let playerPosition, setPlayerPosition = React.useStateWithUpdater(Vector2((-4.0000007450581f, -2.9802322387695312e-8f)))
    // todo: another edge case at (-5.000000022351742, -1.0000000447034836)
    
    // todo: screenWidth = 1f blockSize = 1f screenHeight = 0.375f
    // let windowHeight = 480f
    // let windowWidth = 640f
    // todo: -1, 7 also has errors (looks neat tho)
    // todo: -0, 7 also has errors second cube at (2, 3))
    // todo: 1, 7 also has errors
    // todo: (0.8999999985098839, -7) with cube at (8, 8) (1 right from 1, -7)
    let mutable playerRotationInRadians = 0f 
    let focalLength = 0.5f
    let playerPosition, setPlayerPosition =
        // React.useStateWithUpdater(Vector2(-4.2f, 3f))
        React.useStateWithUpdater(Vector2(2.1f, 0f))
    let screenCasts = seq {
        let rayCount = 320
        for i in -(rayCount / 2) + 1..rayCount / 2 do
            yield (Vector2((float32 i / float32 rayCount) * (screenWidth / 2f), focalLength))
            // yield (Vector2(float32 i / float32 rayCount * screenSize, 1f))
        // yield Vector2(float32 8 / float32 rayCount * screenSize, 1f)
    }
    let canvasRef = React.useRef null
    
    React.useEffectOnce <| fun () ->
        let canvas = document.createElement "canvas" :?> HTMLCanvasElement
        canvas.width <- 640
        canvas.height <- 480
        document.body.appendChild canvas |> ignore
        canvasRef.current <- canvas
    React.useEffectOnce <| fun () ->
        let element = document.createElement "img" :?> HTMLImageElement
        element.setAttribute("src", "./image.png")
        element.onload <- fun _ ->
            let canvas = document.createElement "canvas" :?> HTMLCanvasElement
            let size = 64
            canvas.width <- element.width
            canvas.height <- element.height
            let context2d = canvas.getContext_2d ()
            // Load wall texture
            context2d.drawImage(U3.Case1 element, 0, 0)
            let wallTextureData = context2d.getImageData(0, 0, size, size)
            
            let mutable playerPosition = Vector2(2f, 0f)
            let keysPressed = Dictionary<string, bool>()
            let isKeyPressed key =
                if keysPressed.ContainsKey key then keysPressed[key] else false
            window.onkeydown <- fun key ->
                // console.log key
                keysPressed[key.key.ToLower()] <- true
            window.onkeyup <- fun key ->
                keysPressed[key.key.ToLower()] <- false
            window.onblur <- fun ev ->
                for kv in keysPressed do
                    keysPressed[kv.Key] <- false
            let interval = 7f
            let mutable lastTime = 0.0
            let rec render (time: float) : unit =
            // window.setInterval ((fun () ->
                let speed = 2.8f
                // setInterval
                let deltaTime = interval / 1000f
                // requestAnimationFrame
                let deltaTime = float32 (time - lastTime) / 1000f
                console.log ("delta time = ", deltaTime * 1000f)
                lastTime <- time
                
            // Track start position in case new position collides with walls and we reset the player to the last position
                let originalPosition = playerPosition
                
                let mutable velocityDirection = Vector2(0f, 0f)
                if isKeyPressed "d" || isKeyPressed "ArrowRight" then
                    velocityDirection <- velocityDirection + Vector2(speed, 0f)
                if isKeyPressed "w" || isKeyPressed "ArrowUp" then
                    velocityDirection <- velocityDirection + Vector2(0f, speed)
                if isKeyPressed "a" || isKeyPressed "ArrowLeft" then
                    velocityDirection <- velocityDirection + Vector2(-speed, 0f)
                if isKeyPressed "s" || isKeyPressed "ArrowDown" then
                    velocityDirection <- velocityDirection + Vector2(0f, -speed)
                    
                
                if isKeyPressed "q" then
                    playerRotationInRadians <- playerRotationInRadians + (MathF.Tau * deltaTime * 0.2f)
                if isKeyPressed "e" then
                    playerRotationInRadians <- playerRotationInRadians - (MathF.Tau * deltaTime * 0.2f)
                    
                // velocityDirection <- velocityDirection / velocityDirection.Length()
                velocityDirection <- (rotateVector playerRotationInRadians velocityDirection).Normalized * speed * deltaTime
                
                playerPosition <- playerPosition + velocityDirection
                
                if mapData.ContainsKey (int (MathF.Floor playerPosition.X), int (MathF.Floor playerPosition.Y)) then
                    // playerPosition <- Vector2(0f, 0f)
                    playerPosition <- originalPosition
                let canvas = canvasRef.current
                let ctx = canvas.getContext_2d ()
                let img = drawCamera wallTextureData (int canvas.width) (int canvas.height) mapData playerPosition playerRotationInRadians
                let imgData = ImageData.Create (img :> obj :?> _, int canvas.width, int canvas.height)
                // console.log imgData
                ctx.putImageData (imgData, 0, 0)
                window.requestAnimationFrame render
                |> ignore
            window.requestAnimationFrame render
            // ), int interval)
            |> ignore
    Html.div [
        Html.div [
            Html.p $"{playerPosition}"
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(-0.1f, 0f)))
                prop.text "Left"
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(0.1f, 0f)))
                prop.text "Right"
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(0f, 0.1f)))
                prop.text "Forward"
            ]
            Html.button [
                prop.onClick (fun _ -> setPlayerPosition (fun p -> p + Vector2(0f, -0.1f)))
                prop.text "Back"
            ]
        ]
        // Svg.svg [
        //     svg.width screen.Width
        //     svg.height screen.Height
        //     svg.children [
        //         yield screen.line "black" -(int (windowWidth / 2f)) (int (windowHeight / 2f)) (int windowWidth / 2) (int (windowHeight / 2f))
        //         yield screen.line "black" -(int (windowWidth / 2f)) -(int (windowHeight / 2f)) (int (windowWidth / 2f)) -(int (windowHeight / 2f))
        //         yield screen.circle 5 (int (playerPosition.X * 40f)) (int (playerPosition.Y * 40f))
        //         yield! drawGrid screen
        //         for ray in screenCasts do
        //             // let ray = ray * 1000f
        //             let voxelsIntersected = findVoxelsAlongRay playerPosition ray
        //             let traversed = voxelsIntersected |> Seq.takeWhile (fun (key, _) -> not (Map.containsKey key map)) |> Seq.toArray
        //             let intersection =
        //                 voxelsIntersected
        //                 |> Seq.tryPick (fun ((x, y), pt) -> Map.tryFind (x, y) map |> Option.map (fun color -> color, (x, y), pt))
        //             // for (x, y) in traversed do
        //             //     screen.voxel 40 x y "yellow"
        //             // console.log traversed
        //             yield! drawMinimap screen
        //             match intersection with
        //             | Some (color, (voxelX, voxelY), (x, y)) ->
        //                 // let ray = ray * 400f
        //                 // screen.line color 0 0 (int ray.X) (int ray.Y)
        //                 // let distance = System.MathF.Sqrt((x - float32 voxelX) ** 2f + (y - float32 voxelY) ** 2f)
        //                 // let distance = Vector2((x - float32 voxelX), (y - float32 voxelY)).Length()
        //                 // let distance = Vector2(x, y).Length() * 40f
        //                 // let distance = (Vector2(x, y) - playerPosition + Vector2(0f, 0.7f)).Length() // System.MathF.Sqrt ((x * x) + (y * y))
        //                 let distance = (Vector2(x, y) - (playerPosition + ray)).Length()
        //                 
        //                 // let height = (1f / distance) * (blockSize / screenHeight) * (windowHeight / 4f) // * 0.5f
        //                 let height = (((blockSize / screenHeight) * windowHeight)
        //                               / distance)
        //                     
        //                 let height = MathF.Min(height, windowHeight)
        //                 // if color = "green" || color = "pink" then
        //                 //     console.log (color, (voxelX, voxelY), distance, (x, y), height)
        //                 yield screen.line color (int (playerPosition.X * 40f)) (int (playerPosition.Y * 40f)) (int (x * 40f)) (int (y * 40f))
        //                 let lineX = int (ray.X * windowWidth / screenWidth * 2f)
        //                 // let yOffset = int (height / 2f) - int (windowHeight / 2f)
        //                 // let yStart = int (windowHeight / 2f) - int (height / 2f)
        //                 let yStart = int (height / 2f)
        //                 // let yEnd = yStart + int height
        //                 let yEnd = -yStart
        //                 // console.log ((int (ray.X * 320f)), (320 - yOffset), (int (ray.X * 320f)), (-320 + yOffset))
        //                 // yield screen.line color (int (ray.X * 320f)) (120 - yOffset) (int (ray.X * 320f)) (-120 + yOffset)
        //                 // yield screen.line color lineX yStart lineX yEnd
        //                 yield screen.line color (lineX + 1) yStart (lineX + 1) yEnd
        //             | None ->
        //                 yield screen.line "grey" 0 0 (int ray.X) (int ray.Y)
        //     ]
        // ]
    ]
// try drawMap width height map with error -> console.log ("error = ", error)
let createGameRoot () =
    ReactDOM.createRoot (document.getElementById "root")
    |> fun root -> root.render(RaycastDemo2 ())
