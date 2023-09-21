module Dootverse.Client.Raycaster_Demo

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
    findVoxelsAlongRay (start / float size) (ray / float size)

let drawMap width height (grid: Map<int * int, string>) =
    let arr = JS.Constructors.Uint8ClampedArray.Create (width * height * 4)
    let start = Vector2(0., 0.)
    console.log ("start = ")
    console.log start
    let dir = Vector2(1., 1.)
    let rays = seq {
        for i in -320 / 2..-1 do
            yield (Vector2(0., 0.), Vector2(float i / 160., 1.))
        for i in 1..320 / 2 do
            yield (Vector2(0., 0.), Vector2(float i / 160., 1.))
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
[<ReactComponent>]
let RaycastDemo () =
    let map, setMap = React.useState(mapData)
    let canvasRef = React.useRef None
    let canvas, setCanvas = React.useState null
    console.log ("canvas = ", box canvas)
    let arr = JS.Constructors.Uint8ClampedArray.Create (width * height * 4)
    let start = Vector2(0., 0.)
    console.log ("start = ")
    console.log start
    let dir = Vector2(1., 1.)
    let rays = seq {
        // for i in -320 / 2..-1 do
        //     yield (Vector2(0., 0.), Vector2(float i / 160., 1.))
        // for i in 1..320 / 2 do
        //     yield (Vector2(0., 0.), Vector2(float i / 160., 1.))
        // yield (Vector2(0., 0.), Vector2(0.1., 0.2f))
        yield (Vector2(0., 0.), Vector2(0.22, 1.))
        // yield (Vector2(0., 0.), Vector2(0.4f, 1.))
        // yield (Vector2(0., 0.), Vector2(0.888f, 1.))
        // yield (Vector2(0., 0.), Vector2(-1., 1.))
    }
    
    // drawMap width height map
    let toSvgPoint (x: int) (y: int) =
        // (((width / 2) + x) * width), (((height / 2) - y) * height)
        ((width / 2) + x) * width, ((height / 2) - y) * height
    Html.div [
        Svg.svg [
            svg.width ((width * 20) + 40)
            svg.height ((height * 20) + 40)
            svg.children [
                // Trace the raycast tiles path
                for (start, ray) in rays do
                    let coordinates = Voxel.Traversal.traverseRay 1 start ray
                    let steps =
                        coordinates
                        |> Seq.skip 1
                        |> Seq.takeWhile (fun (x, y) -> x <= width && y <= height)
                        |> Seq.toArray
                    for step in steps do
                        let (x, y) = toSvgPoint (fst step) (snd step)
                        Svg.rect [
                            svg.x (x - 10)
                            svg.y (y + 10)
                            svg.width 20
                            svg.height 20
                            // svg.width 20
                            // svg.height 20
                            svg.fill "grey"
                        ]
                for (start, ray) in rays do
                    let coordinates = Voxel.Traversal.traverseRay 1 start ray
                    let steps =
                        coordinates
                        |> Seq.takeWhile (fun (x, y) -> x <= width && y <= height)
                        |> Seq.toArray
                    console.log ("steps = ", steps)
                    steps
                    |> Array.find map.ContainsKey
                    // Draw the raycast line and its color
                    |> fun (x, y) -> Svg.line [
                        svg.stroke (map[(x, y)])
                        let (x, y) = toSvgPoint x y
                        let ray: Vector2 = ray * 40.
                        let (x, y) = toSvgPoint (int ray.X) (int ray.Y)
                        let cx, cy = toSvgPoint 0 0
                        svg.x1 cx
                        svg.y1 cy
                        svg.x2 x
                        svg.y2 y
                        svg.strokeWidth 0.2
                    ]
                for x in -10..10 do
                    let x, _ = toSvgPoint x 0
                    Svg.line [
                        svg.x1 (x - 10)
                        svg.x2 (x - 10)
                        svg.y1 0
                        svg.y2 (height * 20)
                        svg.stroke "brown"
                        svg.strokeWidth 0.2
                    ]
                for y in -10..10 do
                    let _, y = toSvgPoint 0 y
                    Svg.line [
                        svg.x1 0
                        svg.x2 (width * 20)
                        svg.y1 (y + 10)
                        svg.y2 (y + 10)
                        svg.stroke "brown"
                        svg.strokeWidth 0.2
                    ]
                // Draw the map
                for kv in map do
                    let (x, y) = toSvgPoint (fst kv.Key) (snd kv.Key)
                    Svg.rect [
                        svg.width 20
                        svg.height 20
                        svg.fill kv.Value
                        svg.x (x - 10) // (x - 10) //(((width / 2) + (fst kv.Key)) * width)
                        svg.y (y - 10) //(y - 10) //(((height / 2) - (snd kv.Key)) * height)
                    ]
                    Svg.circle [
                        svg.r 5
                        svg.fill "black"
                        svg.cx x // (x - 10) //(((width / 2) + (fst kv.Key)) * width)
                        svg.cy y //(y - 10) //(((height / 2) - (snd kv.Key)) * height)
                    ]
                for x in -10..10 do
                    for y in -10..10 do
                        let display = $"{x}, {y}"
                        let x, y = toSvgPoint x y
                        Svg.text [
                            svg.x x
                            svg.y y // (y + 10)
                            svg.text display
                            svg.fontSize 4
                        ]
                        Svg.rect [
                            svg.cx x
                            svg.cy y
                            svg.width 2
                        ]
                Svg.circle [
                    svg.cx 200
                    svg.cy 200
                    svg.r 10
                ]
                    
                // Svg.line [
                //     svg.x1 100
                //     svg.y1 100
                //     svg.x2 200
                //     svg.y2 200
                //     svg.stroke "blue"
                // ]
                // Svg.rect [
                //     svg.width 10
                //     svg.height 10
                //     svg.x 100
                //     svg.y 100
                //     svg.stroke "green"
                // ]
                // Svg.circle [
                //     svg.cx 208
                //     svg.cy 208
                //     svg.r 8
                //     // svg.x 200
                //     // svg.y 200
                //     svg.stroke "green"
                // ]
            ]
        ]
        Html.canvas [
            prop.ref setCanvas
        ]
    ]
let raycast () = ()
// type Screen = {
    // voxel: int -> int -> int -> string -> ReactElement
    // width: int
// } with
    // member this.voxel size x y color = Svg.rect []