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

open type PGA.PGA3D

// Source: https://academo.org/demos/rotation-about-point/
let rotatePoint2d pt radians =
    let (x, y) = pt
    (x * Math.Cos radians - y * Math.Sin radians,
     y * Math.Cos radians + x * Math.Sin radians)
// todo: size scaling
let traverseRay size (start: Vector2) (ray: Vector2) =
    findVoxelsAlongRay (start / float size) (ray / float size)
let cartesianToScreen screenWidth screenHeight x y =
    screenWidth / 2 + x, screenHeight / 2 - y
//// Returns the offset (in cartesian coordinates) from the center of the camera from projecting
/// a world coordinate onto the camera plane
let worldCoordinatesToScreenCoordinates screen (cameraPosition: float2f) cameraRotation (position: float3f) =
    // todo: Opposite rotation signs for GA and rotatePoint2d
    let cameraRotation = cameraRotation
    // Player position and rotation
    // Y = 0.5f is that the camera sits at the midpoint of wall heights
    let pt = point(float32 cameraPosition.X, 0.5f, float32 cameraPosition.Y)
    let r = rotor(cameraRotation, point(0f, 1f, 0f) &&& point(0f, 0f, 0f))
    
    let initCameraOrigin = point(0f, 0f, 1f)
    let initCameraRight = point(1f, 0f, 1f)
    // Rotated camera plane
    let cameraOrigin = translate(r * initCameraOrigin * ~~~r, pt.AsDirection)
    let cameraRight = translate(r * initCameraRight * ~~~r, pt.AsDirection)
    let cameraPlane =
        cameraOrigin &&& cameraRight &&&
        translate(cameraOrigin, direction(0f, 1f, 0f))
    console.log ("camera origin = ", cameraOrigin.Vector)
        
    let cameraEye = point(float32 cameraPosition.X, 0.5f, float32 cameraPosition.Y)
    let worldPosition = point(float32 position.X, float32 position.Y, float32 position.Z)
    // Ray from the camera eye to the world position
    let ray = cameraEye &&& worldPosition
    let pointOnPlane = ray ^^^ cameraPlane
    // The relative offset between the camera plane's center and the pixel intersected with the
    // line between the camera eye and the position
    console.log ("point on plane = ", pointOnPlane.Vector)
    let offset =
        let pointOnPlane = ~~~r * (pointOnPlane.normalized() - pt.AsDirection) * r
        console.log ("point on initial plane = ", pointOnPlane.Vector)
        // todo: Check
        // todo: X shouldn't be -0.5f
        let initTopLeft = translate(initCameraOrigin, direction(-0.5f, 0.5f, 0f))
        console.log ("initTopLeft =", initTopLeft.Vector)
        // let cameraTopLeft = rotate(initTopLeft, r)
        // pointOnPlane.normalized() - ((~~~r * cameraOrigin * r) + direction(0f, 0.0f, 0f)).AsDirection
        pointOnPlane.normalized() - initCameraOrigin.AsDirection
    let distanceFromPlane = distance(worldPosition, cameraPlane)
    let rotatedPosition = rotate(worldPosition, rotor(float32 Math.Tau - cameraRotation, point(0f, 1f, 0f) &&& point(0f, 0f, 0f)))
    console.log ("rotatedPosition =", rotatedPosition.Vector)
    // if rotatedPosition.Z < cameraOrigin.Z then
        // offset, -distanceFromPlane
    // else
    offset, distanceFromPlane

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
// let mutable screenWidth = 0
let mutable screenRaycastLines = Array.zeroCreate<float> 0
let mutable screenRaycasts = Array.zeroCreate<Vector2> 0
let getRaycastAtColumn width rotationRadians column =
    let (rx, ry) = rotatePoint2d (0, 1) rotationRadians
    let (rx1, ry1) = rotatePoint2d (1, 1) rotationRadians
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
    let (rx, ry) = rotatePoint2d (0, 1) rotationRadians
    let (rx1, ry1) = rotatePoint2d (1, 1) rotationRadians
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
/// Produces a line perpindicular to the forward direction of the position+rotation (offset by 1 unit)
let cameraLine rotation position =
    let x = 0.
    let y = 1.0
    let x1 = 1.0
    let y1 = 1.0
    let cameraOrigin = Vector2 (rotatePoint2d (x, y) rotation)
    let cameraFirstColumn = Vector2(rotatePoint2d (x1, y1) rotation)
    cameraOrigin + position, cameraFirstColumn + position
    // let (a, b) = (rx1 - rx, ry1 - ry)
let rotateVector (rotation: float) (dir: Vector2) =
    Vector2 (rotatePoint2d (dir.X, dir.Y) rotation)
    
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
let scaleImage (scale: int) (img: ImageData) : ImageData =
    let buffer = ImageData.Create (img.width * float scale, img.height * float scale)
    for y in 0..int buffer.height - 1 do
        for x in 0..int buffer.width - 1 do
            let imageOffset = (y / scale * int img.width + x / scale) * 4
            let offset = (y * int buffer.width + x) * 4
            for i in 1..4 do
                buffer.data[offset + i - 1] <- img.data[imageOffset + i - 1]
            // for yi in 0..scale - 1 do
            //     for xi in 0..scale - 1 do
            //         let offset = ((y + yi) * int buffer.width + (x + xi)) * 4
            //         for i in 1..4 do
            //             buffer.data[offset + i - 1] <- img.data[imageOffset + i - 1]
    buffer
// todo: what about when the image is out of bounds?
let imgRgba (img: ImageData) offset =
    img.data[offset],
    img.data[offset + 1],
    img.data[offset + 2],
    img.data[offset + 3]
let writeRgbToImage (img: ImageData) offset (r, g, b) =
    img.data[offset] <- r
    img.data[offset + 1] <- g
    img.data[offset + 2] <- b
let writeRgbaToImage (img: ImageData) offset (r, g, b, a) =
    img.data[offset] <- r
    img.data[offset + 1] <- g
    img.data[offset + 2] <- b
    img.data[offset + 3] <- a
let drawRectangle (x: int) (y: int) (width: int) (height: int) (color: int * int -> byte * byte * byte * byte) (buffer: ImageData) =
    for y in y..y + height - 1 do
        if y < int buffer.height && y > 0 then
            for x in x..x + width - 1 do
                if x < int buffer.width && x > 0 then
                    let color = color (x, y)
                    let offset = (y * int buffer.width + x) * 4
                    writeRgbaToImage buffer offset color
let drawScaledSprite (img: ImageData) (position: int * int) (scale: float) (buffer: ImageData) =
    let imgWidth = JS.Math.round(scale * img.width) |> int
    let imgHeight = JS.Math.round(scale * img.height) |> int
    let (pX, pY) = position
    for y in pY..pY + imgHeight - 1 do
        if y < int buffer.height && y > 0 then
            for x in pX..pX + imgWidth - 1 do
                if x < int buffer.width && x > 0 then
                    let nearestY = (float (y - pY) / float imgHeight) * img.height |> JS.Math.round |> int
                    let nearestX = (float (x - pX) / float imgWidth) * img.width |> JS.Math.round |> int
                    let offset = (nearestY * int img.width + nearestX) * 4
                    let bufferOffset = (y * int buffer.width + x) * 4
                    let (r, g, b, a) = imgRgba img offset
                    if a <> 0uy then
                        writeRgbaToImage buffer bufferOffset (imgRgba img offset)
                        // for i in 0..3 do
                        //     buffer.data[bufferOffset + i] <- img.data[offset + i]
// todo: what about when the image is out of bounds?
let drawImage (img: ImageData) (position: float2f) (buffer: ImageData) =
    let position = {
        X = if position.X < 0 then buffer.width + position.X else position.X
        Y = if position.Y < 0 then buffer.height + position.Y else position.Y
    }
    for y in 0..int img.height - 1 do
        for x in 0..int img.width - 1 do
            if x + int position.X < int buffer.width then
                let offset = (y * int img.width + x) * 4
                let bufferOffset = ((y + int position.Y) * int buffer.width + (x + int position.X)) * 4
                let (r, g, b, a) = imgRgba img offset
                if (r, g, b, a) <> (255uy, 255uy, 255uy, 0uy) then // && a <> 0uy then
                    for i in 1..4 do
                        buffer.data[bufferOffset + i - 1] <- img.data[offset + i - 1]
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
                
                let (r, g, b, a) = imgRgba wallTextureData textureDataOffset
                if (r, g, b) <> (255uy, 255uy, 255uy) then
                    buffer[offset] <- r
                    buffer[offset + 1] <- g
                    buffer[offset + 2] <- b
                    buffer[offset + 3] <- a
        | None ->
            ()
        index <- index + 1
        
    buffer
