module WebGPU
open System.Runtime.InteropServices

open Microsoft.FSharp.NativeInterop
open Silk.NET.Core.Native
open Silk.NET.Maths
open Silk.NET.WebGPU
open Silk.NET.Windowing

let mutable options = WindowOptions.Default
options.API <- GraphicsAPI.None
options.Size <- Vector2D(640, 480)
options.FramesPerSecond <- 60
options.UpdatesPerSecond <- 60
options.Position <- Vector2D(400, 400)
options.Title <- "WebGPU Demo"
options.IsVisible <- true
options.ShouldSwapAutomatically <- false
options.IsContextControlDisabled <- false
let shader = System.IO.File.ReadAllText(System.IO.Path.Join(__SOURCE_DIRECTORY__, "raymarching.wgsl"))

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
let mutable bindGroup = Unchecked.defaultof<_>
let window = Window.Create options
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
let onWindowLoad () =
    wgpu <- WebGPU.GetApi ()
    let descriptor = InstanceDescriptor()
    nativeInstance <- wgpu.CreateInstance(&descriptor)
    surface <- window.CreateWebGPUSurface(wgpu, nativeInstance)
    // get adapter
    let request = RequestAdapterOptions(CompatibleSurface = surface)
    let callback = new PfnRequestAdapterCallback(RequestAdapterCallback(fun _ adapter1 _ _ ->
        adapter <- adapter1
    ))
    wgpu.InstanceRequestAdapter(nativeInstance, &request, callback, Unchecked.defaultof<_>)
    // Get device
    let callback = new PfnDeviceLostCallback(DeviceLostCallback(fun reason arg1 arg2 -> ()))
    let descriptor = DeviceDescriptor(DeviceLostCallback = callback)
    let callback = new PfnRequestDeviceCallback(RequestDeviceCallback(fun _ device1 _ _ -> device <- device1))
    wgpu.AdapterRequestDevice(adapter, &descriptor, callback, Unchecked.defaultof<_>)
    let mutable wgslDescriptor = ShaderModuleWGSLDescriptor(
        ChainedStruct(SType = SType.ShaderModuleWgsldescriptor),
        NativePtr.ofNativeInt <| SilkMarshal.StringToPtr(shader)
    )
    let shaderModuleDescriptor = ShaderModuleDescriptor(
        NativePtr.ofNativeInt (NativePtr.toNativeInt &&wgslDescriptor)
    )
    shaderModule <- wgpu.DeviceCreateShaderModule(device, &shaderModuleDescriptor)
    wgpu.SurfaceGetCapabilities(surface, adapter, &surfaceCapabilities)
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
        ``Module`` = shaderModule,
        TargetCount = unativeint 1,
        Targets = &&colorTargetState,
        EntryPoint = NativePtr.ofNativeInt (SilkMarshal.StringToPtr("fs_main"))
    )
    // let vbLayout = VertexBufferLayout(
    //     
    // )
    let mutable bindingLayout = BufferBindingLayout(
        Type = BufferBindingType.Uniform,
        MinBindingSize = 8uL
    )
    let mutable bindGroupLayout = BindGroupLayoutEntry(
        Binding = 0u,
        Visibility = (ShaderStage.Vertex ||| ShaderStage.Fragment),
        Buffer = bindingLayout
    )
    let mutable bindGroupLayoutDesc = BindGroupLayoutDescriptor(
        EntryCount = unativeint 1,
        Entries = &&bindGroupLayout
    )
    let bindGroupLayout = wgpu.DeviceCreateBindGroupLayout(device, &&bindGroupLayoutDesc)
    let bindGroupLayouts = [| bindGroupLayout |]
    use bindGroupLayouts = fixed bindGroupLayouts
    let mutable layoutDesc = PipelineLayoutDescriptor(
        BindGroupLayoutCount = unativeint 1,
        BindGroupLayouts = bindGroupLayouts
    )
    let layout = wgpu.DeviceCreatePipelineLayout(device, &&layoutDesc)
    let renderPipelineDescriptor = RenderPipelineDescriptor(
        Vertex = VertexState(
            Module = shaderModule,
            EntryPoint = NativePtr.ofNativeInt (SilkMarshal.StringToPtr("vs_main"))
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
        Layout = layout
    )
    // let mutable desc = BufferDescriptor(
    //     Size = 8uL,
    //     // Usage = (BufferUsage.Vertex ||| BufferUsage.CopyDst)
    //     Usage = (BufferUsage.Uniform ||| BufferUsage.CopyDst)
    // )
    let mutable uniformBufferDesc = BufferDescriptor(
        Size = 8uL,
        Usage = (BufferUsage.CopyDst ||| BufferUsage.Uniform)
    )
    uniformBuffer <- wgpu.DeviceCreateBuffer(device, &&uniformBufferDesc)
    let mutable binding = BindGroupEntry(
        Binding = 0u,
        Buffer = uniformBuffer,
        Offset = 0uL,
        Size = 8uL
    )
    let mutable descriptor = BindGroupDescriptor(
        Layout = bindGroupLayout,
        EntryCount = bindGroupLayoutDesc.EntryCount,
        Entries = &&binding
    )
    bindGroup <- wgpu.DeviceCreateBindGroup(device, &&descriptor)
    // changingVertexBuffer <- wgpu.DeviceCreateBuffer(device, &&desc)
    renderPipeline  <- wgpu.DeviceCreateRenderPipeline(device, &renderPipelineDescriptor)
    
    // todo end
    
    // let bindGroupEntries: BindGroupEntry[] = [|
    //     BindGroupEntry(
    //         Binding = 0u,
    //         Buffer = changingVertexBuffer
    //     )
    // |]
    // use ptr = fixed bindGroupEntries
    // let mutable bindGroupDescription = BindGroupDescriptor(
    //     Layout = wgpu.RenderPipelineGetBindGroupLayout(renderPipeline, 0u),
    //     Entries = ptr
    // )
    // bindGroup <- wgpu.DeviceCreateBindGroup(device, &&bindGroupDescription)
    swap ()
    
let onWindowClosing () =
    wgpu.ShaderModuleRelease(shaderModule)
    wgpu.RenderPipelineRelease(renderPipeline)
    wgpu.DeviceRelease(device)
    wgpu.AdapterRelease(adapter)
    wgpu.SurfaceRelease(surface)
    wgpu.InstanceRelease(nativeInstance)
    wgpu.Dispose()
let onUpdate t = ()
let onWindowRender t =
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
    | _ -> ()
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
    
    let screen: float32[] = [| float32 window.FramebufferSize.X; float32 window.FramebufferSize.Y |]
    use data = fixed screen
    //
    wgpu.QueueWriteBuffer(queue, uniformBuffer, 0uL, data |> NativePtr.toVoidPtr, unativeint 8)
    
    wgpu.RenderPassEncoderDraw(renderPass, 6u, 2u, 0u, 0u)
    wgpu.RenderPassEncoderEnd(renderPass)
    let cbd = CommandBufferDescriptor()
    let mutable buffer = wgpu.CommandEncoderFinish(encoder, &cbd)
    wgpu.QueueSubmit(queue, unativeint 1, &&buffer)
    wgpu.SurfacePresent(surface)
    wgpu.CommandBufferRelease(buffer)
    wgpu.CommandEncoderRelease(encoder)
    wgpu.TextureViewRelease(view)
    wgpu.TextureRelease(texture.Texture)
    
let onFramebufferResize size =
    swap()
    
window.add_Load onWindowLoad
window.add_Closing onWindowClosing
window.add_Update onUpdate
window.add_Render onWindowRender
window.add_FramebufferResize onFramebufferResize
window.Run()