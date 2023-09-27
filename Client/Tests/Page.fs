module Dootverse.Client.Tests.Page
open System
open Browser.Types
open Doot.Maths.Voxel.Traversal
open Dootverse.Client
open Dootverse.Client.Svg
open Fable.Core
// open Fable.Core.JS
open Feliz
open Browser
open PGA
open Dootverse
open Dootverse.Models
open Dootverse.Render


let (x, y) = (0., 10.)
let (x1, y1) = (10., 10.)

let theta = System.Math.Tau / 4.
// let theta = 0f
let RAPIER: RAPIER.IExports = JsInterop.importAll "@dimforge/rapier3d-compat"

type svg with
    static member cx x = svg.cx (int x)
    static member cy x = svg.cx (int x)
    
[<ReactComponent>]
let RotatePoint () =
    let screen = Screen(400, 400)
    let int = JS.Math.round >> int
    let (rx, ry) = Render.rotatePoint2d (x, y) theta
    let (rx1, ry1) = Render.rotatePoint2d (x1, y1) theta

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
        if Math.Abs(pt.X - voxel.X) > Math.Abs(pt.Y - voxel.Y) then
            if pt.X > voxel.X + 0.5 then Left, 1. - (pt.Y - voxel.Y)
            else Right, pt.Y - voxel.Y
        else
            if pt.Y > voxel.Y + 0.5 then Up, 1. - (pt.X - voxel.X)
            else Down, pt.X - voxel.X
            
    let pixelsForColumn (imageData: ImageData) n size =
        let x = JS.Math.round(Math.Min(imageData.width * n, imageData.width)) |> int
        [|
            for i in 0..size - 1 do
                let y = int (JS.Math.round imageData.height * (float i / float size))
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
                let scaledColumn = pixelsForColumn data (float i * 0.02) 32
                let columnData = ImageData.Create(JS.Constructors.Uint8ClampedArray.Create scaledColumn :> obj :?> _, 1, 32)
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
            for key in JS.Object.keys localStorage do
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
module RenderingTests =
    open type PGA3D
    [<ReactComponent>]
    let WorldToScreenCoordinates () =
        let screen = Screen(400, 400)
        let canvasHeight = 480f
        let canvasWidth = 640f
        let x, setX = React.useState -8
        let y, setY = React.useState 0
        let rotation, setRotation = React.useState (Math.Tau / 8.0)
        let pt = point(float32 x, 0f, float32 y)
        let initCameraOrigin = point(0f, 0f, 1f)
        let initCameraRight = point(1f, 0f, 1f)
        let r = rotor(float32 rotation, point(0f, 0f, 0f) &&& point(0f, 10f, 0f))
        let playerForward = r * direction(0f, 0f, 400f) * ~~~r
        let cameraOrigin = r * initCameraOrigin * ~~~r + pt.AsDirection
        let cameraRight = (r * initCameraRight * ~~~r) + pt.AsDirection
        let distance_ = pt + playerForward
        let x1, y1 = distance_.X * 10f, distance_.Z * 10f
        
        let cameraPlane =
            // let p = point(0f, canvasHeight / 2f, 10f) &&& point(10f, canvasHeight / 2f, 10f) &&& point(0f, canvasHeight / 2f + 10f, 10f)
            let p = cameraOrigin &&& cameraRight &&& translate(cameraOrigin, direction(0f, 1f, 0f))
            p
            // r * p * ~~~r
        let cameraEye = point(float32 x, 0.5f, float32 y)
        // let cameraPlane = point
        let entityPoint = point(2f, 1f, 8f)
        let ray = cameraEye &&& entityPoint
        let pointOnPlane = ray ^^^ cameraPlane
        let offset =
            // let r = rotor(float32 -rotation, point(0f, 0f, 0f) &&& point(0f, 10f, 0f))
            // let pointOnPlane = r * pointOnPlane * ~~~r
            let pointOnPlane = ~~~r * pointOnPlane * r
            pointOnPlane.normalized() - ((~~~r * cameraOrigin * r) + direction(0f, 0.5f, 0f)).AsDirection
        let distanceFromPlane = distance(entityPoint, cameraPlane)
        // let offset, distanceFromPlane = Render.worldCoordinatesToScreenCoordinates null { X =  }
            // distance = || a.norm v b.norm ||
            // entityPoint.normalized() &&& cameraPlane.normalized()
            // |> fun result -> result.norm()
            // pointOnPlane.normalized() - initCameraOrigin.AsDirection
        Html.div [
            Html.div [
                Html.input [
                    prop.type' "range"
                    prop.onChange setX
                    prop.max (screen.Width / 2)
                    prop.min (-screen.Width / 2)
                    prop.value x
                ]
                Html.input [
                    prop.type' "range"
                    prop.onChange setY
                    prop.max (screen.Height / 2)
                    prop.min (-screen.Height / 2)
                    prop.value y
                ]
                Html.input [
                    prop.type' "range"
                    prop.max Math.Tau
                    prop.min -Math.Tau
                    prop.value rotation
                    prop.step (Math.Tau / 32.0)
                    prop.onChange setRotation
                ]
                Html.span rotation
            ]
            Html.h4 ("e0 = " + string e0)
            Html.h4 ("pt = " + string pt)
            Html.h4 ("up line = " + string (point(0f, 1f, 0f) &&& point(0f, 0f, 0f)))
            Html.h4 ("camera plane = " + string cameraPlane)
            Html.h4 ("rotor = " + string r)
            Html.h4 ("forward direction = " + string playerForward)
            Html.h4 ("Camera origin = " + string cameraOrigin.Vector)
            Html.h4 ("Camera right = " + string cameraRight.Vector)
            Html.h4 ("Entity point = " + string entityPoint.Vector)
            Html.h4 ("Point on plane = " + string pointOnPlane.Vector)
            Html.h4 ("Point on plane = " + string pointOnPlane)
            Html.h4 ("Offset = " + string offset.Vector)
            Html.h4 ("Offset z = " + string (Math.Abs(offset.Z) < 0.00001f))
            Html.h4 (string cameraOrigin)
            Html.h4 ("entity distance from plane = " + string distanceFromPlane)
            // todo: Always have to normalize a point before adding a direction vector
            Html.h4 (string (cameraOrigin.AsDirection * (cameraOrigin + cameraOrigin) * ~~~cameraOrigin.AsDirection))
            Svg.svg [
                svg.width screen.Width
                svg.height screen.Height
                svg.children [
                    screen.circle ("pink", 2, x * 10, y * 10)
                    screen.circle ("black", 2, int (10f * pt.X), int (10f * pt.Z))
                    // screen.circle ("blue", 2, int (pt.normalized() + (cameraRight.AsDirection * 10f)).X, int (pt.normalized() + cameraRight.AsDirection * 10f).Z)
                    screen.circle ("blue", 2, int (cameraRight.X * 10f), int (cameraRight.Z * 10f))
                    screen.circle ("red", 2, int (entityPoint.X * 10f), int (entityPoint.Z * 10f))
                    screen.circle ("green", 2, int (pointOnPlane.X * 10f), int (pointOnPlane.Z * 10f))
                    // screen.line ("blue", int x, int y, int x1, int y1)
                    // screen.line "green" (int ) (int y) (int x1) (int y1)
                    screen.line "green" (int (pt.X * 10f)) (int (pt.Z * 10f)) (int x1) (int y1)
                ]
            ]
        ]
let runTestApp () =
    document.getElementById "root" |> ReactDOM.createRoot |> fun root -> root.render (App ())
open type PGA.PGA2D
let run () = promise {
    do! RAPIER.init ()
    let world = RAPIER.World.Create (RAPIER.Vector3.Create (0, -9.81, 0))
    let pt = !!!(e0 + 2f * e1 + 4f * e2)
    let dir = direction (4.2f, 1f)
    console.log pt
    console.log dir
    console.log (pt + dir)
    console.log world
    localStorage["files"] <- "yo!"
    // localStorage.clear()
    screenColumns 8 theta (Vector2(0., 0.))
    |> Seq.iter (printfn "%A")
    document.getElementById "root" |> ReactDOM.createRoot |> fun root -> root.render (RenderingTests.WorldToScreenCoordinates ())
}
// console.log ("distance (0, 2) from (0, 1) -> (1, 1) = ", distanceFromLine (Vector2(0f, 1f)) (Vector2(1f, 1f)) (Vector2(8f, 11f)))