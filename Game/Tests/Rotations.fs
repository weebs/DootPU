module Rotations

open System
open Browser
open Feliz
open PGA
open type PGA3D

console.log DateTime.Now
type Coordinates(width: float32, height: float32) =
    let translatePoint (p: PGA3D) =
        point(p.X + (width / 2f), p.Y, -p.Z + (height / 2f))
    member _.drawPoint (p: PGA3D) =
        let p = translatePoint p
        Svg.circle [
            svg.cx (int p.X)
            svg.cy (int p.Z)
            svg.stroke "blue"
            svg.fill "green"
            svg.r 5
        ]
    member this.drawPoint (p: PGA3D, title: string) =
        let p' = translatePoint p
        Svg.svg [
            Svg.text [
                svg.text title
                svg.x (int p'.X)
                svg.y (int p'.Z)
            ]
            this.drawPoint p
        ]
    member _.drawLine (p: PGA3D) (p2: PGA3D) =
        let p = translatePoint p
        let p2 = translatePoint p2
        Svg.line [
            svg.x1 (int p.X)
            svg.x2 (int p2.X)
            svg.y1 (int p.Z)
            svg.y2 (int p2.Z)
            svg.stroke "black"
        ]
[<ReactComponent>]
let App () =
    let grid = Coordinates(800f, 800f)
    let point_a = point(0f, 0f, 200f)
    let point_b = point(200f, 0f, 100f)
    let line = point_a &&& point_b
    
    let dir = direction(10f, 0f, 10f)
    let originRotor (angle: float32) =
        rotor(MathF.Tau / 4f, e1 * e3)
        // MathF.E * ((point(0f, 0f, 0f)) * angle)

    let angle = 0.02f
    
    let cameraPlaneDir = direction(0f, 0f, 1f)
    let player = point(-200f, 0f, -80f)
    
    let playerRotation = MathF.Tau / 8f
    let origin = point(0f, 0f, 0f)
    let rotationOrigin = e1 * e3 //(player.X * e1) * (player.Y * e3)
    // let rotationOrigin = player &&& (player + direction(0f, 10f, 0f))
    let rotationOrigin = player &&& direction(0f, 10f, 0f)
    let cameraPos =
        (rotor(playerRotation, rotationOrigin) *
            (player + direction(0f, 0f, 20f)) *
            ~~~rotor(playerRotation, rotationOrigin))
    let cameraLeft =
        (rotor(playerRotation, rotationOrigin) *
            (player + direction(-20f, 0f, 20f)) *
            ~~~rotor(playerRotation, rotationOrigin))
    let cameraRight =
        (rotor(playerRotation, rotationOrigin) *
            (player + direction(20f, 0f, 20f)) *
            ~~~rotor(playerRotation, rotationOrigin))
    let playerForward =
        (rotor(playerRotation, rotationOrigin) *
            direction(0f, 0f, 1f) *
            ~~~rotor(playerRotation, rotationOrigin))
    let forward = player + (400f * playerForward)
    // let cameraPlaneOrigin = player + (20f * playerForward)
    // let cameraLeft = player + (20f * (playerForward + left))
    // let cameraRight = player + (20f * (playerForward + right))
    let intersection = player &&& (point_a &&& point_b)
    let screen = point(0f, 1f, 0f) &&& point(0f, 0f, 0f) &&& point(1f, 0f, 0f)
    let rayDir = player &&& point(0f, 0f, 0f)
    let rotatedPointA = (originRotor angle) * point_a * ~~~(originRotor angle)
    // let (c, cameraLeft, cameraRight) =
    //     let rotor = rotor(playerRotation, player)
    //     let c = rotor * cameraPlaneOrigin * ~~~rotor
    //     // let cameraLeft = rotor * cameraLeft * ~~~rotor
    //     // let cameraRight = rotor * cameraRight * ~~~rotor
    //     (c, cameraLeft, cameraRight)
    // let rayIntersection = rayDir ^^^  
    Html.div [
        // Html.h4 "Hello, world!"
        // Html.h4 (string point_a.ToPoint)
        // Html.h4 (string (intersection.normalized()))
        // Html.h4 $"dir = {dir}"
        // Html.h4 $"point_a + dir = {point_a + dir}"
        // Html.h4 $"point_a + dir = {(point_a + dir).ToPoint}"
        // Html.h4 $"rotated point {point_a} around rotor {originRotor angle} = {rotatedPointA.normalized()}"
        // Html.h4 $"rotated point {point_a.ToPoint} around rotor = {rotatedPointA.ToPoint}"
        Html.h4 $"player = {player.ToPoint}"
        // Html.h4 $"cameraPlaneOrigin = {cameraPlaneOrigin.ToPoint}"
        Html.h4 $"cameraPos = {cameraPos}"
        Html.h4 $"cameraPos = {cameraPos.ToPoint}"
        Html.h4 $"rotationOrigin = {rotationOrigin}"
        
        Html.h4 $"forward = {forward.ToPoint}"
        // Html.h4 $"cameraLeft = {cameraLeft.ToPoint}"
        // Html.h4 $"cameraRight = {cameraRight.ToPoint}"
        Svg.svg [
            svg.width 800
            svg.height 800
            svg.children [
                grid.drawLine (point(-400f, 0f, 0f)) (point(400f, 0f, 0f))
                grid.drawLine (point(0f, 0f, -400f)) (point(0f, 0f, 400f))
                // grid.drawPoint point_a
                // grid.drawPoint point_b
                // grid.drawLine point_a point_b
                // grid.drawPoint rotatedPointA
                // grid.drawPoint cameraPlaneOrigin
                
                grid.drawPoint (player, "player")
                grid.drawPoint (forward, "forward")
                grid.drawPoint (cameraPos, "cameraPos")
                // grid.drawPoint cameraPlaneOrigin
                grid.drawPoint cameraLeft
                grid.drawPoint cameraRight
            ]
        ]
    ]

ReactDOM.createRoot(document.body)
|> fun root -> root.render(App ())