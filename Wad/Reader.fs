module Wad.Reader

open System.IO
open Fable.Core
open Fable.Core.JsInterop

module Mixins =
    type MemoryStream(bytes: JS.ArrayBuffer) =
        member this.Buffer = bytes
    
    type BinaryReader(stream: MemoryStream) as this =
        let mutable streamPosition = 0
        let buffer = stream.Buffer
        // let blob = stream.Blob
        let view = JS.Constructors.DataView.Create buffer
        member this.Buffer = stream.Buffer
        // #if !FABLE_COMPILER
        // new(memoryStream: obj) = BinaryReader(memoryStream :?> _)
        // #endif
        
        // let advanceStream<'t> () =
        //     streamPosition <- streamPosition + sizeof<'t>
        member this.BaseStream = {| Position = int64 streamPosition |}
        member this.ReadChars(length: int) : char[] =
            // let chars = blob.slice(streamPosition, streamPosition + length)
            let items = [| for i in 1..length do yield (char 0) |]
            streamPosition <- streamPosition + length
            for i in streamPosition - length..streamPosition - 1 do
                items[streamPosition - length + i] <- char (view.getUint8(i))
            // streamPosition <- streamPosition + length
            // chars.
            items
        member this.ReadInt16() =
            streamPosition <- streamPosition + 2
            // True = Little endian
            view.getInt16 (streamPosition - 2, true)
        member this.ReadInt32() : int32 =
            streamPosition <- streamPosition + 4
            // True = Little endian
            view.getInt32 (streamPosition - 4, true)
#if FABLE_COMPILER
open Mixins
#endif


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
let toReader bytes lump =
#if !FABLE_COMPILER
    let bytes = bytes |> Array.skip lump.FilePos |> Array.take lump.Size
    new BinaryReader(new MemoryStream(bytes))
#else
    // todo: skip
    Mixins.BinaryReader(bytes?slice(lump.FilePos, lump.Size))
#endif
#if !FABLE_COMPILER
let inline parse<'t> bytes (reader: BinaryReader -> 't) (lump: Lump) : 't[] =
#else
let inline parse<'t> bytes (reader: Mixins.BinaryReader -> 't) (lump: Lump) : 't[] =
#endif
    let r = toReader bytes lump
    let start = r.BaseStream.Position
    let mutable offset = int64 0
    // let mutable i = 0
    [|
        while offset < lump.Size do
            offset <- r.BaseStream.Position - start
            // i <- i + 1
            // if i = 1 then
            //     let t = reader r
            // else
        // for i in 1..(lump.Size / sizeof<'t>) do
            yield reader r
    |]
let parseLevel dataView (level: Map<string, Lump>) =
    // let inline parse reader lump = parse dataView reader lump
    {
        Things = parse dataView readThing level["THINGS"]
        LineDefs = parse dataView readLineDef level["LINEDEFS"]
        SideDefs = parse dataView readSideDef level["SIDEDEFS"]
        Vertexes = parse dataView readVertex level["VERTEXES"]
        Segs = parse dataView readSegment level["SEGS"]
        Subsectors = parse dataView readSubsector level["SSECTORS"]
        Nodes = level["NODES"]
        Sectors = parse dataView readSector level["SECTORS"]
        Reject = level["REJECT"]
        Blockmap = level["BLOCKMAP"]
    }
let levelsData headers =
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

type File() =
    // #if !FABLE_COMPILER
//     let readFile (file: byte[]) =
//     #else
//     let readFile (file: JS.ArrayBuffer) =
//     #endif
//         // #if !FABLE_COMPILER
//         let reader = BinaryReader(new MemoryStream(file))
//         // #else
//         // let reader = BinaryReader(new MemoryStream(file))
//         // #endif
//         // JS.Constructors.Array.from<byte> reader
//         let header = reader.ReadChars(4)
//         let numLumps = reader.ReadInt32()
//         let dirAddress = reader.ReadInt32()
//
//         // let dir = BinaryReader(reader?slice(0, dirAddress))
//         #if !FABLE_COMPILER
//         let dir = new BinaryReader(new MemoryStream(file |> Array.skip dirAddress))
//         #else
//         let dir = new BinaryReader(new MemoryStream(file.slice(0, dirAddress)))
//         #endif
//         let headers = headers dir numLumps
//         ()
    #if !FABLE_COMPILER
    static member headers (dir: BinaryReader, numLumps) = [|
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
    #endif
    static member headers (dir: Mixins.BinaryReader, numLumps) = [|
        for _ in 1..numLumps do
            // JS.debugger ()
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
    #if !FABLE_COMPILER
    static member readFile (file: byte[]) =
        let reader = BinaryReader(new MemoryStream(file))
        let header = reader.ReadChars(4)
        let numLumps = reader.ReadInt32()
        let dirAddress = reader.ReadInt32()

        #if !FABLE_COMPILER
        let dir = new BinaryReader(new MemoryStream(file |> Array.skip dirAddress))
        #else
        let dir = new BinaryReader(new MemoryStream(file.slice(0, dirAddress)))
        #endif
        File.headers (dir, numLumps)
    #endif

    static member readFile (file: JS.ArrayBuffer) =
        let reader = Mixins.BinaryReader(new Mixins.MemoryStream(file))
        let header = reader.ReadChars(4)
        let numLumps = reader.ReadInt32()
        let dirAddress = reader.ReadInt32()

        let dir = new Mixins.BinaryReader(new Mixins.MemoryStream(file.slice(dirAddress)))
        let headers = File.headers (dir, numLumps)
        JS.console.log ("dir = ", dir, "headers = ", headers, numLumps, dirAddress)
        // for info in headers do
        //     printfn $"{info.Name} - {info.Size} - {info.FilePos}"
        headers
let parseFable (bytes: Mixins.BinaryReader) (reader: BinaryReader -> 't) (lump: Lump) : 't[] =
    let r = Mixins.BinaryReader(Mixins.MemoryStream(bytes.Buffer.slice(lump.FilePos, lump.FilePos + lump.Size)))
    let start = r.BaseStream.Position
    let mutable offset = int64 0
    // let mutable i = 0
    [|
        while offset < lump.Size do
            // i <- i + 1
            // if i = 1 then
            //     let t = reader r
            // else
        // for i in 1..(lump.Size / sizeof<'t>) do
            // todo
            // todo yield reader r
            offset <- r.BaseStream.Position - start
    |]
let parseLevelFable (dataView: Mixins.BinaryReader) (level: Map<string, Lump>) : Level =
    // let inline parse reader lump = parse dataView reader lump
    {
        Things = parseFable dataView readThing level["THINGS"]
        LineDefs = parseFable dataView readLineDef level["LINEDEFS"]
        SideDefs = parseFable dataView readSideDef level["SIDEDEFS"]
        Vertexes = parseFable dataView readVertex level["VERTEXES"]
        Segs = parseFable dataView readSegment level["SEGS"]
        Subsectors = parseFable dataView readSubsector level["SSECTORS"]
        Nodes = level["NODES"]
        Sectors = parseFable dataView readSector level["SECTORS"]
        Reject = level["REJECT"]
        Blockmap = level["BLOCKMAP"]
    }
let getLevelGeometryData e1m1 =
    let inline toPair (i: int) value = (i, value)
    let lines = e1m1.LineDefs
    let verts = e1m1.Vertexes
        
    let linesBySideDefId =
        Map.ofArray [|
            yield! (lines |> Array.map (fun line -> line.FrontSideDef, line) |> Array.groupBy fst)
            yield! lines |> Array.map (fun line -> line.BackSideDef, line) |> Array.groupBy fst
        |]
    let sidesBySectorId =
        Map.ofArray (
            e1m1.SideDefs
            |> Array.groupBy (fun sideDef -> sideDef.Sector))
    let linesBySectorId =
        Array.groupBy fst [|
            for (id, def) in Array.mapi toPair e1m1.SideDefs do
                if linesBySideDefId.ContainsKey (int16 id) then
                    for line in linesBySideDefId[int16 id] do
                        yield (def.Sector, line)
            // for line in e1m1.LineDefs do
            //     for def in e1m1.SideDefs do
            //         yield (def.Sector, line)
                // for sector in e1m1.Sectors do
            // for kv in sidesBySectorId do
            //     if e1m1.Sectors.Length > int kv.Key then
            //         let sector = e1m1.Sectors[int kv.Key]
            //         let sectorId = int kv.Key
            //         for side in kv.Value do
            //             for kv in linesBySideDefId do
            //                 for line in kv.Value do
            //                     yield (sectorId, line)
        |]
        |> Map.ofArray
        // |> Map.map (fun id lines -> lines |> Array.distinctBy snd)
    (linesBySectorId, sidesBySectorId)
