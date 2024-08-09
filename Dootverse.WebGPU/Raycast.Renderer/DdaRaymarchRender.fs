module Raycast.Renderer.DdaRaymarchRender

open System
open System.Runtime.InteropServices
open System.Text
open System.Threading.Tasks
open Dootverse.WebGPU
open Dootverse.WebGPU.Shaders
open Dootverse.WebGPU.Wgsl
open Raycast.Compute.ComputeShaders.Shaders
open Silk.NET.Input
open Microsoft.FSharp.NativeInterop
open Silk.NET.Core.Native
open Silk.NET.Maths
open Silk.NET.WebGPU
open Silk.NET.Windowing
open System.Collections.Generic

// let mutable shaderModule = Unchecked.defaultof<nativeptr<ShaderModule>>
let mutable posX = 4f
let mutable posZ = 0f
let mutable posY = 11f
let keys = Dictionary()

keys[int Key.Space] <- false
keys[int Key.ShiftLeft] <- false
for i in 1..100 do
    keys[i] <- false

let binding0Size = 20uL

// [<AbstractClass>]
type WebGpuWin(window: IWindow, bindings: ShaderWithBindings) as this =
    let mutable windowWidth = 1920
    let mutable windowHeight = 1080
    let wgpu = bindings.Info.wgpu
    // let wgpu = wgpu
    let surface = wgpu.Surface

    let surfaceCapabilities =
        let mutable result = Operators.Unchecked.defaultof<_>
        wgpu.SurfaceGetCapabilities(surface, wgpu.Adapter, &result)
        result

    let shaderModule =
        wgpu.CreateShader(wgpu.Device.Device, bindings.Info.code)

    let groups =
        wgpu.InitBindings
            ShaderStage.Fragment
            wgpu.Device.Device
            bindings.Buffers

    let swap () =
        // let mutable surfaceConfig = Operators.Unchecked.defaultof<_>
        // Create swap
        let mutable surfaceConfig =
            SurfaceConfiguration(
                Usage = TextureUsage.RenderAttachment,
                Format = NativePtr.read surfaceCapabilities.Formats,
                PresentMode = PresentMode.Fifo,
                Device = wgpu.Device.Device,
                Width = uint window.FramebufferSize.X,
                Height = uint window.FramebufferSize.Y
            )

        wgpu.SurfaceConfigure(surface, &surfaceConfig)

    let renderPipeline =
        let layout =
            wgpu.CreatePipelineLayout(
                wgpu.Device.Device,
                [| groups.layout |]
            )

        let result =
            Init.createRender
                wgpu
                wgpu.Device.Device
                surfaceCapabilities
                layout
                shaderModule

        swap()
        result

    do
        window.add_Closing(fun () ->
            wgpu.ShaderModuleRelease(shaderModule)
            wgpu.RenderPipelineRelease(renderPipeline)
            wgpu.DeviceRelease(wgpu.Device.Device)
            wgpu.AdapterRelease(wgpu.Adapter)
            wgpu.SurfaceRelease(surface)
            wgpu.InstanceRelease(wgpu.Instance)
            wgpu.Dispose()
        )

    let onFramebufferResize (size: Vector2D<int>) =
        windowWidth <- size.X
        windowHeight <- size.Y
        swap()

    do window.add_FramebufferResize onFramebufferResize

    do
        let input = window.CreateInput()

        let onKeyDown (keyboard: IKeyboard) (key: Key) (code: int) =
            keys[int key] <- true

        let onKeyUp keyboard key code = keys[int key] <- false
        let onMouseMove mouse movement = ()
        let onMouseDown mouse button = ()
        let onMouseUp mouse button = ()
        input.Keyboards |> Seq.iter(fun k -> k.add_KeyDown onKeyDown)
        input.Keyboards |> Seq.iter(fun k -> k.add_KeyUp onKeyUp)
        input.Mice |> Seq.iter(fun m -> m.add_MouseMove onMouseMove)
        input.Mice |> Seq.iter(fun m -> m.add_MouseDown onMouseDown)
        input.Mice |> Seq.iter(fun m -> m.add_MouseUp onMouseDown)

    member this.SurfaceCapabilities = surfaceCapabilities
    member this.Surface = surface
    member this.RenderPipeline = renderPipeline
    member this.Window = window

    member this.onRender value =
        window.add_Render(fun t ->
            let mutable texture = SurfaceTexture()
            wgpu.SurfaceGetCurrentTexture(surface, &&texture)

            match texture.Status with
            | SurfaceGetCurrentTextureStatus.Timeout
            | SurfaceGetCurrentTextureStatus.Lost
            | SurfaceGetCurrentTextureStatus.Outdated ->
                wgpu.TextureRelease(texture.Texture)
                swap()
            | SurfaceGetCurrentTextureStatus.OutOfMemory
            | SurfaceGetCurrentTextureStatus.DeviceLost
            | SurfaceGetCurrentTextureStatus.Force32 -> failwith "Error"
            | SurfaceGetCurrentTextureStatus.Success -> ()
            | _ -> ()

            let view =
                wgpu.TextureCreateView(
                    texture.Texture,
                    Unchecked.defaultof<nativeptr<_>>
                )

            value view t
            wgpu.TextureViewRelease(view)
            wgpu.TextureRelease(texture.Texture)
        )


    member this.Groups = groups
    member this.BindGroup = groups.bindGroup
    // let renderPipeline = this.Init
    member this.Wgpu = wgpu
// abstract member Init : nativeptr<RenderPipeline>
// let vbLayout = VertexBufferLayout(
//
// )
// type yo' (window: IWindow, wgpu: WebGPU', shaderCode, shader) as this =
open type Wgsl
open Raycast.Compute.ComputeShaders
open System.Diagnostics

let gridSize = Settings.gridSize
let sw = Stopwatch()
let mutable frame = 0
sw.Start()
let init (window: IWindow) =
    // let gridSize = 200
    let map = Raycast.Compute.Program.createMap 20000
    let wgpu = new WebGPU'(window)
    // let state = wgpu.CreateBinder Dda.Raymarcher
    let state = wgpu.CreateBinder Dda.Raycaster
    let config = Raycast.Compute.Program.config
    let (cfg, state) = Wgpu.Bind state
    let (voxelGrid, state) = Wgpu.Bind state map.voxels.Length
    let (hashes, state) = Wgpu.Bind state map.hashes.Length
    let (ids, state) = Wgpu.Bind state map.ids.Length
    // let (objects, state) = Wgpu.Bind state map.objects.Length
    let (objects, state) = Wgpu.Bind state map.objects.Length
    let (shapes, state) =
        Wgpu.Bind (state, Raycast.Compute.Program.serializeObject) map.shapes.Length
    let (output, state) =
        Wgpu.Map state (config.widthPixels * config.heightPixels)
    let (dbg, state) =
        Wgpu.Map state (config.widthPixels * config.heightPixels * 10)
        
    let mutable wroteMap = false
    let win = WebGpuWin(window, state)
    let wgpu = win.Wgpu
    printfn $"{state.Info.code}"

    let mutable time = 0.

    win.onRender (fun view t ->
        // sw.Stop()
        sw.Start()
        time <- time + t

        if keys[int Key.A] then
            posX <- posX - 1f * float32 t

        if keys[int Key.D] then
            posX <- posX + 1f * float32 t

        if keys[int Key.W] then
            posZ <- posZ + 1f * float32 t

        if keys[int Key.S] then
            posZ <- posZ - 1f * float32 t
            
        if keys[int Key.Space] then
            posY <- posY + 1f * float32 t
            
        if keys[int Key.ShiftLeft] then
            posY <- posY - 1f * float32 t

        let colorAttachment =
            RenderPassColorAttachment(
                View = view,
                ResolveTarget = Unchecked.defaultof<_>,
                LoadOp = LoadOp.Clear,
                StoreOp = StoreOp.Store,
                ClearValue = Color(0, 1, 0, 1)
            )

        let encoder = wgpu.Device.CreateCommandEncoder()
        let queue = wgpu.Device.GetQueue()
        let renderPass = encoder.StartRenderPass'(colorAttachment)
        renderPass.SetPipeline win.RenderPipeline
        renderPass.SetBindGroup win.BindGroup 0u

        cfg.Write (wgpu, queue, { config with cameraX = posX; cameraY = posY; cameraZ = posZ })
        if not wroteMap then
            wroteMap <- true
            
            voxelGrid.Write(wgpu, queue, 0uL, map.voxels)
            hashes.Write(wgpu, queue, 0uL, map.hashes)
            ids.Write(wgpu, queue, 0uL, map.ids)
            objects.Write(wgpu, queue, 0uL, map.objects)
            shapes.Write(wgpu, queue, 0uL, map.shapes)
        // voxels.Write(wgpu, queue, 0uL, [| 1; 2; 3; 4; 5; 6; 7; 8; 0; 11 |])

        // Vertex indicies for fragment shader
        renderPass.Draw 6u 2u 0u 0u
        renderPass.End()
        let buffer = arrayPtr [| encoder.Finish() |]
        wgpu.QueueSubmit(queue, unativeint 1, buffer)
        wgpu.SurfacePresent(win.Surface)
        wgpu.CommandBufferRelease(NativePtr.read buffer)
        encoder.Release()
        frame <- frame + 1
        let a = wgpu.DevicePoll(wgpu.Device.Device, true)
        sw.Stop()
        if frame % 10 = 0 then
            printfn $"{float32 sw.ElapsedMilliseconds * 0.1f}"
            sw.Reset()
    )

let run () =
    let mutable options = WindowOptions.Default
    options.API <- GraphicsAPI.None
    options.Size <- Vector2D(400, 300)
    // options.Size <- Vector2D(1920, 1080)
    // options.Size <- Vector2D(960, 720)
    // options.Size <- Vector2D(480, 360)
    options.FramesPerSecond <- 60
    options.UpdatesPerSecond <- 60
    options.Position <- Vector2D(400, 400)
    options.Title <- "WebGPU Demo"
    options.IsVisible <- true
    options.ShouldSwapAutomatically <- true
    options.IsContextControlDisabled <- false
    // let window = Window.Create options
    let window = Window.Create options
    window.add_Load(fun () -> init window)
    window.Run()
    // yo' window
