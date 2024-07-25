module WebGPU
open System
open System.Runtime.InteropServices
open System.Threading.Tasks
open Dootverse.WebGPU
open Dootverse.WebGPU.Wgsl
open Silk.NET.Input
open Microsoft.FSharp.NativeInterop
open Silk.NET.Core.Native
open Silk.NET.Maths
open Silk.NET.WebGPU
open Silk.NET.Windowing
open System.Collections.Generic

type Ptr =
    static member inline arrayPtr (values: 't[]) =
        use ptr = fixed values
        ptr
    static member inline ptr<'t when 't : unmanaged> (value: 't) =
        use ptr = fixed [| value |]
        ptr
        
open type Ptr

// let mutable shaderModule = Unchecked.defaultof<nativeptr<ShaderModule>>
let mutable posX = 0f
let mutable posY = 0f
let keys = Dictionary()
for i in 1..100 do
    keys[i] <- false
let binding0Size = 20uL
let createRender (wgpu: WebGPU) device (surfaceCapabilities: SurfaceCapabilities) layout shaderModule =
    let blendState = BlendState(
        Color = BlendComponent(
            SrcFactor = BlendFactor.One,
            DstFactor = BlendFactor.Zero,
            Operation = BlendOperation.Add
        ),
        Alpha = BlendComponent(
            SrcFactor = BlendFactor.One,
            DstFactor = BlendFactor.Zero,
            Operation = BlendOperation.Add
        )
    )
    let colorTargetState = ColorTargetState(
        Format = NativePtr.read surfaceCapabilities.Formats,
        Blend = arrayPtr [| blendState |],
        WriteMask = ColorWriteMask.All
    )
    let fragmentState = FragmentState(
        Module = shaderModule,
        TargetCount = unativeint 1,
        Targets = arrayPtr [| colorTargetState |],
        EntryPoint = C.string "fragment"
    )
    let renderPipelineDescriptor = RenderPipelineDescriptor(
        Vertex = VertexState(
            Module = shaderModule,
            EntryPoint = C.string "vertex"
            // BufferCount = unativeint 1
        ),
        Primitive = PrimitiveState(
            Topology = PrimitiveTopology.TriangleList,
            StripIndexFormat = IndexFormat.Undefined,
            FrontFace = FrontFace.Ccw,
            CullMode = CullMode.None
        ),
        Multisample = MultisampleState(
            Count = 1u,
            Mask = ~~~0u,
            AlphaToCoverageEnabled = false
        ),
        // Fragment = &&fragmentState,
        Fragment = ptr fragmentState,
        DepthStencil = Unchecked.defaultof<_>,
        // Layout = wgpu.CreatePipelineLayout(device, [| group.bindGroupLayout |])
        Layout = layout
    )
    wgpu.CreateRenderPipeline(device, renderPipelineDescriptor)
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
    let shaderModule = wgpu.CreateShader(wgpu.Device.Device, bindings.Info.code)
    let groups = wgpu.InitBindings ShaderStage.Fragment wgpu.Device.Device bindings.Buffers
    let swap () =
        // let mutable surfaceConfig = Operators.Unchecked.defaultof<_>
        // Create swap
        let mutable surfaceConfig = SurfaceConfiguration(
            Usage = TextureUsage.RenderAttachment,
            Format = NativePtr.read surfaceCapabilities.Formats,
            PresentMode = PresentMode.Fifo,
            Device = wgpu.Device.Device,
            Width = uint window.FramebufferSize.X,
            Height = uint window.FramebufferSize.Y
        )
        wgpu.SurfaceConfigure(surface, &surfaceConfig)
    let renderPipeline =
        let layout = wgpu.CreatePipelineLayout(wgpu.Device.Device, [| groups.layout |])
        let result = createRender wgpu wgpu.Device.Device surfaceCapabilities layout shaderModule
        swap ()
        result
    do
        window.add_Closing(fun () ->
            wgpu.ShaderModuleRelease(shaderModule)
            wgpu.RenderPipelineRelease(renderPipeline)
            wgpu.DeviceRelease(wgpu.Device.Device)
            wgpu.AdapterRelease(wgpu.Adapter)
            wgpu.SurfaceRelease(surface)
            wgpu.InstanceRelease(wgpu.Instance)
            wgpu.Dispose())
    let onFramebufferResize (size: Vector2D<int>) =
        windowWidth <- size.X
        windowHeight <- size.Y
        swap ()
    do
        window.add_FramebufferResize onFramebufferResize
    do            
        let input = window.CreateInput()
        let onKeyDown (keyboard: IKeyboard) (key: Key) (code: int) = 
            keys[int key] <- true
        let onKeyUp keyboard key code = 
            keys[int key] <- false
        let onMouseMove mouse movement = ()
        let onMouseDown mouse button = ()
        let onMouseUp mouse button = ()
        input.Keyboards |> Seq.iter (fun k -> k.add_KeyDown onKeyDown)
        input.Keyboards |> Seq.iter (fun k -> k.add_KeyUp onKeyUp)
        input.Mice |> Seq.iter (fun m -> m.add_MouseMove onMouseMove)
        input.Mice |> Seq.iter (fun m -> m.add_MouseDown onMouseDown)
        input.Mice |> Seq.iter (fun m -> m.add_MouseUp onMouseDown)
    member this.SurfaceCapabilities = surfaceCapabilities
    member this.Surface = surface
    member this.RenderPipeline = renderPipeline
    member this.Window = window
    member this.onRender value =
        window.add_Render (fun t ->
            let mutable texture = SurfaceTexture()
            wgpu.SurfaceGetCurrentTexture(surface, &&texture)
            match texture.Status with
            | SurfaceGetCurrentTextureStatus.Timeout
            | SurfaceGetCurrentTextureStatus.Lost
            | SurfaceGetCurrentTextureStatus.Outdated ->
                wgpu.TextureRelease(texture.Texture)
                swap ()
            | SurfaceGetCurrentTextureStatus.OutOfMemory
            | SurfaceGetCurrentTextureStatus.DeviceLost
            | SurfaceGetCurrentTextureStatus.Force32 ->
                failwith "Error"
            | SurfaceGetCurrentTextureStatus.Success -> ()
            | _ -> ()
            let view = wgpu.TextureCreateView(texture.Texture, Unchecked.defaultof<nativeptr<_>>)
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
type Output = {
    [<Location(0)>] xy: vec2<float32>
    [<BuiltIn(Builtin'.position)>] position: vec4<float32>
}
[<ReflectedDefinition>]
type Shader(data: int[], output: int[]) =
    [<Wgsl.Vertex>]
    member this.vertex([<BuiltIn(Builtin'.vertex_index)>] index: uint) =
        let pos = [|
            vec2(-1f, 1f)
            vec2(-1f, -1f)
            vec2(1f, -1f)
            
            vec2(1f, 1f)
            vec2(-1f, 1f)
            vec2(1f, -1f)
        |]
        let n = int index
        {
            xy = vec2(pos[n].x,pos[n].y)
            position = vec4(pos[n], 0f, 1f)
        }
    [<Fragment; Location 0>]
    member this.fragment(vertexOutput: Output) =
        vec4(0.05f, vertexOutput.xy.x, vertexOutput.xy.y, 1f)
let yo' (window: IWindow) =
    let wgpu = new WebGPU'(window)
    let state = wgpu.CreateBinder Shader
    let (voxels, state) = Wgpu.Bind state 10
    let (output, state) = Wgpu.Map state 10
    let win = WebGpuWin(window, state)
    let wgpu = win.Wgpu
    
    let mutable time = 0.
    
    win.onRender <| fun view t ->
        time <- time + t
        if keys[int Key.A] then
            posX <- posX - 10f * float32 t
        if keys[int Key.D] then
            posX <- posX + 10f * float32 t
        if keys[int Key.W] then
            posY <- posY + 10f * float32 t
        if keys[int Key.S] then
            posY <- posY - 10f * float32 t

        let colorAttachment = RenderPassColorAttachment(
            View = view,
            ResolveTarget = Unchecked.defaultof<_>,
            LoadOp = LoadOp.Clear,
            StoreOp = StoreOp.Store,
            ClearValue = Color(0, 1, 0, 1)
        )
        
        let encoder = wgpu.Device.CreateCommandEncoder ()
        let queue = wgpu.Device.GetQueue ()
        let renderPass = encoder.StartRenderPass' (colorAttachment)
        renderPass.SetPipeline win.RenderPipeline
        renderPass.SetBindGroup win.BindGroup 0u
        
        voxels.Write (wgpu, queue, 0uL, [| 1; 2; 3; 4; 5; 6; 7; 8; 0; 11 |])
        
        renderPass.Draw 6u 2u 0u 0u
        renderPass.End ()
        let buffer = arrayPtr [| encoder.Finish() |]
        wgpu.QueueSubmit (queue, unativeint 1, buffer)
        wgpu.SurfacePresent (win.Surface)
        wgpu.CommandBufferRelease (NativePtr.read buffer)
        encoder.Release ()

let mutable options = WindowOptions.Default
options.API <- GraphicsAPI.None
options.Size <- Vector2D(1920, 1080)
options.FramesPerSecond <- 60
options.UpdatesPerSecond <- 60
options.Position <- Vector2D(400, 400)
options.Title <- "WebGPU Demo"
options.IsVisible <- true
options.ShouldSwapAutomatically <- false
options.IsContextControlDisabled <- false
// let window = Window.Create options
let window = Window.Create options
window.add_Load (fun () -> yo' window)
window.Run()
// yo' window