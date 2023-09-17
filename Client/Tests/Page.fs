module Dootverse.Client.Tests.Page
open System
open Browser.Types
open Doot.Maths.Voxel.Traversal
open Dootverse.Client
open Dootverse.Client.Svg
open Fable.Core
open Fable.Core.JS
open Feliz
open Browser
open Raycaster_Demo
open Dootverse.Game


let (x, y) = (0f, 10f)
let (x1, y1) = (10f, 10f)

let theta = MathF.Tau / 4f
// let theta = 0f


type svg with
    static member cx x = svg.cx (int x)
    static member cy x = svg.cx (int x)
    
[<ReactComponent>]
let RotatePoint () =
    let screen = Screen(400, 400)
    let int = MathF.Round >> int
    let (rx, ry) = rotate (x, y) theta
    let (rx1, ry1) = rotate (x1, y1) theta

    console.log (x, y)
    console.log (x1, y1)
    console.log (rx, ry)
    console.log (rx1, ry1)
    Html.div [
        Html.h4 "Rotate Point"
        Svg.svg [
            svg.width screen.Width
            svg.height screen.Height
            svg.children [
                screen.circle ("green", 2, (int x), (int y))
                screen.circle ("blue", 2, (int x1), (int y1))
                screen.circle ("green", 2, (int rx), (int ry))
                screen.circle ("blue", 2, (int rx1), (int ry1))
                screen.line "grey" -200 0 200 0
                screen.line "grey" 0 -200 0 200
            ]
        ]
    ]

[<ReactComponent>]
let SpriteScaling () =
    let columnRelativePosition voxel pt =
        if MathF.Abs(pt.X - voxel.X) > MathF.Abs(pt.Y - voxel.Y) then
            if pt.X > voxel.X + 0.5f then Left, 1f - (pt.Y - voxel.Y)
            else Right, pt.Y - voxel.Y
        else
            if pt.Y > voxel.Y + 0.5f then Up, 1f - (pt.X - voxel.X)
            else Down, pt.X - voxel.X
            
    let pixelsForColumn (imageData: ImageData) n size =
        let x = MathF.Round(MathF.Min(float32 imageData.width * n, float32 imageData.width)) |> int
        [|
            for i in 0..size - 1 do
                let y = int (System.Math.Round imageData.height * (float i / float size))
                for n in 0..3 do
                    yield imageData.data[(((y * int imageData.width) + x) * 4) + n]
        |]
            
    let size = 64
    let canvasRef = React.useRef<HTMLCanvasElement option> None
    let scaledCanvasRef = React.useRef<HTMLCanvasElement option> None
    // let data = context2d.getImageData(0, 0, canvas.width, canvas.height)
    // console.log data
    React.useEffectOnce <| fun () ->
        let element = document.createElement "img" :?> HTMLImageElement
        element.setAttribute("src", "./image.png")
        element.onload <- fun _ ->
            let canvas = document.createElement "canvas" :?> HTMLCanvasElement
            canvas.width <- size
            canvas.height <- size
            let context2d = canvas.getContext_2d ()
            context2d.drawImage(U3.Case1 element, 0, 0)
            // console.log canvas.width
            let data = context2d.getImageData(0, 0, size, size)
            console.log data
            
            canvasRef.current.Value.getContext_2d().putImageData(data, 0, 0)
            let context2d =  scaledCanvasRef.current.Value.getContext_2d()
            // context2d.clearRect(0, 0, 400, 400)
            for i in 1..32 do
                let scaledColumn = pixelsForColumn data (float32 i * 0.02f) 32
                let columnData = ImageData.Create(Constructors.Uint8ClampedArray.Create scaledColumn :> obj :?> _, 1, 32)
                console.log columnData
                context2d.putImageData(columnData, i, 0)
            // canvasRef.current.Value.getContext_2d().drawImage(U3.Case1 element, 100, 100)
        // ()
    Html.div [
        Html.h4 "Sprite scaling"
        Html.canvas [
            prop.width size
            prop.height size
            prop.ref canvasRef
        ]
        Html.h5 "Scaled image"
        Html.canvas [
            prop.width 400
            prop.height 100
            prop.ref scaledCanvasRef
        ]
        // Html.img [
        //     prop.src "./image.png"
        // ]
    ]
    
[<ReactComponent>]
let Header () =
    Html.div [
        Html.h3 $"Files = {localStorage.length}"
        Html.ul [
            for key in Object.keys localStorage do
                console.log (localStorage.Item key)
                Html.li (localStorage.Item key)
        ]
    ]
    
[<ReactComponent>]
let App () =
    Html.div [
        Header ()
        SpriteScaling ()
    ]
    
let run () =
    localStorage["files"] <- "yo!"
    // localStorage.clear()
    screenColumns 8 theta (Vector2(0f, 0f)) |> Seq.iter (printfn "%A")
    document.getElementById "root" |> ReactDOM.createRoot |> fun root -> root.render (App ())
// console.log ("distance (0, 2) from (0, 1) -> (1, 1) = ", distanceFromLine (Vector2(0f, 1f)) (Vector2(1f, 1f)) (Vector2(8f, 11f)))