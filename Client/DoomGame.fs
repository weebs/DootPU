module Dootverse.Client.DoomGame

open Fable.Core
open Browser
// open Fable.Core.JS

open Feliz
open Wad.Reader.Mixins

[<Emit("fetch($0, $1)")>]
let fetch (url: string) (obj: obj) : JS.Promise<Fetch.Types.Response> = nativeOnly

let alignToScreen width height minX minY maxX maxY distX distY (p: Wad.Reader.Vertex) =
    let x = (single (p.PosX - minX) / single distX) * single width
    // todo: backwards for SVG
    let y = (single (maxY - p.PosY) / single distY) * single height
    int x, int y


type viewPort = { width: float32; height: float32 }
[<ReactComponent>]
let Minimap (level: Wad.Reader.Level) =
    let lines = level.LineDefs
    let verts = level.Vertexes
    let width = 320
    let height = 240
    let line_verts = React.useMemo((fun () ->
        level.LineDefs
        |> Array.map (fun line -> [| verts[int line.StartVertex]; verts[int line.EndVertex] |])
        |> Array.collect id
    ), [| level.LineDefs |])
    let minX = (line_verts |> Array.minBy (fun v -> v.PosX)).PosX
    let maxX = (line_verts |> Array.maxBy (fun v -> v.PosX)).PosX
    
    let maxY = (line_verts |> Array.maxBy (fun v -> v.PosY)).PosY
    let minY = (line_verts |> Array.minBy (fun v -> v.PosY)).PosY
        
    let distX = System.Math.Abs(maxX - minX)
    let distY = System.Math.Abs(maxY - minY)
    let playerObject = level.Things |> Array.find (fun thing -> thing.Type = 1s)
    
    let ((x, y), setPlayer) = React.useStateWithUpdater((playerObject.PosX, playerObject.PosY))
    let (x, y) = alignToScreen width height minX minY maxX maxY distX distY { PosY = int16 y; PosX = int16 x }
    console.log ("player coords = ", (x, y))
        
    let svgMinimapLines = React.useMemo((fun () -> [|
        for line in level.LineDefs do
            let v1 = verts[int line.StartVertex]
            let (v1x, v1y) = alignToScreen width height minX minY maxX maxY distX distY v1
            let v2 = verts[int line.EndVertex]
            let (v2x, v2y) = alignToScreen width height minX minY maxX maxY distX distY v2
            // console.log(int v1.PosX, int v1.PosY, int v2.PosX, int v2.PosY)
            // Raylib.DrawText(sprintf "%A %A %A %A %A %A" width height minX minY maxX maxY, 0, 0, 12, Color.BLUE)
            // if count = 1 then
            //     Raylib.DrawText(sprintf "%A %A %A %A" v1x v1y v2x v2y, 0, 40, 12, Color.BLUE)
            // Raylib.DrawCircle(int v1x, int v1y, 2f, Color.BLUE)
            // console.log(v1x, v1y, v2x, v2y)
            yield (v1x, v1y, v2x, v2y)
        |]), [| level.LineDefs |])
    React.useEffectOnce (fun () ->
        document.onkeydown <- fun ev ->
            console.log ev
            if ev.key = "ArrowUp" then
                setPlayer (fun (x, y) -> (x, y + 8s))
    )
    Svg.svg [
        svg.width 640
        svg.height 480
        svg.children [
            for (x1, y1, x2, y2) in svgMinimapLines do
                Svg.line [
                    svg.x1 x1
                    svg.y1 y1
                    svg.x2 x2
                    svg.y2 y2
                    svg.stroke "blue"
                ]
            Svg.circle [
                svg.cx x
                svg.cy y
                svg.r 2
            ]
        ]
    ]
promise {
    let! response = fetch "doom1.wad" null
    
    let! data = response.arrayBuffer()
    let reader = BinaryReader(MemoryStream(data))
    
    let header = (reader.ReadChars 4)
    let numLumps = reader.ReadInt32()
    let dirAddress = reader.ReadInt32()
    
    // let level = Wad.Reader.parseLevel reader
    let file = Wad.Reader.File.readFile data
    let headers = file
    let levelsData = Wad.Reader.levelsData file
    for level in levelsData do
        console.log level.Key
        console.log level.Value.Keys
    let level = Wad.Reader.parseLevelFable reader levelsData["E1M1"]
    let geometry = Wad.Reader.getLevelGeometryData level
    let verts = level.Vertexes
    
    
    // console.log level
    console.log $"header = {header}, # of lumps = {numLumps}, dirAddress = {dirAddress}"
    // for info in headers do
    //     printfn $"{info.Name} - {info.Size} - {info.FilePos}"
        
    console.log file
    console.log level
    
    console.log geometry
    
    console.log (level.Things[0])

    ReactDOM.createRoot(document.getElementById "root")
    |> fun root -> root.render(Minimap level)
} |> ignore
