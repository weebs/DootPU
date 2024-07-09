module Dootverse.WebGPU.Main

open Dootverse.WebGPU.Setup
open System.Runtime.InteropServices
open Dootverse.WebGPU
open Silk.NET.Input
open Microsoft.FSharp.NativeInterop
open Silk.NET.Core.Native
open Silk.NET.Maths
open Silk.NET.WebGPU
open Silk.NET.Windowing
open System.Collections.Generic

let gridSize = 22
let memSize = uint64 (gridSize * gridSize * 4 * sizeof<float32>)
let shapes = [|
    for i in 1..(gridSize * gridSize) do
        0f; 0f; 0f; 0f
|]
type [<Struct>] output = { mutable color: int }

type QuoteBuilder() =
    // member this.Zero (a: 'a) = Operators.Unchecked.defaultof<'a>
    // member this.Yield (a: 'a) = Operators.Unchecked.defaultof<'a>
    member this.Return<'a> (a: 'a) = a
    member this.Quote () = ()
let wgsl = QuoteBuilder()
let foo = wgsl {
    let x = 1234
    let foo2 = wgsl { return 1 }
    return x
}
let _return: Quotations.Expr<obj -> obj> = <@ wgsl.Return @>
type Module =
    static member Foo (struct (a: int32[], b: float32[])) = <@
        0
    @>
let asdf = <@ Module.Foo  @>
let asdf' = Module.Foo
let _1 (value: 'a) (fn: Quotations.Expr<'a -> 'b>) = Operators.Unchecked.defaultof<'a -> 'b>
let foo' = _1 "" <@ _.ToLower() @>
let shader =
    // System.IO.File.ReadAllText(System.IO.Path.Join(__SOURCE_DIRECTORY__, "raymarching.wgsl"))
    // + "\n" +
    """struct output {
    @builtin(position) position: vec4f,
    @location(0) xy: vec2f,
};""" +
    Wgsl.output
let shaderStructs = Wgsl.gatherStructs Wgsl.shader'
printfn $"{shader}"
type Main() =
    let mutable windowWidth = 1920
    let mutable windowHeight = 1080
    let mutable options = WindowOptions.Default
    do
        options.API <- GraphicsAPI.None
        options.Size <- Vector2D(windowWidth, windowHeight)
        options.FramesPerSecond <- 60
        options.UpdatesPerSecond <- 60
        options.Position <- Vector2D(400, 400)
        options.Title <- "WebGPU Demo"
        options.IsVisible <- true
        options.ShouldSwapAutomatically <- false
        options.IsContextControlDisabled <- false
    let mutable wgpu = Unchecked.defaultof<WebGPU>
    let mutable nativeInstance = Unchecked.defaultof<nativeptr<Instance>>
    let mutable surface = Unchecked.defaultof<nativeptr<Surface>>
    let mutable surfaceConfig = Unchecked.defaultof<SurfaceConfiguration>
    let mutable surfaceCapabilities = Unchecked.defaultof<SurfaceCapabilities>
    let mutable adapter = Unchecked.defaultof<nativeptr<Adapter>>
    let mutable device = Unchecked.defaultof<nativeptr<Device>>
    let mutable shaderModule = Unchecked.defaultof<nativeptr<ShaderModule>>
    let mutable renderPipeline = Unchecked.defaultof<nativeptr<RenderPipeline>>
    // let mutable changingVertexBuffer = Unchecked.defaultof<nativeptr<_>>
    let mutable uniformBuffer = Unchecked.defaultof<nativeptr<_>>
    let mutable circlesBuffer = Unchecked.defaultof<nativeptr<_>>
    let mutable bindGroup = Unchecked.defaultof<_>
    let window = Window.Create options
    let mutable posX = 0f
    let mutable posY = 0f
    let keys = Dictionary()
    do
        for i in 1..100 do
            keys[i] <- false
    let swap () =
        // Create swap
        surfaceConfig <- new SurfaceConfiguration(
            Usage = TextureUsage.RenderAttachment,
            Format = NativePtr.read surfaceCapabilities.Formats,
            PresentMode = PresentMode.Fifo,
            Device = device,
            Width = uint window.FramebufferSize.X,
            Height = uint window.FramebufferSize.Y
        )
        wgpu.SurfaceConfigure(surface, &surfaceConfig)
    let binding0Size = 20uL
    let onWindowLoad () =
        do // Setup input
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
        do // Setup surface variable
            wgpu <- WebGPU.GetApi ()
            let descriptor = InstanceDescriptor()
            nativeInstance <- wgpu.CreateInstance(&descriptor)
            surface <- window.CreateWebGPUSurface(wgpu, nativeInstance)
        do // get adapter
            let request = RequestAdapterOptions(CompatibleSurface = surface)
            let callback = new PfnRequestAdapterCallback(RequestAdapterCallback(fun _ adapter1 _ _ ->
                adapter <- adapter1
            ))
            wgpu.InstanceRequestAdapter(nativeInstance, &request, callback, Unchecked.defaultof<_>)
            wgpu.SurfaceGetCapabilities(surface, adapter, &surfaceCapabilities)
        do // Get device
            let callback = new PfnDeviceLostCallback(DeviceLostCallback(fun reason arg1 arg2 -> ()))
            let descriptor = DeviceDescriptor(DeviceLostCallback = callback)
            let callback = new PfnRequestDeviceCallback(RequestDeviceCallback(fun _ device1 _ _ -> device <- device1))
            wgpu.AdapterRequestDevice(adapter, &descriptor, callback, Unchecked.defaultof<_>)
        // Setup shader module
        let shaderModule =
            wgpu.CreateShader(device, shader)
        let mutable blendState = BlendState(
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
        let mutable colorTargetState = ColorTargetState(
            Format = NativePtr.read surfaceCapabilities.Formats,
            Blend = &&blendState,
            WriteMask = ColorWriteMask.All
        )
        let mutable fragmentState = FragmentState(
            Module = shaderModule,
            TargetCount = unativeint 1,
            Targets = &&colorTargetState,
            // EntryPoint = NativePtr.ofNativeInt (SilkMarshal.StringToPtr("fs_main"))
            EntryPoint = NativePtr.ofNativeInt (SilkMarshal.StringToPtr("fragment"))
        )
        // let vbLayout = VertexBufferLayout(
        //     
        // )
        let bindGroupLayout = BindGroupLayoutEntry(
            Binding = 0u,
            Visibility = (ShaderStage.Vertex ||| ShaderStage.Fragment),
            Buffer = BufferBindingLayout(
                Type = BufferBindingType.Uniform,
                MinBindingSize = binding0Size
            )
        )
        let arrayLayout = 
            BindGroupLayoutEntry(
                Binding = 1u,
                Visibility = (ShaderStage.Fragment),
                Buffer = BufferBindingLayout(
                    Type = BufferBindingType.Storage,
                    MinBindingSize = uint64 memSize
                )
            )
        let mutable bindGroupLayout = 
            wgpu.CreateBindGroupLayout(device, [|
                bindGroupLayout
                arrayLayout
            |])
        let mutable layoutDesc = PipelineLayoutDescriptor(
            BindGroupLayoutCount = unativeint 1,
            // BindGroupLayouts = &&bindGroupLayout
            BindGroupLayouts = &&bindGroupLayout
        )
        let renderPipelineDescriptor = RenderPipelineDescriptor(
            Vertex = VertexState(
                Module = shaderModule,
                EntryPoint = NativePtr.ofNativeInt (SilkMarshal.StringToPtr("vertex"))
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
            Fragment = &&fragmentState,
            DepthStencil = Unchecked.defaultof<_>,
            // Layout = layout
            Layout = wgpu.DeviceCreatePipelineLayout(device, &&layoutDesc)
        )
        try
            renderPipeline  <- wgpu.DeviceCreateRenderPipeline(device, &renderPipelineDescriptor)
            do // Setup bindings
                let mutable uniformBufferDesc = BufferDescriptor(
                    Size = binding0Size,
                    Usage = (BufferUsage.CopyDst ||| BufferUsage.Uniform)
                )
                let mutable circlesBufferDesc = BufferDescriptor(
                    Size = memSize,
                    Usage = (BufferUsage.Storage ||| BufferUsage.CopyDst)
                )
                uniformBuffer <- wgpu.DeviceCreateBuffer(device, &&uniformBufferDesc)
                circlesBuffer <- wgpu.DeviceCreateBuffer(device, &&circlesBufferDesc)
                let bindings = [|
                    BindGroupEntry(
                        Binding = 0u,
                        Buffer = uniformBuffer,
                        Offset = 0uL,
                        Size = binding0Size
                    )
                    BindGroupEntry(
                        Binding = 1u,
                        Buffer = circlesBuffer,
                        Offset = 0uL,
                        Size = memSize
                        // Size = 16uL
                    )
                |]
                bindGroup <- wgpu.CreateBindGroup(device, bindGroupLayout, bindings)
            // changingVertexBuffer <- wgpu.DeviceCreateBuffer(device, &&desc)
            swap ()
        with error ->
            printfn $"{error}"
            reraise ()
        
    let onWindowClosing () =
        wgpu.ShaderModuleRelease(shaderModule)
        wgpu.RenderPipelineRelease(renderPipeline)
        wgpu.DeviceRelease(device)
        wgpu.AdapterRelease(adapter)
        wgpu.SurfaceRelease(surface)
        wgpu.InstanceRelease(nativeInstance)
        wgpu.Dispose()
    let mutable time = 0.0

    // type Window =
    //     abstract member Render : float -> unit
    //     abstract member Update : float -> unit
    let handleStatus (texture: SurfaceTexture) =
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
    let onUpdate t =
        if keys[int Key.A] then
            posX <- posX - 10f * float32 t
        if keys[int Key.D] then
            posX <- posX + 10f * float32 t
        if keys[int Key.W] then
            posY <- posY + 10f * float32 t
        if keys[int Key.S] then
            posY <- posY - 10f * float32 t
    let onWindowRender t =
        time <- time + t

        let texture = wgpu.GetCurrentTexture surface
        handleStatus texture
        
        let view = wgpu.TextureCreateView(texture.Texture, Unchecked.defaultof<nativeptr<_>>)
        
        let encoderDescriptor = CommandEncoderDescriptor()
        let encoder = wgpu.DeviceCreateCommandEncoder(device, &encoderDescriptor)
        let mutable colorAttachment = RenderPassColorAttachment(
            View = view,
            ResolveTarget = Unchecked.defaultof<_>,
            LoadOp = LoadOp.Clear,
            StoreOp = StoreOp.Store,
            ClearValue = Color(0, 1, 0, 1)
        )
        let renderPassDescriptor = RenderPassDescriptor(
            ColorAttachments = &&colorAttachment,
            ColorAttachmentCount = unativeint 1,
            DepthStencilAttachment = Unchecked.defaultof<nativeptr<RenderPassDepthStencilAttachment>>
        )
        let mutable queue = wgpu.DeviceGetQueue(device)
        let renderPass = wgpu.CommandEncoderBeginRenderPass(encoder, &renderPassDescriptor)
        wgpu.RenderPassEncoderSetPipeline(renderPass, renderPipeline)
        wgpu.RenderPassEncoderSetBindGroup(renderPass, 0u, bindGroup, unativeint 0, Unchecked.defaultof<nativeptr<_>>)
        // wgpu.RenderPassEncoderSetBindGroup(renderPass, 0u, bindGroup, unativeint 0, Unchecked.defaultof<nativeptr<_>>)
        
        let screen = [|
            // float32 time
            float32 0
            float32 posX
            float32 posY
            float32 windowWidth
            float32 windowHeight
        |]
        use data = fixed screen
        let intptr = NativePtr.ofNativeInt<int32> (NativePtr.toNativeInt data)
        NativePtr.write intptr gridSize
        // let shapes' = Marshal.AllocHGlobal memSize |> NativePtr.ofNativeInt<float32>
        for i in 0..gridSize - 1 do
            for j in 0..gridSize - 1 do
                // let i = (x + (gridSize / 2))
                // let j = (y + (gridSize / 2))
                let index = 4 * ((i * gridSize) + j)
                // NativePtr.set shapes' index (posX + (float32 i * 20.0f))
                // NativePtr.set shapes' (index + 1) (posY + (float32 j * 20.0f))
                // NativePtr.set shapes index (uint32 posX + uint32 i)
                // NativePtr.set shapes (index + 1) (uint32 posY + uint32 j)
                // NativePtr.set shapes index (float32 time)
                // NativePtr.set shapes (index + 1) (float32 time)
                // NativePtr.set shapes' (index + 2) (float32 time)
                // NativePtr.set shapes' (index + 3) (float32 time)
                
                
                shapes[index] <- (posX + (float32 i * 0.2f))
                shapes[index + 1] <- (posY + (float32 j * 0.2f))
                shapes[index + 2] <- float32 time
                shapes[index + 3] <- float32 time
        // for n in -(gridSize * gridSize * 4 / 2)..(gridSize * gridSize * 4 / 2) - 1 do
        //     let x = n / gridSize
        //     let y = n % gridSize
        //     let index = n + (gridSize * gridSize * 4 / 2)
        //     NativePtr.set shapes index (float32 time)
        // use shapes = fixed [|
        //     // for i in -gridSize / 2 .. gridSize / 2 do
        //     //     for j in -gridSize / 2 .. gridSize / 2 do
        //     //         float32 time; 0f; 0f; 0f
        //     // for n in -(gridSize * gridSize / 2)..(gridSize * gridSize / 2) - 1 do
        //         // let i = n / gridSize
        //         // let j = n % gridSize
        //     // for i in 1..16 do
        //         // for j in 1..16 do
        //         // posX * 10.0f + (float32 i * 10.0f)
        //         // posY * 10.0f + (float32 j + 10.0f)
        //         // posX * 10.0f + (float32 i * 10.0f)
        //         // posY * 10.0f + (float32 j + 10.0f)
        //         // float32 time; 0f; 0f; 0f
        //         // 40f; 0f; 0f; 0f;
        //         // posY
        //         // posX
        //         // posY
        //     for i in 1..(gridSize * gridSize) do
        //         float32 time;
        // |]
        //
        do
            use shapes = fixed shapes
            wgpu.QueueWriteBuffer(queue, uniformBuffer, 0uL, data |> NativePtr.toVoidPtr, unativeint binding0Size)
            wgpu.QueueWriteBuffer(queue, circlesBuffer, 0uL, shapes |> NativePtr.toVoidPtr, unativeint memSize)
            
            wgpu.RenderPassEncoderDraw(renderPass, 6u, 2u, 0u, 0u)
            wgpu.RenderPassEncoderEnd(renderPass)
            let cbd = CommandBufferDescriptor()
            // let mutable buffer = wgpu.CommandEncoderFinish(encoder, &cbd)
            use buffers = fixed [| wgpu.CommandEncoderFinish(encoder, &cbd) |]
            // wgpu.QueueSubmit(queue, unativeint 1, &&buffer)
            wgpu.QueueSubmit(queue, unativeint 1, buffers)
            wgpu.SurfacePresent(surface)
            wgpu.CommandBufferRelease(NativePtr.read buffers)
            wgpu.CommandEncoderRelease(encoder)
            wgpu.TextureViewRelease(view)
            wgpu.TextureRelease(texture.Texture)
            Marshal.FreeHGlobal (NativePtr.toNativeInt shapes)
        
    let onFramebufferResize (size: Vector2D<int>) =
        windowWidth <- size.X
        windowHeight <- size.Y
        swap ()
    do    
        window.add_Load onWindowLoad
        window.add_Closing onWindowClosing
        window.add_Update onUpdate
        window.add_Render onWindowRender
        window.add_FramebufferResize onFramebufferResize
        window.Run()
        
let main = Main ()