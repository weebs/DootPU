module Dootverse.Render

open System
open System.Collections.Generic
open Browser
open Fable.Core
open Doot.Maths.Voxel.Traversal
open Doot.Maths
open Browser.Types
open Dootverse.Models

type BoxFace = Right | Left | Up | Down


// todo: size scaling
let traverseRay size (start: Vector2) (ray: Vector2) =
    findVoxelsAlongRay (start / float size) (ray / float size)

let drawMap width height (grid: Map<int * int, string>) =
    let arr = JS.Constructors.Uint8ClampedArray.Create (width * height * 4)
    let start = Vector2(0., 0.)
    console.log ("start = ")
    console.log start
    let dir = Vector2(1.0, 1.0)
    let rays = seq {
        for i in -320 / 2..-1 do
            yield (Vector2(0., 0.), Vector2(float i / 160., 1.0))
        for i in 1..320 / 2 do
            yield (Vector2(0., 0.), Vector2(float i / 160., 1.0))
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
// Source: https://academo.org/demos/rotation-about-point/
let rotate pt radians =
    let (x, y) = pt
    (x * Math.Cos radians - y * Math.Sin radians,
     y * Math.Cos radians + x * Math.Sin radians)
// let mutable screenWidth = 0
let mutable screenRaycastLines = Array.zeroCreate<float> 0
let mutable screenRaycasts = Array.zeroCreate<Vector2> 0
let getRaycastAtColumn width rotationRadians column =
    let (rx, ry) = rotate (0, 1) rotationRadians
    let (rx1, ry1) = rotate (1, 1) rotationRadians
    let (a, b) = (rx1 - rx, ry1 - ry)
    let magnitude = Math.Sqrt((a * a) + (b * b))
    let lineSlope = (a / magnitude, b / magnitude)
    let (run, rise) = lineSlope
    let percentI = float column / float width
    Vector2(rx + (run * percentI), ry + (rise * percentI))
let screenColumns width rotationRadians (offset: Vector2) =
    let x = 0.
    let y = 1.0
    let x1 = 1.0
    let y1 = 1.0
    let (rx, ry) = rotate (0, 1) rotationRadians
    let (rx1, ry1) = rotate (1, 1) rotationRadians
    let (a, b) = (rx1 - rx, ry1 - ry)
    // console.log ("theta = ", rotationRadians)
    // console.log ("a, b = ", (a, b))
    let magnitude = Math.Sqrt((a * a) + (b * b))
    let lineSlope = (a / magnitude, b / magnitude)
    let (run, rise) = lineSlope
    // console.log ("line slope = ", lineSlope)
    // [|
    if screenRaycastLines.Length <> width * 2 then
        screenRaycastLines <- Array.zeroCreate (width * 2)
        screenRaycasts <- Array.zeroCreate width
    for i in -width / 2..width / 2 do
        let index = (i + (width / 2)) * 2
        let percentI = float i / (float width)
        // Vector2((rx + (run * float i)) + offset.X, (ry + (rise * float i)) + offset.Y)
        screenRaycastLines[index] <- rx + (run * float percentI)
        screenRaycastLines[index + 1] <- ry + (rise * float percentI)
        screenRaycasts[index / 2] <- Vector2(screenRaycastLines[index], screenRaycastLines[index + 1])
    screenRaycastLines
    screenRaycasts
    // seq {
    //     for i in -width / 2..width / 2 do
    //         let i = float i / (float width)
    //         // Vector2((rx + (run * float i)) + offset.X, (ry + (rise * float i)) + offset.Y)
    //         Vector2((rx + (run * float i)), (ry + (rise * float i)))
    // }
    // |]
let distanceFromLine (lineA: Vector2) (lineB: Vector2) (pt: Vector2) =
    let numerator = Math.Abs(((lineB.X - lineA.X) * (lineA.Y - pt.Y)) - ((lineA.X - pt.X) * (lineB.Y - lineA.Y)))
    let denom = Math.Sqrt((lineB.X - lineA.X) ** 2.0 + (lineB.Y - lineA.Y) ** 2.0)
    numerator / denom
let cameraLine rotation position =
    let x = 0.
    let y = 1.0
    let x1 = 1.0
    let y1 = 1.0
    let cameraOrigin = Vector2 (rotate (x, y) rotation)
    let cameraFirstColumn = Vector2(rotate (x1, y1) rotation)
    cameraOrigin + position, cameraFirstColumn + position
    // let (a, b) = (rx1 - rx, ry1 - ry)
let rotateVector (rotation: float) (dir: Vector2) =
    Vector2 (rotate (dir.X, dir.Y) rotation)
    
let columnRelativePosition voxel pt : _ * float =
    // if Math.Abs(pt.X - voxel.X) > Math.Abs(pt.Y - voxel.Y) then
    //     if pt.X > voxel.X + 0.5 then Left, 1.0 - (pt.Y - voxel.Y)
    //     // else Right, pt.Y - voxel.Y
    //     else Right, 0. //1.0 - (pt.Y - voxel.Y)
    // else
    //     if pt.Y > voxel.Y + 0.5 then Up, 0. //1.0 - (pt.X - voxel.X)
    //     else Up, 0. //1.0 - (pt.X - voxel.X)
    //     Up, pt.X - voxel.X |> Math.Abs
    //     Up, voxel.X - pt.X
    if pt.Y = voxel.Y && pt.X > voxel.X then
        Down, pt.X - voxel.X
    elif pt.X = voxel.X && pt.Y > voxel.Y then
        Left, pt.Y - voxel.Y
    elif pt.X - 1.0 = voxel.X && pt.Y > voxel.Y then
        // Right, pt.Y - voxel.Y
        // Flips texture
        Right, voxel.Y - pt.Y
    elif pt.Y - 1.0 = voxel.Y && pt.X > voxel.X then
        // Up, pt.X - voxel.X
        // Flips texture
        Up, voxel.X - pt.X
    else
        Up, 0
        
    // if (pt.X - voxel.X) > (pt.Y - voxel.Y) then
    //     Up, voxel.X - pt.X
    //     Up, 0.5
    // else
    //     Up, voxel.X - pt.X
    //     Up, 0.1
    // if
        // else Down, pt.X - voxel.X
// todo: Unused ?        
let pixelsForColumn (imageData: ImageData) n size =
    let x = JS.Math.round(Math.Min(float imageData.width * n, float imageData.width)) |> int
    [|
        for i in 0..size - 1 do
            let y = int (JS.Math.round imageData.height * (float i / float size))
            for n in 0..3 do
                yield imageData.data[(((y * int imageData.width) + x) * 4) + n]
    |]
    
let mutable buffer = Unchecked.defaultof<JS.Uint8ClampedArray>
let skyboxColor = (0uy, 24uy, 50uy)

let drawCamera (wallTextureData: ImageData) width height (level: Dictionary<_,_>) position rotation =
    if buffer = Unchecked.defaultof<_> then
        buffer <- JS.Constructors.Uint8ClampedArray.Create (width * height * 4)
    // Draw floor and ceiling
    let (r, g, b) = skyboxColor
    for y in 0..height - 1 do
        for x in 0..width - 1 do
            let offset = (y * width + x) * 4
            buffer[offset] <- r
            buffer[offset + 1] <- g
            buffer[offset + 2] <- b
            buffer[offset + 3] <- 255uy
            
    let mutable index = 0
    let focalLength = 1.0
    
    let screenCasts = screenColumns width rotation position
    let (cameraPlaneOrigin, cameraPlaneFirstColumn) = cameraLine rotation position
    for ray in screenCasts do
        // let ray = ray * 1000.
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
        let raycastResult =
            findIntersection position ray level.ContainsKey
            |> Option.map (fun (voxel, pt) -> level[voxel], voxel, pt)
        // let intersection = Some ((255uy, 255uy, 255uy), (0, 0), (1.1, 1.1))
        match raycastResult with
        | Some ((r, g, b), (x, y), pt) ->
            // let distance = distance * (1.0 - (Math.Abs(ray.X / 2.0) / distance))
            // https://www.permadi.com/tutorial/raycast/rayc8.html
            let distance = focalLength + (distanceFromLine cameraPlaneOrigin cameraPlaneFirstColumn (Vector2 pt))
            // let distance = Math.Max(Math.Min(1.0, distance), distance)
            let columnHeight = int (float height / distance) * 2
            let yStart = (height - columnHeight) / 2
            let yEnd = yStart + columnHeight
            let _, n = columnRelativePosition { X = float x; Y = float y } { X = fst pt; Y = snd pt }
            // let pixels = pixelsForColumn wallTextureData n columnHeight
            for y in yStart..yEnd do
                let relativePositionVertical = float (y - yStart) / float columnHeight 
                let x =
                    JS.Math.round(Math.Min
                        (wallTextureData.width * float n,
                         wallTextureData.width))
                    |> int
                let yOffset = int (JS.Math.round(relativePositionVertical * wallTextureData.height))
                let textureDataOffset = (yOffset * int32 wallTextureData.width + x) * 4
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
                    buffer[offset] <- r
                    buffer[offset + 1] <- g
                    buffer[offset + 2] <- b
                    buffer[offset + 3] <- a
        | None ->
            ()
        index <- index + 1
        
    buffer
