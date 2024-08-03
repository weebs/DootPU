module Raycast.Compute.Program

open System.IO
open FSharp.Data.LiteralProviders
open System.Net.WebSockets
open type System.Text.Encoding
open Bolero.Html
open System
open System.Threading
open System.Collections.Concurrent
open System.Diagnostics
open System.Reflection
open System.Threading.Tasks
open Dootverse.WebGPU
open Dootverse.WebGPU.Wgsl
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Silk.NET.WebGPU

open ComputeShaders
open WatsonWebsocket
open System.Collections.Concurrent

type NetworkSurface() =
    let ws = new WatsonWsServer("127.0.0.1", 1338)
    let mutable clientId = Guid.Empty
    do ws.ClientConnected.Add <| fun e -> clientId <- e.Client.Guid

    do
        ws.MessageReceived.Add
        <| fun e ->
            let msg = UTF8.GetString e.Data

            let fn (msg: string) =
                ws.SendAsync(e.Client.Guid, msg) |> ignore

            ()

    do ws.Start()

    member this.SendFrame(frame: byte[]) =
        if clientId <> Guid.Empty then
            ws.SendAsync(clientId, frame)
        else
            task { return false }

let ns = NetworkSurface()

// let mutable posX = 32f
let mutable posX =
    // 35f
    // 35.5f
    // 33.5f
    // 34f // todo : vertical line split
    29.5f
// 30.5f
// 35.4f
let mutable debugX =
    // 0
    // 600 // Random line
    650 // Random line
    640
    0
    100
// 110u
// 180u
// 117u
// 608u // 500u
let debugY =
    0
    125
// 328u
// 350u
// 464u // 420u
let runEventLoop = true
let mutable posZ = 32f
let scale = 7

let config =
    ({
        gridSize = 64
        widthPixels = 50 * scale
        heightPixels = 35 * scale
        cameraX = 64f / 2.0f
        cameraY = 0.2f
        cameraZ = 64f / 2.0f
        voxelGridScale = 1f
        debugX = debugX
        debugY = debugY
        blue = BitConverter.ToInt32([| 0uy; 0uy; 255uy; 255uy |], 0)
        purple = BitConverter.ToInt32([| 100uy; 120uy; 200uy; 255uy |], 0)
    }
    : Shaders.Config)
// let bytes = [|
//     for y in 1..height do
//         for x in 1..width do
//             let raycast = raycasts[x - 1]
//             if raycast.t <> 4000 then
//                 // let bounds = (320f / raycast.distance) - 0.5f
//                 let ph = 320f / raycast.distance
//                 let topY = ph / 2f
//                 let botY = -ph / 2f
//                 // let gridY = ((float32 y / float32 height) - 0.5f) * -1f
//                 // let gridY = float32 y - 160f
//                 let gridY = (float32 (y - height) * -1f) - 160f
//                 if gridY < topY && gridY > botY then
//                     yield 0uy
//                     yield 255uy
//                     yield 255uy
//                 else
//                     yield 0uy
//                     yield 0uy
//                     yield 0uy
//             else
//                 yield 0uy
//                 yield 0uy
//                 yield 0uy
// |]
let getColor (raycast: Shaders.RaycastResult) =
    if raycast.color <> 0 then
        BitConverter.GetBytes raycast.color
    else
        [| 0uy; 0uy; 0uy; 0uy |]
// if r.t = 1337 then
//     yield 0uy
//     yield 255uy
//     yield 255uy
//     yield 255uy
// elif r.color <> 0 then
//     yield! BitConverter.GetBytes r.color
// else
//     yield 0uy
//     yield 0uy
//     yield 0uy
//     yield 0uy

let createImage (raycasts: Shaders.RaycastResult[]) =
    let data = ResizeArray()

    for i in 0 .. raycasts.Length - 1 do
        getColor raycasts[i] |> data.AddRange

    data
// let ray1 = raycasts[250000]
// let ray2 = raycasts[250001]
let r = Random()

let map = [|
    for z in 1 .. config.gridSize do
        for y in 1 .. config.gridSize do
            for x in 1 .. config.gridSize do
                if y % 2 = 0 && x % 2 = 0 && z % 12 = 0 then
                    // 1
                    let inline random () = r.NextInt64 255L |> byte
                    let data = [| random(); random(); random(); 255uy |]
                    BitConverter.ToInt32(data, 0)
                else
                    0
// if y > config.gridSize / 2 && x > config.gridSize / 2 && (x - (y % 8)) % 4 = 0 then
//     1
// else
//     0
|]

let setCoordinate x z value =
    map[(z * config.gridSize * config.gridSize) + x] <- value

setCoordinate 34 34 1337
setCoordinate 33 34 1337
setCoordinate 33 38 1337
setCoordinate 33 41 1337
setCoordinate 33 43 1337
setCoordinate 33 47 1337
setCoordinate 33 48 1337
setCoordinate 28 34 1337
setCoordinate 28 34 1337
setCoordinate 28 38 1337
setCoordinate 28 41 1337
setCoordinate 28 43 1337
setCoordinate 28 47 1337
setCoordinate 28 48 1337
setCoordinate 30 50 1337

let testCoordinate (program: Shaders.Raycaster) x y =
    // let y = y / config.widthPixels
    // let x = x - (y * config.widthPixels)
    program.main { x = uint x; y = uint y; z = 0u }
// let ddaTest () =
// let outputRaycasts = Array.zeroCreate (config.widthPixels * config.heightPixels)
// let program = Shaders.Raycaster(config, map, outputRaycasts)
// do // todo : testing on cpu
// let outputRaycasts = Array.zeroCreate (config.widthPixels * config.heightPixels)
// let steps = Array.zeroCreate 1000
// let program = Shaders.Raycaster(config, map, outputRaycasts, steps)
// testCoordinate program 200 320
let outputRaycasts =
    Array.zeroCreate(config.widthPixels * config.heightPixels)

let steps = Array.zeroCreate 1000

let program =
    Shaders.Raycaster(
        {
            config with
                cameraX = posX
                cameraZ = posZ
                debugX = debugX
        },
        map,
        outputRaycasts,
        steps
    )

let debugPixelCpu () =
    program.main {
        x = uint debugX
        y = uint debugY
        z = 0u
    }

let renderImageOnCpu () =
    let runJob t q =
        for j in 1 .. config.heightPixels / q do
            for i in 1 .. config.widthPixels do
                // for j in 1..config.heightPixels / 10 do
                // let x = ((i - 1) + t * (config.widthPixels / q))
                let x = i - 1
                // let y = ((j - 1) * t * 10)
                // let y = j - 1
                let y = ((j - 1) + t * (config.heightPixels / q))

                if x = 500 && y = 420 then
                    ()

                program.main { x = uint x; y = uint y; z = 0u }

    let tasks = ResizeArray()
    let q = 10

    for t in 0 .. q - 1 do
        // runJob t
        tasks.Add(task { runJob t q } :> Task)
    // task {
    // } :> Task
    Task.WaitAll(tasks.ToArray())
    createImage outputRaycasts

Environment.SetEnvironmentVariable("RUST_BACKTRACE", "full")
let wgpu = new WebGPU'(WebGPU.GetApi())
let result = Setup.compileModule Shaders.Raycaster
let code = Compiler.Print.module' result
let setup = wgpu.CreateBinder Shaders.Raycaster
let mutable wroteGrid = false

let createCompute () =
    let (cfg, setup) = Wgpu.Bind setup

    let (grid, setup) =
        Wgpu.Bind
            setup
            (config.gridSize * config.gridSize * config.gridSize)

    let (output, setup) =
        Wgpu.Map setup (config.heightPixels * config.widthPixels)

    let (steps, setup) = Wgpu.Map setup 1000
    printfn $"{code}"
    let compute = new Extensions.ComputePipeline("main", setup)
    let sw = Stopwatch()

    let runRaycasts x z =
        sw.Restart()
        // compute.Begin (uint (config.widthPixels / 64), 1u, 1u)
        compute.Begin
            (uint config.widthPixels, uint config.heightPixels / 64u, 1u)
            (fun encoder ->
                output.AddCopy(wgpu, encoder)
                steps.AddCopy(wgpu, encoder)
            )
            (fun queue ->
                cfg.Write(
                    wgpu,
                    queue,
                    {
                        config with
                            cameraX = x
                            cameraZ = z
                            debugX = debugX
                    }
                )

                if not wroteGrid then
                    grid.Write(wgpu, queue, 0uL, map)
                    wroteGrid <- true
            )

        sw.Stop()

        task {
            let! raycasts =
                output.ReadBufferRange(
                    wgpu,
                    0,
                    config.widthPixels * config.heightPixels
                )

            let! steps = steps.ReadBufferRange(wgpu, 0, 1000)
            let elapsed = sw.ElapsedTicks
            let formatted = (double elapsed * 0.1).ToString("0.#") + "us"
            // printfn $"{raycasts}"
            printfn $""
            printfn $"{x}, {z} => {formatted}"
            // steps |> Seq.takeWhile (fun s -> s.voxelX <> 0f) |> Seq.iter (printfn "%A")
            // printfn $"{raycasts[(config.widthPixels * debugY) + debugX]}"
            return raycasts
        }

    runRaycasts

let gpuRaycasts = createCompute()

let width = config.widthPixels
let height = config.heightPixels
//     for i in 0..config.widthPixels - 1 do
//         for j in 0u..639u do
//             program.main { x = uint i; y = j; z = 0u }

let t = task

type Keyboard() =
    let keyboard = ConcurrentDictionary()

    do
        let initKeys = "abcdefghijklmnopqrstuvwxyz1234567890"

        for k in initKeys do
            keyboard[string k] <- false

    member this.Item
        with get index =
            if keyboard.ContainsKey index then
                keyboard[index]
            else
                false
        and set index value = keyboard[index] <- value

type Interop =
    static let keyboard = ConcurrentDictionary()

    static do
        let initKeys = "abcdefghijklmnopqrstuvwxyz1234567890"

        for k in initKeys do
            keyboard[string k] <- false

    [<JSInvokable>]
    static member KeyboardInput(key: string, code: string, pressed: bool) =
        printfn $"{key} {code} {pressed}"
        keyboard[key] <- pressed
        keyboard[code] <- pressed

    static member Keyboard = keyboard

open Bolero
// type MyComponent() =
//     inherit ElmishComponent<string, int>()

//     override this.View model dispatch =
//         printfn "elmish"
//         div { "SimpleElmish" }

// type FuckU<'a, 'b, 'c when 'c :> ElmishComponent<'a, 'b> and 'c :> FuckU<'a, 'b, 'c>>() =
// override this.View model dispatch = failwith "todo"

// let inline ecomp'<'a, 'b, 'c> = Bolero.Html.ecomp<'a, 'b, 'c>

// let model = Operators.Unchecked.defaultof<_>
// let dispatch = Operators.Unchecked.defaultof<_>

// let ecomp (ctor: unit -> 'c) model dispatch =
// Bolero.Html.ecomp<'c, 'model, 'dispatch> model dispatch

// ecomp MyComponent model dispatch

[<AbstractClass>]
type FuckU<'a, 'b, 'c
    when 'c :> ElmishComponent<'a, 'b> and 'c :> FuckU<'a, 'b, 'c>>
    (value: 'c) =
    // type FuckU<'a, 'b, 'c when 'c :> ElmishComponent<'a, 'b> and 'c :> FuckU<'a, 'b, 'c>>() =
    // type FuckU<'a, 'b, 'c when 'c :> FuckU<'a, 'b, 'c>>() =
    inherit ElmishComponent<'a, 'b>()
    // new (a: 'a, foo: 'foo) = FuckU()
    static member view model dispatch =
        Bolero.Html.ecomp<'c, _, _> model dispatch

// let f = { new FuckU<_,_,_>(1, 2) with
//             override this.View model dispatch = failwith "todo" }
// let f' = { new FuckU<_,_,_>(1, 2) with
//             override this.View model dispatch = failwith "todo" }

// type ElmishComponent<'a, 'b> with
//     static member view model dispatch =
//         ()
// type Yo() as this =
//     inherit FuckU<int, int, Yo>(this)
//     override this.View model dispatch =
//         printfn "view"
//         div {
//             "doot"
//         }

type Builders.ElementBuilder with
    // member this.Yield (a: unit -> 'a, (b: 'model, c: 'msg -> unit)) =
    member this.Yield(tpl: (unit -> 'a) * 'model * ('msg -> unit)) =
        let _, b, c = tpl
        Bolero.Html.ecomp<'a, 'model, 'msg> b c |> this.Yield

type App() as this =
    inherit Component()

    let code =
        $$"""
{{File.ReadAllText TextFile.``env.js``.Path}}
init({{config.widthPixels}}, {{config.heightPixels}}, `{{File.ReadAllText TextFile.``worker.js``.Path}}`)
"""

    let useCpu = false

    [<Inject>]
    member val js = Unchecked.defaultof<IJSRuntime> with get, set

    member this.render useCpu =
        task {
            let! raycasts = gpuRaycasts posX posZ
            let img = createImage raycasts
            let! _ = ns.SendFrame(img.ToArray())
            // do! this.js.InvokeVoidAsync ("drawImage", img)
            debugPixelCpu()

            if useCpu then
                task {
                    let image = renderImageOnCpu()
                    do! this.js.InvokeVoidAsync("drawImage", image)
                }
                |> ignore
        }
        :> Task

    member this.StartRender() =
        let loop () =
            while runEventLoop do
                // do! Async.Sleep 25
                // Threading.Thread.Sleep 17
                Threading.Thread.Sleep 8

                if Interop.Keyboard["a"] then
                    posX <- posX - 0.05f

                if Interop.Keyboard["d"] then
                    posX <- posX + 0.05f

                if Interop.Keyboard["w"] then
                    posZ <- posZ + 0.05f

                if Interop.Keyboard["s"] then
                    posZ <- posZ - 0.05f
                // do! this.render false
                let task = this.render false
                task.Wait()

        Thread(ThreadStart(loop)) |> _.Start()


    override this.OnInitializedAsync() = task {
        do! this.js.InvokeVoidAsync("eval", code)
        do! this.render false
        this.StartRender()
    // } |> ignore
    }

    override this.OnAfterRenderAsync _ = task { do! this.render false }

    override this.Render() =
        let asm = Assembly.GetExecutingAssembly()
        // let movementSpeed = 0.0625f / 32f
        let movementSpeed = 0.25f

        div {
            on.click(fun _ ->
                task { do! this.js.InvokeAsync("log", [| 1; 2; 3; 4 |]) }
                |> ignore
            )
            // ecomp
            // let a = ecomp App "" (printfn "%A")
            // Bolero.Html.ecomp<MyComponent,_,_> "" (printfn "%A")
            // ecomp MyComponent "" Console.WriteLine
            // Yo.view 1337 (printfn "%A")
            // SimpleElmish, "", ""

            // a
            input {
                attr.``type`` "text"
                on.change(fun e -> debugX <- Int32.Parse(string e.Value))
                attr.value debugX
            }

            button {
                on.click(fun _ -> posX <- posX - movementSpeed
                // this.render true |> ignore
                )

                "Left"
            }

            button {
                on.click(fun _ -> posX <- posX + movementSpeed
                // this.render true |> ignore
                )

                "Right"
            }

            input {
                attr.``type`` "range"
                attr.min "24"
                attr.max "50"
                attr.value posX
                attr.step "0.01"

                on.input(fun e ->
                    match Double.TryParse(string e.Value) with
                    | true, d -> posX <- float32 d
                    | _ -> ()
                // printfn $"{string e.Value}"
                )
            }

        // p { $"{posX}, {posZ}" }
        }

// ImageDisplay.showImage width height bytes'
// let foo = webviews[e.Client.Guid]
// foo.ProcessMsg (msg, fn)

Bolero.WebView.Program.startAppThread<App>() |> _.Join()

// type Asdf<'t when 't: (member ToString: unit -> string)>(t: 't) =
//     member inline this.Value =
//         t.ToString()
// type Asdf2<'t when 't: (member ToString: string)>(t: 't) =
//     member inline this.Value =
//         t.ToString

// let inline asdf (a: Asdf<_>) =
//     a.Value

// let a = Asdf(1234)
// let s = a.Value
// let b = Asdf2({| ToString = "" |})
