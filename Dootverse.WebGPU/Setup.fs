namespace global

type WgslShader = interface end

open Microsoft.FSharp.Quotations
open System.Threading.Tasks
open Silk.NET.WebGPU
open Silk.NET.Core.Native
open Microsoft.FSharp.NativeInterop
// module Foo =    
//     let foo<'a, 'b> (a: 'a) (b: 'b) : (Impl<'a> & Impl<'b>)=
//         { new Impl<'a> with member this.Get = a
//           interface Impl2<'b> with member this.Get = b }

type C =
    static member string value = NativePtr.ofNativeInt<byte> (SilkMarshal.StringToPtr value)

type BufferInfo =
    {
        isUniform: bool
        usage: BufferUsage
        // size: uint64
        size: int
    }
    
type DotnetBuffer =
    {
        ptr: nativeptr<Buffer> ref
        info: BufferInfo
    }
    
// type 't with todo Extensions to types with a reference to themselves, ie: serializeWith
//     member this.Foo = ()

[<AutoOpen>]
module WebGPUBind =
    // type ShaderBinder<'t>(buffer: nativeptr<Buffer> ref) =
    // todo : create a RenderInstance type that will hold onto all the buffers so you don't need to save variables?
    type ShaderBinder<'t>(acc) = // todo : linear types would guarantee that this only gets used once so you don't accidentally unbind another shader
        let buffer = ref Unchecked.defaultof<nativeptr<Buffer>>
        member this.Buffer = buffer
        member this.BufferRefs info =
            let variables = List.toArray <| List.rev ({ ptr = buffer; info = info } :: acc)
            variables
    // type ShaderBinder<'t2, 't1>(buffer: nativeptr<Buffer> ref, cons: ShaderBinder<'t1>) =
    type ShaderBinder<'t2, 't1>(acc: _ list) =
        // inherit ShaderBinding<'t1>(cons.Buffer1)
        let buffer = ref Unchecked.defaultof<nativeptr<Buffer>>
        // member this.Bindings = bindings
        member this.Buffer = buffer
        member this.Rest info = ShaderBinder<'t1>({ ptr = buffer; info = info } :: acc)
    type ShaderBinder<'t3, 't2, 't1>(acc: _ list) =
        // inherit ShaderBinding<'t1>(cons.Buffer1)
        let buffer = ref Unchecked.defaultof<nativeptr<Buffer>>
        // member this.Bindings = bindings
        member this.Buffer = buffer
        member this.Rest info = ShaderBinder<'t2, 't1>({ ptr = buffer; info = info } :: acc)
    type ShaderBinder<'t4, 't3, 't2, 't1>(acc: _ list) =
        // inherit ShaderBinding<'t1>(cons.Buffer1)
        let buffer = ref Unchecked.defaultof<nativeptr<Buffer>>
        // member this.Bindings = bindings
        member this.Buffer = buffer
        member this.Rest info = ShaderBinder<'t3, 't2, 't1>({ ptr = buffer; info = info } :: acc)
type ShaderVariable<'t>(buffer: _ ref, serializer: 't -> byte[]) =
    member this.Write(wgpu: WebGPU, queue, data: 't) =
        let serialized = serializer data
        let ptr = fixed serialized
        wgpu.QueueWriteBuffer(queue, buffer.Value, 0uL, NativePtr.toVoidPtr ptr, unativeint serialized.Length)
    member this.Buffer = buffer.Value
type ShaderMapVar<'t>(buffer: _ ref, serializer: 't -> byte[]) =
    member this.Write(wgpu: WebGPU, queue, data: 't) =
        let serialized = serializer data
        let ptr = fixed serialized
        wgpu.QueueWriteBuffer(queue, buffer.Value, 0uL, NativePtr.toVoidPtr ptr, unativeint serialized.Length)
    member this.Buffer = buffer.Value
type ShaderBuffer<'t>(buffer: _ ref, size: int, serializer: 't -> byte[]) =
    member this.Write(wgpu: WebGPU, queue, offset, data: 't seq) =
        let bufferData = [| for item in data do yield! serializer item |]
        let (* todo: use ? *) ptr = fixed bufferData
        let writeSize = bufferData.Length
        wgpu.QueueWriteBuffer(queue, buffer.Value, offset, NativePtr.toVoidPtr ptr, unativeint writeSize)
    member this.Buffer = buffer.Value
type ShaderMap<'t when 't: unmanaged>(wgpu: WebGPU, device: nativeptr<Device>, buffer: _ ref, size: int, serializer: 't -> byte[]) =
    // todo
    let stagingDesc = BufferDescriptor(
        Usage = (BufferUsage.MapRead ||| BufferUsage.CopyDst),
        Size = uint64 size,
        MappedAtCreation = false
    )
    let stagingBuffer = wgpu.DeviceCreateBuffer(device, &stagingDesc)
    member this.Write(wgpu: WebGPU, queue, offset, data: 't seq) =
        let bufferData = [| for item in data do yield! serializer item |]
        let (* todo: use ? *) ptr = fixed bufferData
        let writeSize = bufferData.Length
        wgpu.QueueWriteBuffer(queue, buffer.Value, offset, NativePtr.toVoidPtr ptr, unativeint writeSize)
    member this.AddCopy(wgpu: WebGPU, encoder) =
        wgpu.CommandEncoderCopyBufferToBuffer(encoder, buffer.Value, 0uL, stagingBuffer, 0uL, uint64 size)
    member this.MapAsync (wgpu: WebGPU) =
        task {
            let promise = TaskCompletionSource<_>()
            let callback = new PfnBufferMapCallback(fun _ _ ->
                ignore <| promise.TrySetResult ()
            )
            wgpu.BufferMapAsync(stagingBuffer, MapMode.Read, unativeint 0, unativeint size, callback, Unchecked.defaultof<_>)
            do! promise.Task
            callback.Dispose()
            let result = wgpu.BufferGetMappedRange(stagingBuffer, unativeint 0, unativeint size)
            return result |> NativePtr.ofVoidPtr<'t>
        }
    member this.Buffer = buffer.Value
type W = { wgpu: WebGPU } // todo
type Dev = { device: Device; wgpu: W } // todo
type CompPipeline = { p: nativeptr<ComputePipeline>; device: Dev }
type Wgpu =
    static member Bind(binding: ShaderBinder<'t[]>) = fun info -> fun serializer ->
        ShaderBuffer<'t>(binding.Buffer, int info.size, serializer), binding.BufferRefs info
    static member Bind(bindings: ShaderBinder<'t2[], 't1>) = fun info -> fun serializer ->
        ShaderBuffer<'t2>(bindings.Buffer, int info.size, serializer), bindings.Rest info
    static member Bind(bindings: ShaderBinder<'t3[], 't2, 't1>) = fun info -> fun serializer ->
        ShaderBuffer<'t3>(bindings.Buffer, int info.size, serializer), bindings.Rest info
    static member Bind(bindings: ShaderBinder<'t4[], 't3, 't2, 't1>) = fun info -> fun serializer ->
        ShaderBuffer<'t4>(bindings.Buffer, int info.size, serializer), bindings.Rest info
    static member MapS(binding: ShaderBinder<'t[]>) = fun wgpu device info -> fun serializer ->
        ShaderMap<'t>(wgpu, device, binding.Buffer, int info.size, serializer), binding.BufferRefs info
    static member Map(bindings: ShaderBinder<'t2[], 't1>) = fun wgpu device info -> fun serializer ->
        ShaderMap<'t2>(wgpu, device, bindings.Buffer, int info.size, serializer), bindings.Rest info
    static member Map(bindings: ShaderBinder<'t3[], 't2, 't1>) = fun wgpu device info -> fun serializer ->
        ShaderMap<'t3>(wgpu, device, bindings.Buffer, int info.size, serializer), bindings.Rest info
    static member Map(bindings: ShaderBinder<'t4[], 't3, 't2, 't1>) = fun wgpu device info -> fun serializer ->
        ShaderMap<'t4>(wgpu, device, bindings.Buffer, int info.size, serializer), bindings.Rest info
    static member Shader(shader: Quotations.Expr<'a * 'b -> _>) =
        Unchecked.defaultof<WebGPUBind.ShaderBinder<'a, 'b>>
[<AutoOpen>]
module WebGPUBindExtensions =
    // let inline takesList<'t, 'a when 'a: (member value: 't option)> (value: {| value: 't option; cons: 'a |}) =
    //     ()
    type Wgpu with
        static member BindS(binding: ShaderBinder<'t>) = fun info serializer ->
            ShaderVariable<'t>(binding.Buffer, serializer), binding.BufferRefs info
        static member Bind(binding: ShaderBinder<'t2, 't1>) = fun info serializer ->
            ShaderVariable<'t2>(binding.Buffer, serializer), binding.Rest info
        static member Bind(binding: ShaderBinder<'t3, 't2, 't1>) = fun info serializer ->
            ShaderVariable<'t3>(binding.Buffer, serializer), binding.Rest info
        static member Bind(binding: ShaderBinder<'t4, 't3, 't2, 't1>) = fun info serializer ->
            ShaderVariable<'t4>(binding.Buffer, serializer), binding.Rest info
        static member MapS(binding: ShaderBinder<'t>) = fun info -> fun serializer ->
            ShaderMapVar<'t>(binding.Buffer, serializer), binding.BufferRefs info
        static member Map(bindings: ShaderBinder<'t2, 't1>) = fun info -> fun serializer ->
            ShaderMapVar<'t2>(bindings.Buffer, serializer), bindings.Rest info
        static member Map(bindings: ShaderBinder<'t3, 't2, 't1>) = fun info -> fun serializer ->
            ShaderMapVar<'t3>(bindings.Buffer, serializer), bindings.Rest info
        static member Map(bindings: ShaderBinder<'t4, 't3, 't2, 't1>) = fun info -> fun serializer ->
            ShaderMapVar<'t4>(bindings.Buffer, serializer), bindings.Rest info
            

[<AutoOpen>]
module Extensions =
    type PollDelegate = delegate of nativeptr<Device> * bool * nativeint -> bool
    let mutable poll = None
    type WebGPU with
        member inline this.DevicePoll(device, wait, userData) =
            if poll.IsNone then
                let found, ptr = this.Context.TryGetProcAddress("wgpuDevicePoll")
                poll <- Some (System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<PollDelegate> ptr)
            poll.Value.Invoke(device, wait, userData)
        member inline this.RequestDeviceAsync(adapter, ?request) =
            let request = request |> Option.defaultValue (DeviceDescriptor())
            let promise = TaskCompletionSource<_>()
            let callback = new PfnRequestDeviceCallback(RequestDeviceCallback(fun _ device _ _ ->
                promise.TrySetResult device |> ignore))
            task {
                this.AdapterRequestDevice(adapter, &request, callback, Unchecked.defaultof<_>)
                let! result = promise.Task
                callback.Dispose()
                return result
            }
        member inline this.CreateInstance(?desc) =
            let descriptor = desc |> Option.defaultValue (InstanceDescriptor())
            this.CreateInstance(&descriptor)
        member inline this.RequestAdapterAsync(instance, ?request: RequestAdapterOptions) =
            let request = request |> Option.defaultValue (RequestAdapterOptions())
            let promise = TaskCompletionSource<_>()
            let callback = new PfnRequestAdapterCallback(RequestAdapterCallback(fun _ a _ _ ->
                promise.TrySetResult a |> ignore))
            task {
                this.InstanceRequestAdapter(instance, &request, callback, Unchecked.defaultof<_>)
                let! result = promise.Task
                callback.Dispose()
                return result
            }
        member inline this.CreateBuffer(device, desc) =
            let mutable value = desc
            this.DeviceCreateBuffer(device, &&value)
        member inline this.DeviceCreateShaderModule(device, descriptor) =
            this.DeviceCreateShaderModule(device, &descriptor)
        member inline this.CreateShader(device, shader) =
            let ptr = SilkMarshal.StringToPtr shader
            let mutable descriptor = ShaderModuleWGSLDescriptor(
                ChainedStruct(SType = SType.ShaderModuleWgsldescriptor),
                NativePtr.ofNativeInt ptr)
            let module_ = ShaderModuleDescriptor(NativePtr.ofNativeInt (NativePtr.toNativeInt &&descriptor))
            this.DeviceCreateShaderModule(device, module_)
        member inline this.CreateBindGroupLayout(device, bindings: _ []) =
            use entries = fixed bindings
            let mutable descriptor = BindGroupLayoutDescriptor(
                EntryCount = unativeint bindings.Length,
                Entries = entries)
            this.DeviceCreateBindGroupLayout(device, &&descriptor)
        member this.CreateBindGroup(device, layout, entries: _ []) =
            use ptr = fixed entries
            let mutable descriptor = BindGroupDescriptor(
                Layout = layout,
                EntryCount = unativeint entries.Length,
                Entries = ptr)
            this.DeviceCreateBindGroup(device, &&descriptor)
        member inline this.GetCurrentTexture(surface) =
            let mutable texture = SurfaceTexture()
            this.SurfaceGetCurrentTexture(surface, &&texture)
            texture
        member inline this.CreatePipelineLayout(device, entries: _ []) =
            use ptr = fixed entries
            let mutable descriptor = PipelineLayoutDescriptor(
                BindGroupLayoutCount = unativeint entries.Length,
                // BindGroupLayouts = &&bindGroupLayout
                BindGroupLayouts = ptr
            )
            this.DeviceCreatePipelineLayout(device, &&descriptor)
        member inline this.CreateBuffers(device, entries: _ []) =
            [| for descriptor in entries do
                this.CreateBuffer(device, descriptor) |]
            
        member inline this.CreateCommandEncoder(device, ?desc) =
            let mutable value = desc |> Option.defaultValue (CommandEncoderDescriptor())
            this.DeviceCreateCommandEncoder(device, &&value)
        member inline this.EncoderBeginComputePass(encoder, ?desc: ComputePassDescriptor) =
            let value = desc |> Option.defaultValue (ComputePassDescriptor())
            this.CommandEncoderBeginComputePass(encoder, &value)
        member inline this.CreateBindGroupEntries(entries: BufferDescriptor[], buffers: _ []) =
            [|
                for i in 0..buffers.Length - 1 do
                    let buffer = buffers[i]
                    BindGroupEntry(
                        Binding = uint i,
                        Buffer = buffer,
                        Offset = 0uL,
                        Size = entries[i].Size
                    )
            |]
        member inline this.EncoderFinish(encoder, ?desc) =
            let desc = desc |> Option.defaultValue(CommandBufferDescriptor())
            this.CommandEncoderFinish(encoder, &desc)
        member this.CreateBindGroup(device, bindGroupLayout, entries) =
            let buffers = this.CreateBuffers(device, entries)
            let bindings = this.CreateBindGroupEntries(entries, buffers)
            let bindGroup = this.CreateBindGroup(device, bindGroupLayout, bindings)
            {| buffers = buffers; bindings = bindings; bindGroup = bindGroup |}
            
        member this.CreateBuffers device = fun (infos: BufferInfo[]) ->
            let layouts = [|
                for i in 0..infos.Length - 1 do
                    let info = infos[i]
                    let minBindingSize = uint64 info.size
                    let visibility =
                        if info.isUniform
                        then ShaderStage.Compute
                        else ShaderStage.Compute
                    BindGroupLayoutEntry(
                        Binding = uint i,
                        Visibility = visibility,
                        Buffer = BufferBindingLayout(
                            Type = (
                                if info.isUniform
                                then BufferBindingType.Uniform
                                else BufferBindingType.Storage),
                            MinBindingSize = minBindingSize
                        )
                    )
            |]
            let descriptors = [|
                for info in infos do
                    let usage =
                        if info.isUniform
                        then BufferUsage.Uniform
                        else BufferUsage.Storage
                    BufferDescriptor(
                        Size = uint64 info.size,
                        // Usage = (usage ||| BufferUsage.CopyDst)
                        // Usage = (usage ||| info.usage)
                        Usage = info.usage
                    )
            |]
            let layout = this.CreateBindGroupLayout(device, layouts)
            {| this.CreateBindGroup(device, layout, descriptors) with
                bindGroupLayout = layout |}
        member this.CreateCompute shaderCode entryPoint device (vars: DotnetBuffer array) =
            // let layout: nativeptr<BindGroupLayout> = this.ComputePipelineGetBindGroupLayout(pipeline, 0u)
            // let layout = this.CreateBindGroupLayout()
            let infos = vars |> Array.map _.info
            let result = (this.CreateBuffers device infos)
            let computeLayout = this.CreatePipelineLayout(device, [| result.bindGroupLayout |])
            let shaderModule =
                let mutable wgslDesc = ShaderModuleWGSLDescriptor(
                     Chain = ChainedStruct(SType = SType.ShaderModuleWgslDescriptor),
                     Code = C.string shaderCode
                    // NextInChain = &&wgslDesc
                )
                let mutable desc = ShaderModuleDescriptor(NativePtr.ofNativeInt (NativePtr.toNativeInt &&wgslDesc))
                this.DeviceCreateShaderModule(device, &&desc)
            let desc = ComputePipelineDescriptor(
                Compute = ProgrammableStageDescriptor(
                    Module = shaderModule,
                    EntryPoint = C.string entryPoint
                ),
                Layout = computeLayout
            )
            let pipeline = this.DeviceCreateComputePipeline(device, &desc)
            for i in 0..result.buffers.Length - 1 do
                vars[i].ptr.Value <- result.buffers[i]
            {| result with
                pipeline = pipeline
                shaderModule = shaderModule |}
        member this.InitBindings (device: nativeptr<Device>) (vars: DotnetBuffer array) =
            let infos = vars |> Array.map _.info
            let group = this.CreateBuffers device infos
            for i in 0..group.buffers.Length - 1 do
                vars[i].ptr.Value <- group.buffers[i]
            {| bindGroup = group.bindGroup; layout = group.bindGroupLayout |}
        member this.CreateBinder (device, shader: Quotations.Expr<'a * 'b -> _>) =
            // let infoForType (t: System.Type) : BufferInfo =
                // { isUniform = false; size = 0uL }
            // let group = this.CreateBuffers device [| infoForType typeof<'a>; infoForType typeof<'b> |]
            // let a = ref Unchecked.defaultof<_>
            // let b = ref Unchecked.defaultof<_>
            ShaderBinder<'a, 'b>([])
        member this.CreateBinder (shader: Quotations.Expr<'a * 'b -> _>) =
            ShaderBinder<'a, 'b>([])
        member this.CreateBinder (shader: Quotations.Expr<'a * 'b * 'c -> _>) =
            ShaderBinder<'a, 'b, 'c>([])
        member this.CreateBinder (shader: Quotations.Expr<'a * 'b * 'c * 'd -> _>) =
            ShaderBinder<'a, 'b, 'c, 'd>([])
        member this.CreateBinder (shader: 'a * 'b -> _) =
            ShaderBinder<'a, 'b>([])
        member this.CreateBinder (shader: 'a * 'b * 'c -> _) =
            ShaderBinder<'a, 'b, 'c>([])
        member this.CreateBinder (shader: 'a * 'b * 'c * 'd -> _) =
            ShaderBinder<'a, 'b, 'c, 'd>([])
        member this.CreateBinderS (shader: 'a -> _) =
            ShaderBinder<'a>([])
        member inline this.StartRenderPass (encoder, descriptors: _ []) =
            let ptr = fixed descriptors
            let renderPass = RenderPassDescriptor(
                ColorAttachments = ptr,
                ColorAttachmentCount = unativeint descriptors.Length
            )
            this.CommandEncoderBeginRenderPass(encoder, &renderPass)
        member this.RunComputeModule (compute_pipeline, bindGroup, device, x, y, z) =
            let command_encoder = 
                this.CreateCommandEncoder(device, CommandEncoderDescriptor())
            let compute_pass_encoder = 
                this.EncoderBeginComputePass(command_encoder, ComputePassDescriptor())
            let queue = this.DeviceGetQueue(device)
            this.ComputePassEncoderSetPipeline(compute_pass_encoder, compute_pipeline);
            this.ComputePassEncoderSetBindGroup(compute_pass_encoder, 0u, bindGroup, unativeint 0, Unchecked.defaultof<nativeptr<_>>)
            this.ComputePassEncoderDispatchWorkgroups(compute_pass_encoder, uint x, uint y, uint z)
            this.ComputePassEncoderEnd(compute_pass_encoder)
            
            let mutable command_buffer = 
                this.EncoderFinish(command_encoder, CommandBufferDescriptor())
            // this.QueueWriteBuffer // todo
            // this.QueueSubmit // todo
            // this.BufferMapAsync // todo (use the ShaderMapVar / ShaderMap type)
            ()
        
// [<AutoOpen>]        
// module MoreExtensions =
//     type WebGPU with
//         member this.CreateBinder (shader: Quotations.Expr<'a -> _>) =
//             ShaderBinder<'a, 'b>([])
            
            
            
namespace Dootverse.WebGPU