module Wad.Reader
open System
open System.Linq
open System.IO
// open Terminal.Gui
open System.IO
open System.Numerics
open System.Security.Cryptography
open Raylib_cs


// type App() as this =
//     inherit Window()
//     
//     do
//         this.Title <- "Example App"
//     let btn = new Button(Text = "Say Hi")
//     //
//     // do btn.OnClicked <- fun () -> printfn "click"
//     member this.Foo = ()
    
let filePath = Path.Join(__SOURCE_DIRECTORY__, "doom1.wad")
printfn "Hello from F#"
printfn "%A" (File.ReadAllBytes filePath)

type ParserResult<'t, 'state> = //, 'u, 'm> =
    | Success of 't * 'state
    // | Partial of 't * string * 'state
    | Error of string
type Parser<'state, 'stream, 't> = 'state -> 'stream -> ParserResult<'t, 'state>
let parseInt16 : Parser<int, byte[], int16> = fun state stream ->
    match stream[state] with
    | digit when "0123456789".Contains(char digit) ->
        match stream[state + 1] with
        | digit2 when "0123456789".Contains(char digit2) ->
            Success (Int16.Parse(string digit + string digit2), state + 2)
        | digit2 -> Error <| "Unexpected digit 2: " + string digit2
    | digit -> Error <| "Unexpected digit 1: " + string digit
let andThen parser parser2 state stream =
    match parser state stream with
    | Success(result, nextState) ->
        match parser2 nextState stream with
        | Success (result2, state2) ->
            Success ((result, result2), state2)
        | Error string ->
            Error string
    | Error string -> Error string
let twoInts = andThen parseInt16 parseInt16 0 (File.ReadAllBytes filePath)
let bytes = File.ReadAllBytes filePath
let reader = new BinaryReader(new MemoryStream(bytes))
let header = reader.ReadChars(4)
let numLumps = reader.ReadInt32()
let dirAddress = reader.ReadInt32()
printfn $"Header = %A{header}"
printfn $"numLumps = {numLumps}"
printfn $"dirAddress = {numLumps}"

// printfn $"twoInts = %A{twoInts}"
// printfn $"# bytes = {bytes.Length}"
let dir = new BinaryReader(new MemoryStream(bytes |> Array.skip dirAddress))
type [<Struct>] Lump = { FilePos: int; Size: int; Name: string }
type [<Struct>] Thing = { PosX: int16; PosY: int16; Angle: int16; Type: int16; Flags: int16 }
type [<Struct>] Vertex = { PosX: int16; PosY: int16 }
type [<Struct>] SideDefs = { OffsetX: int16; OffsetY: int16; UpperTexture: string; LowerTexture: string; MiddleTexture: string; Sector: int16 }
type [<Struct>] LineDef = {
    StartVertex: int16
    EndVertex: int16
    Flags: int16
    // ActionSpecial: int16
    SpecialType: int16
    SectorTag: int16
    // todo: Doom 2 only
    FrontSideDef: int16
    BackSideDef: int16
}
type [<Struct>] Subsector = { SegmentCount: int16; FirstSegmentIndex: int16 }

type [<Struct>] Segment = {
    StartVert: int16
    EndVert: int16
    Angle: int16
    LineNumber: int16
    // https://doomwiki.org/wiki/Seg
    Direction: int16 // 0 = same as linedef, 1 = opposite of linedef
    SegmentOffset: int16
}




type [<Struct>] Sector =
  { FloorHeight: int16
    CeilingHeight: int16
    FloorTexture: string
    CeilingTexture: string
    AmbientLight: int16
    Special: int16
    Tag: int16 }
type Level = {
    Things: Thing[]
    LineDefs: LineDef[]
    SideDefs: SideDefs[]
    Vertexes: Vertex[]
    Segs: Segment[]
    Subsectors: Subsector[]
    Nodes: Lump
    Sectors: Sector[]
    Reject: Lump
    Blockmap: Lump
}
let readThing (reader: BinaryReader) =
    { PosX = reader.ReadInt16(); PosY = reader.ReadInt16(); Angle = reader.ReadInt16(); Type = reader.ReadInt16(); Flags = reader.ReadInt16() }
let readString (reader: BinaryReader) =
    reader.ReadChars(8) |> Array.filter (fun c -> c <> char "\000") |> Array.map string |> (String.concat "")
let readVertex (reader: BinaryReader) =
    { PosX = reader.ReadInt16(); PosY = reader.ReadInt16() }
let readSegment (reader: BinaryReader) =
    {
        StartVert = reader.ReadInt16()
        EndVert = reader.ReadInt16()
        Angle = reader.ReadInt16()
        LineNumber = reader.ReadInt16()
        Direction = reader.ReadInt16()
        SegmentOffset = reader.ReadInt16()
    }
    
let readSector (reader: BinaryReader) =
    {
        FloorHeight = reader.ReadInt16()
        CeilingHeight = reader.ReadInt16()
        FloorTexture = readString reader
        CeilingTexture = readString reader
        AmbientLight = reader.ReadInt16()
        Special = reader.ReadInt16()
        Tag = reader.ReadInt16()
    }
let readSubsector (reader: BinaryReader) =
    {
        SegmentCount = reader.ReadInt16()
        FirstSegmentIndex = reader.ReadInt16() 
    }
let readLineDef (reader: BinaryReader) =
    { StartVertex = reader.ReadInt16()
      EndVertex = reader.ReadInt16()
      Flags = reader.ReadInt16()
      // ActionSpecial = reader.ReadInt16() 
      SpecialType = reader.ReadInt16()
      SectorTag = reader.ReadInt16()
      FrontSideDef = reader.ReadInt16() 
      BackSideDef = reader.ReadInt16() }
let readSideDef (reader: BinaryReader) : SideDefs =
    {
        OffsetX = reader.ReadInt16()
        OffsetY = reader.ReadInt16()
        UpperTexture = readString reader
        LowerTexture = readString reader
        MiddleTexture = readString reader
        Sector = reader.ReadInt16() 
    }
let toReader lump =
    let bytes = bytes |> Array.skip lump.FilePos |> Array.take lump.Size
    new BinaryReader(new MemoryStream(bytes))
let parse<'t> (reader: BinaryReader -> 't) lump =
    let r = toReader lump
    [|
        for i in 1..(lump.Size / sizeof<'t>) do
            yield reader r
    |]
try
    let headers = [|
        for _ in 1..numLumps do
            let filePos = dir.ReadInt32()
            let size = dir.ReadInt32()
            let name = 
                dir.ReadChars(8)
                |> Array.map string
                |> Array.filter (fun s -> s <> "\000")
                |> String.concat ""
            yield {
                FilePos = filePos
                Size = size
                Name = name
            }
    |]
    let vertexes = [|
        for info in headers |> Array.filter (fun info -> info.Name = "VERTEXES") do
            // printfn $"Data: {info.Name} (size: {info.Size})"
            let data = bytes |> Array.skip info.FilePos |> Array.take info.Size
            use reader = new BinaryReader(new MemoryStream(data))
            yield info, { PosX = reader.ReadInt16(); PosY = reader.ReadInt16() }
    |]
    for info in headers do
        printfn $"{info.Name} - {info.Size} - {info.FilePos}"
        
        
    // try Application.Driver.Refresh () with _ -> ()
    // Environment.SetEnvironmentVariable("LOOP", "TRUE")
    // try Application.RequestStop(Application.Current) with _ -> ()
    
    // Application.Shutdown()
    // Threading.Thread.Sleep(2000)
    
    
    
    // Console.Clear()
    
    //
    // Threading.Thread(Threading.ThreadStart(fun () ->
    //     Application.Run<App> ()
    // )).Start()
    let cache = headers |> Array.groupBy (fun info -> info.Name) |> Map.ofArray
    // printfn "%A %A" cache["THINGS"] (bytes |> Array.skip dirAddress |> Array.skip cache["THINGS"].[0].Size)
    // printfn "%A" (Array.ofSeq cache.Keys |> Array.filter (fun key -> key.StartsWith "THIN"))
    for item in cache["THINGS"] do
        printfn "%A" (bytes |> Array.skip item.FilePos)
        printfn "%A" item
        printfn "%A" (new BinaryReader(new MemoryStream(bytes |> Array.skip item.FilePos)) |> readThing)
    let levelsData =
        headers
        |> Array.mapi (fun index info ->
            if info.Name.StartsWith "E" && info.Name.Contains "M" && info.Size = 0
            then Some index
            elif info.Name = "BLOCKMAP" then Some index
            else None)
        |> Array.choose id
        |> Array.chunkBySize 2
        |> Array.map (fun chunk -> headers |> Array.skip chunk[0] |> Array.take (1 + chunk[1] - chunk[0]))
        |> Array.map (fun level -> level[0].Name, level |> Array.map (fun header -> header.Name, header) |> Map.ofArray)
        |> Map.ofArray
        
    let parseLevel (level: Map<string, Lump>) =
        {
            Things = parse readThing level["THINGS"]
            LineDefs = parse readLineDef level["LINEDEFS"]
            SideDefs = parse readSideDef level["SIDEDEFS"]
            Vertexes = parse readVertex level["VERTEXES"]
            Segs = parse readSegment level["SEGS"]
            Subsectors = parse readSubsector level["SSECTORS"]
            Nodes = level["NODES"]
            Sectors = parse readSector level["SECTORS"]
            Reject = level["REJECT"]
            Blockmap = level["BLOCKMAP"]
        }
    let e1m1 = parseLevel <| levelsData["E1M1"]
    let lines = e1m1.LineDefs
    let verts = e1m1.Vertexes
    printfn "indices = %A" e1m1.Vertexes[0]
    printfn "verts = %A" verts
        
        
    
    // Loop.handleInput <- fun s ->
    //     if s = "keys" then
    //         for key in cache.Keys do
    //             printfn "%A" key
    //     elif s = "headers" then
    //         for h in headers do
    //             printfn $"{h.Name}"
    //     else
    //         printfn "%A" cache[s]
    let mutable msg = ""
    State.c.position <- Vector3(1552f, 0f, -2560f)
    State.c.target <- Vector3(1553f, 0f, -2560f)
    State.callback3dFn <- fun () ->
        Raylib.DrawCube(Vector3(0f, 0f, 0f), 1f, 1f, 1f, Color.SKYBLUE)
        let segments =
            e1m1.Segs
            |> Array.map (fun segment ->
                {|
                    start = e1m1.Vertexes[int segment.StartVert]
                    _end = e1m1.Vertexes[int segment.EndVert]
                    line = e1m1.LineDefs[int segment.LineNumber]
                    dir = segment.Direction
                    offset = segment.SegmentOffset
                |}
            )
        let sectors =
            e1m1.Sectors
            |> Array.mapi (fun index sector ->
                sector, segments
                    |> Array.skip (int e1m1.Subsectors[index].FirstSegmentIndex)
                    |> Array.take (int e1m1.Subsectors[index].SegmentCount)
            )
        for (sector, lines) in sectors do
            // Raylib.DrawCube(Vector3(info.))
            for line in lines do
                let ceiling = single sector.CeilingHeight
                let floor = single sector.FloorHeight
                let v1 = Vector3(single line.start.PosX, ceiling, single line.start.PosY)
                let v1a = Vector3(single line._end.PosX, ceiling, single line._end.PosY)
                let v2 = Vector3(single line.start.PosX, floor, single line.start.PosY)
                let v2a = Vector3(single line._end.PosX, floor, single line._end.PosY)
                msg <- sprintf "%A %A %A %A" v1 v1a v2 v2a
                Raylib.DrawLine3D(v1, v1a, Color.DARKGREEN)
                Raylib.DrawLine3D(v2, v2a, Color.DARKGREEN)
                Raylib.DrawCube(v1, 1f, 1f, 1f, Color.BLUE)
        ()
    State.callbackFn <- fun () ->
        Raylib.DrawText(msg, 0, 0, 12, Color.DARKBROWN)
        let mutable count = 0
        try
            // Console.Clear()
            let width = Raylib.GetScreenWidth() / 2
            let height = Raylib.GetScreenHeight() / 2
            let line_verts =
                lines
                |> Array.map (fun line -> [| verts[int line.StartVertex]; verts[int line.EndVertex] |])
                |> Array.collect id
            let minX = (line_verts |> Array.minBy (fun v -> v.PosX)).PosX
            let minY = (line_verts |> Array.minBy (fun v -> v.PosY)).PosY
            let maxX = (line_verts |> Array.maxBy (fun v -> v.PosX)).PosX
            let maxY = (line_verts |> Array.maxBy (fun v -> v.PosY)).PosY
            let distX = maxX - minX
            let distY = maxY - minY
            let alignToScreen p =
                let x = (single (p.PosX - minX) / single distX) * single width
                let y = (single (p.PosY - minY) / single distY) * single height
                int x, int y
            let (cameraX, cameraY) = alignToScreen { PosX = int16 State.c.position.X; PosY = int16 State.c.position.Z }
            Raylib.DrawCircle(cameraX, cameraY, 10f, Color.GREEN)
            if Raylib.IsKeyDown(KeyboardKey.KEY_D) <> CBool false then
                State.c.position <- State.c.position + Vector3(0.1f, 0f, 0f)
            for line in lines do
                count <- count + 1
                // printfn "%A" line
                let v1 = verts[int line.StartVertex]
                let (v1x, v1y) = alignToScreen v1
                let v2 = verts[int line.EndVertex]
                let (v2x, v2y) = alignToScreen v2
                Raylib.DrawLine(int v1.PosX, int v1.PosY, int v2.PosX, int v2.PosY, Color.BLUE)
                // Raylib.DrawText(sprintf "%A %A %A %A %A %A" width height minX minY maxX maxY, 0, 0, 12, Color.BLUE)
                if count = 1 then
                    Raylib.DrawText(sprintf "%A %A %A %A" v1x v1y v2x v2y, 0, 40, 12, Color.BLUE)
                // Raylib.DrawCircle(int v1x, int v1y, 2f, Color.BLUE)
                Raylib.DrawLine(v1x, v1y, v2x, v2y, Color.BLUE)
                // Raylib.DrawText(sprintf "%A - %A" v1 v2, 20, 40, 24, Color.SKYBLUE)
                Raylib.DrawLine3D(Vector3(single v1.PosX, single v1.PosY, 0f), Vector3(0f, 0f, 0f), Color.DARKGREEN)
                // Raylib.ClearBackground(Color.WHITE)
                // Raylib.DrawText("Hello, world!", 0, 0, 24, Color.SKYBLUE)
        with error ->
            // Raylib.DrawText(string error, 0, 0, 24, Color.SKYBLUE)
            Raylib.DrawText(string count, 20, 40, 24, Color.SKYBLUE)
            // Raylib.DrawText(sprintf "%A" lines[count - 1], 80, 0, 24, Color.SKYBLUE)
            Raylib.DrawText(sprintf "%A" lines, 80, 100, 12, Color.SKYBLUE)
    
with error -> printfn "%A" error