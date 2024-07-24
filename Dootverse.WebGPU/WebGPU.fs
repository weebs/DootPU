module WebGPU
open System
open System.Runtime.InteropServices
open Dootverse.WebGPU
open Silk.NET.Input
open Microsoft.FSharp.NativeInterop
open Silk.NET.Core.Native
open Silk.NET.Maths
open Silk.NET.WebGPU
open Silk.NET.Windowing
open System.Collections.Generic

module Demo =
    let shaderQuotation = <@ fun (floats: float32[], float: float32) -> () @>

let gridSize = 22
let memSize = uint64 (gridSize * gridSize * 4 * sizeof<float32>)
let numShapes = 100
let shapesBufferSize = numShapes * 6 * 4

let shapes = [|
    for i in 1..(gridSize * gridSize) do
        0f; 0f; 0f; 0f
|]
type [<Struct>] output = { mutable color: int }

let mutable options = WindowOptions.Default
options.API <- GraphicsAPI.None
let mutable windowWidth = 1920
let mutable windowHeight = 1080
options.Size <- Vector2D(windowWidth, windowHeight)
options.FramesPerSecond <- 60
options.UpdatesPerSecond <- 60
options.Position <- Vector2D(400, 400)
options.Title <- "WebGPU Demo"
options.IsVisible <- true
options.ShouldSwapAutomatically <- false
options.IsContextControlDisabled <- false
let shader =
    // System.IO.File.ReadAllText(System.IO.Path.Join(__SOURCE_DIRECTORY__, "raymarching.wgsl")) + "\n" +
    Compiler.Print.module' Shaders.compiledWgsl
printfn $"{shader}"
let structs = Compiler.gatherStructs Shaders.shader'
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
let voxelData = Array.zeroCreate (100 * 100 * 100)
let mutable screenVar = Unchecked.defaultof<_>
let mutable voxels = Unchecked.defaultof<_>
let mutable shapeIndexes = Unchecked.defaultof<_>
let mutable shapesVariable = Unchecked.defaultof<_>
let mutable circlesBuffer = Unchecked.defaultof<nativeptr<_>>
let mutable bindGroup = Unchecked.defaultof<_>
let window = Window.Create options
let mutable posX = 0f
let mutable posY = 0f
let keys = Dictionary()
for i in 1..100 do
    keys[i] <- false
let swap () =
    // Create swap
    surfaceConfig <- SurfaceConfiguration(
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
    wgpu <- WebGPU.GetApi ()
    let wgpu' = new WebGPU'(wgpu)
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
    // shaderModule <- wgpu.DeviceCreateShaderModule(device, &shaderModuleDescriptor)
    shaderModule <- wgpu.CreateShader(device, shader)
    wgpu.SurfaceGetCapabilities(surface, adapter, &surfaceCapabilities)
    // let vbLayout = VertexBufferLayout(
    //     
    // )
    try
        // let serializeScreen (value: Shaders.Screen) =
        //     [|
        //         yield! BitConverter.GetBytes value.gridSize
        //         yield! BitConverter.GetBytes value.voxelGridScale
        //         yield! BitConverter.GetBytes value.gridSize
        //         yield! BitConverter.GetBytes value.posX
        //         yield! BitConverter.GetBytes value.posY
        //         yield! BitConverter.GetBytes value.width
        //         yield! BitConverter.GetBytes value.height
        //     |]
        let serializeScreen = Compiler.makeSerialize<Shaders.Screen>()
        let binds = wgpu'.CreateBinder Shaders.shader'
        let serializeShape (shape: Shaders.Shape) =
            let code, vec3, f, a =
                match shape with
                | Shaders.Sphere(vec3, f) -> 0, vec3, f, 0f
                | Shaders.Cube(vec3, f) -> 1, vec3, f, 0f
                | Shaders.RoundedCube(vec3, f, a) -> 2, vec3, f, a
            [|
                yield! BitConverter.GetBytes code
                yield! BitConverter.GetBytes vec3.x
                yield! BitConverter.GetBytes vec3.y
                yield! BitConverter.GetBytes vec3.z
                yield! BitConverter.GetBytes f
                yield! BitConverter.GetBytes a
            |]
        let serializeVoxel (voxel: Shaders.Voxel) =
            [| yield! BitConverter.GetBytes voxel.startIndex; yield! BitConverter.GetBytes voxel.count |]
        let flags = BufferUsage.CopyDst ||| BufferUsage.Storage
        // let flags = BufferUsage.CopyDst
        let (screen, binds') = Wgpu.BindI (binds, { usage = flags; isUniform = false; size = Shaders.sizeofWgslType<Shaders.Screen> () }, serializeScreen)
        // let (circles, binds) = Wgpu.Bind binds { isUniform = true; size = 4 * 10 } (fun _ -> [||])
        // let (shapes, binds) = Wgpu.Bind binds { isUniform = false; size = int memSize } serializeShape
        let voxelGridSize = 100
        let (shapes, binds'') = Wgpu.BindI (binds', { usage = flags; isUniform = false; size = shapesBufferSize }, serializeShape)
        let (voxels_, binds''') =
            Wgpu.BindI (binds'', {
                isUniform = false
                usage = flags
                size = Shaders.sizeofWgslType<Shaders.Voxel> () * (voxelGridSize * voxelGridSize * voxelGridSize)
            }, serializeVoxel)
        let (shapeIndexes_, binds'''') = Wgpu.BindI (binds''', { usage = flags; isUniform = false; size = 108000 })
        let group = wgpu.InitBindings ShaderStage.Fragment device binds''''.Buffers
        
        voxels <- voxels_
        shapeIndexes <- shapeIndexes_
        screenVar <- screen
        shapesVariable <- shapes
        
        uniformBuffer <- screen.Buffer
        circlesBuffer <- shapes.Buffer
        bindGroup <- group.bindGroup
        // let group = wgpu.CreateBuffers device [|
        //     { size = int binding0Size; isUniform = true }
        //     { size = int memSize; isUniform = false }
        // |]
        // uniformBuffer <- group.buffers[0]
        // circlesBuffer <- group.buffers[1]
        // bindGroup <- group.bindGroup
        // let mainFunction () =
        //     let serializer t = [||]
        //     let m = Wgpu.Shader Shaders.shader'
        //     let screen, m = Wgpu.Bind m serializeScreen
        //     let circles = Wgpu.Bind m 100 serializer
        //     ()
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
            Fragment = &&fragmentState,
            DepthStencil = Unchecked.defaultof<_>,
            // Layout = wgpu.CreatePipelineLayout(device, [| group.bindGroupLayout |])
            Layout = wgpu.CreatePipelineLayout(device, [| group.layout |])
        )
        renderPipeline  <- wgpu.CreateRenderPipeline(device, renderPipelineDescriptor)
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
let onUpdate t = ()
let mutable time = 0.0

// type Window =
//     abstract member Render : float -> unit
//     abstract member Update : float -> unit
let onWindowRender t =
    time <- time + t
    if keys[int Key.A] then
        posX <- posX - 10f * float32 t
    if keys[int Key.D] then
        posX <- posX + 10f * float32 t
    if keys[int Key.W] then
        posY <- posY + 10f * float32 t
    if keys[int Key.S] then
        posY <- posY - 10f * float32 t

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
    
    let encoderDescriptor = CommandEncoderDescriptor()
    let encoder = wgpu.DeviceCreateCommandEncoder(device, &encoderDescriptor)
    let colorAttachment = RenderPassColorAttachment(
        View = view,
        ResolveTarget = Unchecked.defaultof<_>,
        LoadOp = LoadOp.Clear,
        StoreOp = StoreOp.Store,
        ClearValue = Color(0, 1, 0, 1)
    )
    let queue = wgpu.DeviceGetQueue(device)
    let renderPass = wgpu.StartRenderPass(encoder, [| colorAttachment |])
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
    do
        use shapes = fixed shapes
        let voxelGridSize = 100
        let getVoxelIndex x y z =
            int z +
            (int y * voxelGridSize) +
            (int x * voxelGridSize * voxelGridSize)
        let populatedVoxels = Dictionary()
        let allShapes = [|
            for i in 0..numShapes - 1 do
                // let position = Wgsl.Wgsl.vec3(posX + float32 i, 0f, 4f)
                // let position = Wgsl.Wgsl.vec3(posX + float32 i, posY + float32 i, 10f)
                // let position = Wgsl.Wgsl.vec3(
                //     -17f + posX + float32 i,
                //     MathF.Cos((float32 i * 0.2f) + float32 time) * 2.48f,
                //     10f)
                let position = Wgsl.Wgsl.vec3(
                    MathF.Sin((float32 i * 0.2f) + float32 time) * 10.48f,
                    float32 i - 10f,
                    MathF.Cos((float32 i * 0.2f) + float32 time) * 10.48f + 20f)
                let voxelPos = Wgsl.Wgsl.floor(position)
                for x in int voxelPos.x - 1 .. int voxelPos.x + 1 do
                    for y in int voxelPos.y - 1 .. int voxelPos.y + 1 do
                        for z in int voxelPos.z - 1 .. int voxelPos.z + 1 do
                            let index = getVoxelIndex (x + 50) (y + 50) (z + 50)
                            if not (populatedVoxels.ContainsKey index) then
                                populatedVoxels[index] <- ResizeArray()
                            populatedVoxels[index].Add i
                let size = 0.25f
                let tag = i % 3
                if tag = 0 then Shaders.Sphere (position, size)
                elif tag = 1 then Shaders.Cube (position, size)
                else Shaders.RoundedCube (position, size, 0.12f)
        |]
        let data = ResizeArray()
        // let mutable offset = 0
        // let mutable index = 0
        // let voxelsGrid = [|
        for i in 0..(100 * 100 * 100) - 1 do
            if populatedVoxels.ContainsKey i then
                let offset = data.Count
                for shapeIndex in populatedVoxels[i] do data.Add shapeIndex
                voxelData[i] <- ({ count = populatedVoxels[i].Count; startIndex = offset } : Shaders.Voxel)
            else
                voxelData[i] <- ({ count = 0; startIndex = 0 } : Shaders.Voxel)
        // |]
        screenVar.Write(wgpu, queue, {
            gridSize = 0
            voxelGridScale = 1.0f
            voxelGridSize = voxelGridSize 
            posX = posX
            posY = posY
            width = float32 windowWidth
            height = float32 windowHeight
        })
        // wgpu.QueueWriteBuffer(queue, uniformBuffer, 0uL, data |> NativePtr.toVoidPtr, unativeint binding0Size)
        shapesVariable.Write (wgpu, queue, 0uL, allShapes)
        voxels.Write (wgpu, queue, 0uL, voxelData)
        shapeIndexes.Write (wgpu, queue, 0uL, data.ToArray())
        // wgpu.QueueWriteBuffer(queue, circlesBuffer, 0uL, shapes |> NativePtr.toVoidPtr, unativeint memSize)
        
        wgpu.RenderPassEncoderDraw(renderPass, 6u, 2u, 0u, 0u)
        wgpu.RenderPassEncoderEnd(renderPass)
        let cbd = CommandBufferDescriptor()
        // let mutable buffer = wgpu.CommandEncoderFinish(encoder, &cbd)
        use buffer = fixed [| wgpu.CommandEncoderFinish(encoder, &cbd) |]
        // wgpu.QueueSubmit(queue, unativeint 1, &&buffer)
        wgpu.QueueSubmit(queue, unativeint 1, buffer)
        wgpu.SurfacePresent(surface)
        wgpu.CommandBufferRelease(NativePtr.read buffer)
        wgpu.CommandEncoderRelease(encoder)
        wgpu.TextureViewRelease(view)
        wgpu.TextureRelease(texture.Texture)
        // Marshal.FreeHGlobal (NativePtr.toNativeInt shapes)
    
let onFramebufferResize (size: Vector2D<int>) =
    windowWidth <- size.X
    windowHeight <- size.Y
    swap ()
    
window.add_Load onWindowLoad
window.add_Closing onWindowClosing
window.add_Update onUpdate
window.add_Render onWindowRender
window.add_FramebufferResize onFramebufferResize
window.Run()