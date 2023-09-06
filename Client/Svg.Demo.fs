module Dootverse.Client.Svg_Demo

open System
open Doot.Maths.Voxel.Traversal
open Feliz
open Fable.Core
open Browser

let screenWidth = 800
let screenHeight = 800

let blockSize = 40
type Screen(width, height) =
    let toSvgCoordinate x y =
        ((width / 2) + x), ((height / 2) - y)
    member _.block size x y =
        let (x, y) = toSvgCoordinate x y
        Svg.rect [
            svg.x x
            svg.y (y - size) // todo: y is off by +1 when (+x, -y) direction
            svg.width size
            svg.height size
            svg.fill "yellow"
            svg.stroke "black"
        ]
    member _.circle (radius: int) x y =
        let (x, y) = toSvgCoordinate x y
        Svg.circle [
            svg.cx x
            svg.cy y
            svg.r radius
            svg.fill "green"
        ]
    member _.line color (x1: int) (y1: int) (x2: int) (y2: int) =
        let x1, y1 = toSvgCoordinate x1 y1
        let x2, y2 = toSvgCoordinate x2 y2
        Svg.line [
            svg.x1 x1
            svg.x2 x2
            svg.y1 y1
            svg.y2 y2
            svg.stroke color
        ]
let drawGrid (screen: Screen) =
    [|
    // screen.circle 10 0 0
        for x in -screenWidth / blockSize / 2..screenWidth / blockSize / 2 do
            for y in (-screenHeight / blockSize / 2)..(screenHeight / blockSize / 2) do
                screen.circle 4 (x * blockSize) (y * blockSize)
        Svg.line [
            svg.x1 0
            svg.x2 screenWidth
            svg.y1 0
            svg.y2 0
            svg.stroke "black"
        ]
        Svg.line [
            svg.x1 0
            svg.x2 screenWidth
            svg.y1 screenHeight
            svg.y2 screenHeight
            svg.stroke "black"
        ]
        Svg.line [
            svg.x1 0
            svg.x2 screenWidth
            svg.y1 (screenHeight / 2)
            svg.y2 (screenHeight / 2)
            svg.stroke "black"
        ]
        Svg.line [
            svg.x1 (screenWidth / 2)
            svg.x2 (screenWidth / 2)
            svg.y1 0
            svg.y2 screenHeight
            svg.stroke "black"
        ]
    |]
let drawShapes () = [
    Svg.circle [
        svg.cx 0
        svg.cy 0
        svg.r 20
        svg.fill "green"
    ]
    Svg.rect [
        svg.x 0
        svg.y 0
        svg.width 40
        svg.height 40
        svg.fill "blue"
    ]
    Svg.rect [
        svg.x 0
        svg.y (screenHeight - 40)
        svg.width 40
        svg.height 40
        svg.fill "blue"
    ]
]
let lines = [
    Vector2(-10f, -10f), Vector2(1f, 1.001f)
    // Vector2(1f, 1.8f), Vector2(-1.2f, -1f)
    
    // Vector2(0f, 0f), Vector2(0.5f, 1f)
    // Vector2(0f, 0.5f), Vector2(0.5f, 1f)
    // Vector2(0.5f, 0f), Vector2(0.5f, 0.5f)
    // Vector2(0.2f, 0f), Vector2(-0.2f, 0.5f)
]
let drawRay lineStart lineDir (screen: Screen) =
    // let items = traverseRay 1 lineStart lineDir |> Seq.takeWhile (fun (x, y) -> Math.Abs(x) < 11 && Math.Abs(y) < 11) |> Array.ofSeq
    let items = findVoxelsAlongRay lineStart lineDir |> Seq.takeWhile (fun (x, y) -> Math.Abs(x) < 11 && Math.Abs(y) < 11) |> Array.ofSeq
    console.log items
    [|
        for (x, y) in items do
            screen.block blockSize (x * blockSize) (y * blockSize)
        screen.line "pink" (int (lineStart.X * 40f)) (int (lineStart.Y * 40f)) (int (lineStart.X + (lineDir.X * 800000f))) (int (lineStart.Y + (lineDir.Y * 800000f)))
    |]
[<ReactComponent>]
let SvgDemo () =
    let screen = Screen(screenWidth, screenHeight)
    let px, setPx = React.useState(0)
    let py, setPy = React.useState(0)
    
    // let items = traverseRay 1 lineStart lineDir |> Seq.takeWhile (fun (x, y) -> Math.Abs(x) < 11 && Math.Abs(y) < 11) |> Array.ofSeq
    // console.log items
    Html.div [
        Svg.svg [
            svg.width screenWidth
            svg.height screenHeight
            svg.children [
                screen.circle 2 px py
                for (start, dir) in lines do
                    yield! drawGrid screen
                    yield! drawRay start dir screen
                
                // for (x, y) in items do
                //     screen.block blockSize (x * blockSize) (y * blockSize)
                // screen.line "pink" (int (lineStart.X * 40f)) (int (lineStart.Y * 40f)) (int (lineStart.X + (lineDir.X * 800000f))) (int (lineStart.Y + (lineDir.Y * 800000f)))
                
                // drawShapes ()
            ]
        ]
        Html.div [
            Html.button [
                prop.text $"Inc x = {px}"
                prop.onClick (fun _ -> setPx (px + 1))
            ]
        ]
        Html.div [
            Html.h4 "Notes"
            Html.ul [
                Html.li "Circles have their center point at (cx, cy)"
                Html.li "Rectangles have their top left point at (x, y)"
                Html.li "Rectangles have their bottom left point at (x, y + height) or cartesian (x, y - height / 2)"
            ]
        ]
    ]
for (u, dir) in lines do
    console.log ("line from ", u, "with vector", dir)
    console.log (findVoxelsAlongRay u dir |> Seq.take 5 |> Seq.toArray)
ReactDOM.createRoot (document.getElementById "root")
|> fun root -> root.render(SvgDemo ())
