module Wad.Reader
open System
open System.IO
open Terminal.Gui
open System.IO

type App() as this =
    inherit Window()
    
    do
        this.Title <- "Example App!"
    let btn = new Button(Text = "Say Hi")
    //
    // do btn.OnClicked <- fun () -> printfn "click"
    member this.Foo = ()
    
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
try
    let contents = [|
        for _ in 1..numLumps do
            let filePos = dir.ReadInt32()
            let size = dir.ReadInt32()
            let name = 
                dir.ReadChars(8)
                |> Array.map string
                |> String.concat ""
            yield {
                FilePos = filePos
                Size = size
                Name = name
            }
    |]
    let vertexes = [|
        for info in contents |> Array.filter (fun info -> info.Name = "VERTEXES") do
            // printfn $"Data: {info.Name} (size: {info.Size})"
            let data = bytes |> Array.skip info.FilePos |> Array.take info.Size
            use reader = new BinaryReader(new MemoryStream(data))
            yield info, { PosX = reader.ReadInt16(); PosY = reader.ReadInt16() }
    |]
    for info in contents do
        printfn $"{info.Name} - "
    Application.Shutdown ()
    
    // printfn "%A" vertexes
    // printfn "%A" vertexData
    // printfn "verts = %A" verts
    // let view model dispatch =
    //     printfn "yo"
    //     View.page [
    //         page.menuBar [
    //             menubar.menus [
    //                 menu.menuBarItem [
    //                     menu.prop.title "File Explorer!"
    //                 ]
    //             ]
    //         ]
    //     ]
    // let update msg model = model, []
    // // Application.Shutdown()
    // Program.quit ()
    // Console.Clear()
    //
    Threading.Thread(Threading.ThreadStart(fun () ->
    //     let program = Program.mkProgram (fun _ -> (), []) update view
    //     Program.run program
        Application.Run<App> ()
    )).Start()
    // task { Application.Run<App> () }
    // |> ignore
    
    
with error -> printfn "%A" error