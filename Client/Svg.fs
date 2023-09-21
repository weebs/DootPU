module Dootverse.Client.Svg

open Feliz

type Screen(width, height, ?scale) =
    let scale = defaultArg scale 1
    let toSvgCoordinate x y =
        ((width / 2) + x * scale), ((height / 2) - y * scale)
    member this.Width = width
    member this.Height = height
    member _.voxel size x y color =
        let (x, y) = toSvgCoordinate (x * size) (y * size)
        Svg.rect [
            svg.x x
            svg.y (y - size)
            svg.width size
            svg.height size
            svg.fill color
        ]
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
    member _.circle (color, radius: int, x, y) =
        let (x, y) = toSvgCoordinate x y
        Svg.circle [
            svg.cx x
            svg.cy y
            svg.r radius
            svg.fill color
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
            svg.fill color
            svg.strokeWidth 2
        ]

let blockSize = 40
let drawGrid (screen: Screen) =
    [|
    // screen.circle 10 0 0
        for x in -screen.Width / blockSize / 2..screen.Width / blockSize / 2 do
            for y in (-screen.Height / blockSize / 2)..(screen.Height / blockSize / 2) do
                screen.circle ("green", 4, (x * blockSize), (y * blockSize))
        Svg.line [
            svg.x1 0
            svg.x2 screen.Width
            svg.y1 0
            svg.y2 0
            svg.stroke "black"
        ]
        Svg.line [
            svg.x1 0
            svg.x2 screen.Width
            svg.y1 screen.Height
            svg.y2 screen.Height
            svg.stroke "black"
        ]
        Svg.line [
            svg.x1 0
            svg.x2 screen.Width
            svg.y1 (screen.Height / 2)
            svg.y2 (screen.Height / 2)
            svg.stroke "black"
        ]
        Svg.line [
            svg.x1 (screen.Width / 2)
            svg.x2 (screen.Width / 2)
            svg.y1 0
            svg.y2 screen.Height
            svg.stroke "black"
        ]
    |]
