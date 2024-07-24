module Raycast.Compute.Program

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

open type Wgsl
[<ReflectedDefinition>]
module Shaders =
    type Config = {
        gridSize: int
        widthPixels: int; heightPixels: int
        cameraX: float32; cameraY: float32; cameraZ: float32
        voxelGridScale: float32
    }
    type [<Struct>] RaycastResult = {
        distance: float32; t: int
        resultX: float32; resultY: float32; resultZ: float32
        deltaX: float32
        deltaY: float32
        deltaZ: float32
        maskX: float32
        maskY: float32
        maskZ: float32
        posX: float32; posY: float32
        pixelX: uint
        pixelY: uint
    }
    type Raycaster(cfg: Config, grid: int[], output: RaycastResult[]) =
        let dda (rayPos: vec3f) rayDir =
            let mapPos = floor(rayPos + 0f);
            let len = length(rayDir)
            let deltaDist = abs(vec3(len) / rayDir)
            let mutable fixedDistance = vec3(0f)
            let epsilon = 0.00000001f
            if abs(rayDir.x) > epsilon then
                fixedDistance.x <- abs(len / rayDir.x)
            if abs(rayDir.y) > epsilon then
                fixedDistance.y <- abs(len / rayDir.y)
            if abs(rayDir.z) > epsilon then
                fixedDistance.z <- abs(len / rayDir.z)
            
            let rayStep = sign(rayDir)

            let sideDist = (sign(rayDir) * (mapPos - rayPos) + (sign(rayDir) * 0.5f) + 0.5f) * deltaDist; 
            
            let mask = lessThanEqual(sideDist, min(sideDist.yzx, sideDist.zxy))
            let maskf = vec3(float32(mask.x), float32(mask.y), float32(mask.z))
            let next = maskf * rayStep + mapPos
            let nextOffset = next - rayPos
            let pointDistances = nextOffset * fixedDistance * maskf
            let distance = max(abs(pointDistances.x), max(abs(pointDistances.y), abs(pointDistances.z)))
            let scaledOffset = rayDir * distance / length(rayDir)

            // let nextTileOffset = fixedDistance * ((mapPos + (maskf * rayStep)) - rayPos)
            // let nextTileDistance = max(nextTileOffset.x, max(nextTileOffset.y, nextTileOffset.z))
            // let offsetVec = nextTileDistance * rayDir / length(rayDir)
            let p = rayPos
            let toReturn = rayPos + scaledOffset
            toReturn
        // member this.raycast (pixelX, pixelY) =
        //     let screenWidthMeters = 1f
        //     let pcX = float32 pixelX / float32 cfg.widthPixels
        //     let pcY = float32 pixelY / float32 cfg.heightPixels
        //     let offsetX = (pcX - 0.5f) * screenWidthMeters
        //     let offsetY = (pcY - 0.5f) * screenWidthMeters
        //     let mutable rayhit = 0
        //     let mutable i = 0
        //     let mutable posX = offsetX + cfg.cameraX
        //     let mutable posY = offsetY + cfg.cameraY
        //     let mutable posZ = 1f + cfg.cameraZ
        //     let maxIterations = 4000
        //     while rayhit = 0 && i < maxIterations && 
        //           int posX < cfg.gridSize && 
        //           int posZ < cfg.gridSize do
        //         i <- i + 1
        //         posX <- (offsetX * 0.005f) + posX
        //         posY <- (offsetY * 0.005f) + posY
        //         posZ <- 0.005f + posZ
        //         let arrayIndex = (int posX) + (int posZ * cfg.gridSize)
        //         if posY > 1f then
        //             i <- maxIterations
        //         elif posY < -1f then
        //             i <- maxIterations
        //         elif grid[arrayIndex] <> 0 then
        //             rayhit <- grid[arrayIndex]
        //     let dx = posX - cfg.cameraX
        //     let dy = posY - cfg.cameraY
        //     {
        //         distance = sqrt((dx * dx) + (dy * dy))
        //         t = i
        //         dirX = offsetY - cfg.cameraX
        //         dirY = 1f
        //         dirZ = 0f
        //         pixelX = pixelX
        //         pixelY = pixelY
        //         posX = 0f
        //         posY = 0f
        //         resultX = 0f
        //         resultY = 0f
        //         resultZ = 0f
        //     }
        member this.raycast (pixelX, pixelY) =
            let screenWidthMeters = 1f
            let pcX = float32 pixelX / float32 cfg.widthPixels
            let pcY = float32 pixelY / float32 cfg.heightPixels
            let offsetX = (pcX - 0.5f) * screenWidthMeters
            let offsetY = (pcY + 0.5f) * screenWidthMeters
            let mutable rayhit = -1
            let mutable i = 0
            let dirY = ((float32 cfg.heightPixels * 0.5f) - float32 pixelY) / float32 cfg.heightPixels
            let dirX = (float32 pixelX - (float32 cfg.widthPixels / 2f)) / float32 cfg.widthPixels
            let mutable pos = vec3(dirX + cfg.cameraX, dirY + cfg.cameraY, 1f + cfg.cameraZ)
            let mutable deltaDir = vec3(0f)
            let mutable mask = vec3(0f)
            let dir = vec3(dirX, dirY, 1f)
            let maxIterations = 200
            while rayhit = -1 && i < maxIterations && 
                  int pos.x < cfg.gridSize && 
                  int pos.z < cfg.gridSize do
                i <- i + 1
                pos <- dda pos dir
                let arrayIndex = (int pos.x) + (int pos.z * cfg.gridSize)
                if pos.y > 1f then
                    i <- maxIterations
                    rayhit <- 0
                elif pos.y < -1f then
                    i <- maxIterations
                    rayhit <- 0
                elif arrayIndex < (cfg.gridSize * cfg.gridSize) && grid[arrayIndex] <> 0 then
                    rayhit <- grid[arrayIndex]
            let dx = pos.x - cfg.cameraX
            let dy = pos.y - cfg.cameraY
            {
                distance = sqrt((dx * dx) + (dy * dy))
                t = rayhit
                resultX = pos.x
                resultY = pos.y
                resultZ = pos.z
                pixelX = pixelX
                pixelY = pixelY
                posX = offsetX
                posY = offsetY
                deltaX = deltaDir.x
                deltaY = deltaDir.y
                deltaZ = deltaDir.z
                maskX = mask.x
                maskY = mask.y
                maskZ = mask.z
            }
        [<Compute; WorkgroupSize(1, 64, 1)>]
        member this.main([<BuiltIn(Builtin'.global_invocation_id)>] globalId: vec3<uint>) =
            let result = this.raycast (globalId.x, globalId.y)
            output[int globalId.x + (cfg.widthPixels * int globalId.y)] <- result

open Shaders

Environment.SetEnvironmentVariable("RUST_BACKTRACE", "full")
let wgpu = new WebGPU'(WebGPU.GetApi())
let result = Setup.compileModule Shaders.Raycaster
let code = Compiler.Print.module' result
let setup = wgpu.CreateBinder Shaders.Raycaster
let config = ({
    gridSize = 64; widthPixels = 800; heightPixels = 640
    cameraX = 64f / 2.0f
    cameraY = 0.5f
    cameraZ = 64f / 2.0f
    voxelGridScale = 1f 
} : Shaders.Config)
let (cfg, setup') = Wgpu.Bind setup
let (grid, setup'') = Wgpu.Bind setup' (config.gridSize * config.gridSize)
let (output, setup''') = Wgpu.Map setup'' (config.heightPixels * config.widthPixels)
let compute = new Extensions.ComputePipeline("main", setup''')
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
    
setCoordinate 33 34 1337
setCoordinate 33 38 1337
setCoordinate 33 41 1337
let runRaycasts x z =
    let sw = Stopwatch()
    sw.Start()
    // compute.Begin (uint (config.widthPixels / 64), 1u, 1u)
    compute.Begin (uint config.widthPixels, uint config.heightPixels / 64u, 1u)
        (fun encoder ->
            output.AddCopy(wgpu, encoder))
        (fun queue ->
            cfg.Write (wgpu, queue, { config with cameraX = x; cameraZ = z })
            grid.Write (wgpu, queue, 0uL, map)
        )
    sw.Stop()
    let raycasts = output.ReadBufferRange(wgpu, 0, config.widthPixels * config.heightPixels).Result
    let elapsed = sw.ElapsedMilliseconds
    printfn $"{raycasts}"
    raycasts

let width = config.widthPixels
let height = config.heightPixels
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
            if r.t > 0 then
                yield 0uy
                yield 255uy
                yield 255uy
                yield 255uy
            else
                yield 0uy
                yield 0uy
                yield 0uy
                yield 0uy
    |]
// let ray1 = raycasts[250000]
// let ray2 = raycasts[250001]
let testCoordinate (program: Shaders.Raycaster) x y =
     // let y = y / config.widthPixels
     // let x = x - (y * config.widthPixels)
     program.main { x = uint x; y = uint y; z = 0u }
// let ddaTest () =
     // let outputRaycasts = Array.zeroCreate (config.widthPixels * config.heightPixels)
     // let program = Shaders.Raycaster(config, map, outputRaycasts)
do // todo : testing on cpu
     let outputRaycasts = Array.zeroCreate (config.widthPixels * config.heightPixels)
     let program = Shaders.Raycaster(config, map, outputRaycasts)
     // let tasks = [|
     //     for t in 0..9 do
     //         task {
     //             for i in 1..config.widthPixels / 10 do
     //                 for j in 1..config.heightPixels / 10 do
     //                     testCoordinate program ((i - 1) * t * 10) ((j - 1) * t * 10)
     //         } :> Task
     // |]
     // Task.WaitAll tasks
     testCoordinate program 200 320
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
type App() =
    inherit Bolero.Component()
    
    let code = "\
function log(data) { console.log(data) }
let canvas = document.createElement('canvas'), ctx = canvas.getContext('2d')
canvas.width = 800
canvas.height = 640
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
}"

    let mutable posX = 32f
    let mutable posZ = 32f
    
    [<Inject>]
    member val js = Unchecked.defaultof<IJSRuntime> with get, set
    
    override this.OnInitializedAsync() =
        task {
            do! this.js.InvokeVoidAsync ("eval", code)
            do! this.js.InvokeVoidAsync ("drawImage", createImage (runRaycasts posX posZ))
        } :> Task

    override this.Render() =
        let asm = Assembly.GetExecutingAssembly()
        Bolero.Html.div {
            Bolero.Html.on.click (fun _ -> task { do! this.js.InvokeAsync ("log", [| 1; 2; 3; 4; |]) } |> ignore )
            Bolero.Html.on.click (fun _ -> (task {
                posX <- posX + 0.1f
                do! this.js.InvokeVoidAsync ("drawImage", createImage (runRaycasts posX posZ))
            } |> ignore))
            "Hello world"
        }
    
// ImageDisplay.showImage width height bytes'
Bolero.WebView.Program.startAppThread<App> ()
|> _.Join()