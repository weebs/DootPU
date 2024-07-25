module Raycast.Compute.Program

open Bolero.Html
open System
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

// let mutable posX = 32f
let mutable posX =
    // 35f
    // 35.5f
    33.5f
    // 29.5f
    // 30.5f
    // 35.4f
let mutable debugX =
    // 0
    // 600 // Random line
    650 // Random line
    640
    0
    // 110u
    // 180u
    // 117u
    // 608u // 500u
let debugY =
    0
    // 328u
    // 350u
    // 464u // 420u
let mutable posZ = 32f
let scale = 5
let config = ({
    gridSize = 64
    widthPixels = 50 * scale
    heightPixels = 35 * scale
    cameraX = 64f / 2.0f
    cameraY = 0.5f
    cameraZ = 64f / 2.0f
    voxelGridScale = 1f
    debugX = debugX; debugY = debugY
    blue = BitConverter.ToInt32([| 0uy; 0uy; 255uy; 255uy; |], 0)
    purple = BitConverter.ToInt32([| 100uy; 120uy; 200uy; 255uy; |], 0)
} : Shaders.Config)
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
let createImage (raycasts: Shaders.RaycastResult []) =
    [|
        for i in 0..raycasts.Length - 1 do
            let r = raycasts[i]
            if r.t = 1337 then
                yield 0uy
                yield 255uy
                yield 255uy
                yield 255uy
            elif r.color <> 0 then
                yield! BitConverter.GetBytes r.color
            else
                yield 0uy
                yield 0uy
                yield 0uy
                yield 0uy
    |]
// let ray1 = raycasts[250000]
// let ray2 = raycasts[250001]
let map = [|
    for y in 1..config.gridSize do
        for x in 1..config.gridSize do
            if y % 2 = 0 && x % 2 = 0
            then 0
            else 0
            // if y > config.gridSize / 2 && x > config.gridSize / 2 && (x - (y % 8)) % 4 = 0 then
            //     1
            // else
            //     0
|]
let setCoordinate x y value =
    map[(y * config.gridSize) + x] <- value
    
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
let outputRaycasts = Array.zeroCreate (config.widthPixels * config.heightPixels)
let steps = Array.zeroCreate 1000
let program = Shaders.Raycaster({
    config with
        cameraX = posX; cameraZ = posZ
        debugX = debugX
}, map, outputRaycasts, steps)
let debugPixelCpu () =
     program.main { x = uint debugX; y = uint debugY; z = 0u }
let renderImageOnCpu () =
     let runJob t q =
         for j in 1..config.heightPixels / q do
             for i in 1..config.widthPixels do
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
     for t in 0..q - 1 do
         // runJob t
         tasks.Add (task { runJob t q } :> Task)
          // task {
         // } :> Task
     Task.WaitAll (tasks.ToArray())
     createImage outputRaycasts

Environment.SetEnvironmentVariable("RUST_BACKTRACE", "full")
let wgpu = new WebGPU'(WebGPU.GetApi())
let result = Setup.compileModule Shaders.Raycaster
let code = Compiler.Print.module' result
let setup = wgpu.CreateBinder Shaders.Raycaster
let createCompute () =
    let (cfg, setup) = Wgpu.Bind setup
    let (grid, setup) = Wgpu.Bind setup (config.gridSize * config.gridSize)
    let (output, setup) = Wgpu.Map setup (config.heightPixels * config.widthPixels)
    let (steps, setup) = Wgpu.Map setup 1000
    printfn $"{code}"
    let compute = new Extensions.ComputePipeline("main", setup)
    let sw = Stopwatch()
    let runRaycasts x z =
        sw.Restart()
        // compute.Begin (uint (config.widthPixels / 64), 1u, 1u)
        compute.Begin (uint config.widthPixels, uint config.heightPixels / 64u, 1u)
            (fun encoder ->
                output.AddCopy(wgpu, encoder)
                steps.AddCopy(wgpu, encoder)
            )
            (fun queue ->
                cfg.Write (wgpu, queue, {
                    config with
                        cameraX = x; cameraZ = z
                        debugX = debugX
                })
                grid.Write (wgpu, queue, 0uL, map)
            )
        sw.Stop()
        let raycasts = output.ReadBufferRange(wgpu, 0, config.widthPixels * config.heightPixels).Result
        let steps = steps.ReadBufferRange(wgpu, 0, 1000).Result
        let elapsed = sw.ElapsedTicks
        let formatted = (double elapsed * 0.1).ToString("0.#") + "us"
        // printfn $"{raycasts}"
        printfn $""
        printfn $"{x}, {z} => {formatted}"
        steps |> Seq.takeWhile (fun s -> s.voxelX <> 0f) |> Seq.iter (printfn "%A")
        printfn $"{raycasts[(config.widthPixels * debugY) + debugX]}"
        raycasts
    runRaycasts
    
let gpuRaycasts = createCompute ()

let width = config.widthPixels
let height = config.heightPixels
//     for i in 0..config.widthPixels - 1 do
//         for j in 0u..639u do
//             program.main { x = uint i; y = j; z = 0u }
    
let t = task
type Interop =
    static let keyboard = ConcurrentDictionary()
    [<JSInvokable>]
    static member KeyboardInput(key: string, code: string, pressed: bool) =
        printfn $"{key} {code} {pressed}"
        keyboard[key] <- pressed
        keyboard[code] <- pressed
type App() as this =
    inherit Bolero.Component()
    
    let code = $$"""
function log(data) { console.log(data) }
let canvas = document.createElement('canvas'), ctx = canvas.getContext('2d')
canvas.width = {{ config.widthPixels }}
canvas.height = {{ config.heightPixels }}
let imgData = ctx.createImageData(canvas.width, canvas.height)
document.body.appendChild(canvas)
document.body.onkeydown = async e => {
   await DotNet.invokeMethodAsync('Raycast.Compute', 'KeyboardInput', e.key, e.code, true)
}
document.body.onkeyup = async e => {
   await DotNet.invokeMethodAsync('Raycast.Compute', 'KeyboardInput', e.key, e.code, false)
}

function drawImage(buffer) {
   debugger;
   imgData.data.set(buffer)
   ctx.putImageData(imgData, 0, 0)
}"""
    let useCpu = false
    [<Inject>]
    member val js = Unchecked.defaultof<IJSRuntime> with get, set
    
    member this.render useCpu =
        task {
            do! this.js.InvokeVoidAsync ("drawImage", createImage (gpuRaycasts posX posZ))
            debugPixelCpu ()
            if useCpu then
                task {
                    let image = renderImageOnCpu ()
                    do! this.js.InvokeVoidAsync ("drawImage", image)
                } |> ignore
        } :> Task
    
    
    override this.OnInitializedAsync() =
        task {
            do! this.js.InvokeVoidAsync ("eval", code)
            do! this.render false
        }
    
    override this.OnAfterRenderAsync _ = task { do! this.render false }

    override this.Render() =
        let asm = Assembly.GetExecutingAssembly()
        // let movementSpeed = 0.0625f / 32f
        let movementSpeed = 0.25f
        div {
            on.click (fun _ -> task { do! this.js.InvokeAsync ("log", [| 1; 2; 3; 4; |]) } |> ignore )
            input {
                attr.``type`` "text"
                on.change (fun e -> debugX <- Int32.Parse (string e.Value))
                attr.value debugX
            }
            button {
                on.click (fun _ ->
                    posX <- posX - movementSpeed
                    // this.render true |> ignore
                )
                "Left"
            }
            button {
                on.click (fun _ ->
                    posX <- posX + movementSpeed
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
                on.input (fun e ->
                    match Double.TryParse(string e.Value) with
                    | true, d -> posX <- float32 d
                    | _ -> ()
                    // printfn $"{string e.Value}"
                )
            }
            p {
                $"{posX}, {posZ}"
            }
        }
    
// ImageDisplay.showImage width height bytes'
Bolero.WebView.Program.startAppThread<App> ()
|> _.Join()