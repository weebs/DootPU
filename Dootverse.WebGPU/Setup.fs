namespace global

open Silk.NET.WebGPU
open Silk.NET.Core.Native
open Microsoft.FSharp.NativeInterop

type BufferInfo =
    {
        isUniform: bool
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
type ShaderVariable<'t>(buffer: _ ref, serializer: 't -> byte[]) =
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
type Wgpu =
    static member Bind(binding: ShaderBinder<'t[]>) = fun info -> fun serializer ->
        ShaderBuffer<'t>(binding.Buffer, int info.size, serializer), binding.BufferRefs info
    static member Bind(bindings: ShaderBinder<'t2[], 't1>) = fun info -> fun serializer ->
        ShaderBuffer<'t2>(bindings.Buffer, int info.size, serializer), bindings.Rest info
    static member Shader(shader: Quotations.Expr<'a * 'b -> _>) =
        Unchecked.defaultof<WebGPUBind.ShaderBinder<'a, 'b>>
[<AutoOpen>]
module WebGPUBindExtensions =
    let inline takesList<'t, 'a when 'a: (member value: 't option)> (value: {| value: 't option; cons: 'a |}) =
        ()
    type Wgpu with
        static member Bind(binding: ShaderBinder<'t>) = fun info serializer ->
            ShaderVariable<'t>(binding.Buffer, serializer), binding.BufferRefs info
        static member Bind(binding: ShaderBinder<'t2, 't1>) = fun info serializer ->
            ShaderVariable<'t2>(binding.Buffer, serializer), binding.Rest info
            

[<AutoOpen>]
module Extensions =
    type WebGPU with
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
        member inline this.CreateBindGroup(device, layout, entries: _ []) =
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
        member inline this.CreateBindGroup(device, bindGroupLayout, entries) =
            let buffers = this.CreateBuffers(device, entries)
            let bindings = this.CreateBindGroupEntries(entries, buffers)
            let bindGroup = this.CreateBindGroup(device, bindGroupLayout, bindings)
            {| buffers = buffers; bindings = bindings; bindGroup = bindGroup |}
            
        member this.CreateBuffers device = fun (infos: BufferInfo[]) ->
            let layouts = [|
                for i in 0..infos.Length - 1 do
                    let minBindingSize = 0uL // todo
                    let info = infos[i]
                    let visibility =
                        if info.isUniform
                        then ShaderStage.Vertex ||| ShaderStage.Fragment
                        else ShaderStage.Fragment
                    BindGroupLayoutEntry(
                        Binding = uint i,
                        Visibility = visibility,
                        Buffer = BufferBindingLayout(
                            Type = (if info.isUniform then BufferBindingType.Uniform else BufferBindingType.Storage),
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
                        Usage = (usage ||| BufferUsage.CopyDst)
                    )
            |]
            let layout = this.CreateBindGroupLayout(device, layouts)
            {| this.CreateBindGroup(device, layout, descriptors) with
                bindGroupLayout = layout |}
        member this.InitBindings device (vars: DotnetBuffer array) =
            let infos = vars |> Array.map _.info
            let group = this.CreateBuffers device infos
            for i in 0..group.buffers.Length - 1 do
                vars[i].ptr.Value <- group.buffers[i]
            {| bindGroup = group.bindGroup; layout = group.bindGroupLayout |}
        member this.CreateBinder device = fun (shader: Quotations.Expr<'a * 'b -> _>) ->
            // let infoForType (t: System.Type) : BufferInfo =
                // { isUniform = false; size = 0uL }
            // let group = this.CreateBuffers device [| infoForType typeof<'a>; infoForType typeof<'b> |]
            // let a = ref Unchecked.defaultof<_>
            // let b = ref Unchecked.defaultof<_>
            ShaderBinder<'a, 'b>([])
        member inline this.StartRenderPass (encoder, descriptors: _ []) =
            let ptr = fixed descriptors
            let renderPass = RenderPassDescriptor(
                ColorAttachments = ptr,
                ColorAttachmentCount = unativeint descriptors.Length
            )
            this.CommandEncoderBeginRenderPass(encoder, &renderPass)
    type C =
        static member string value = NativePtr.ofNativeInt<byte> (SilkMarshal.StringToPtr value)
            
            
            
namespace Dootverse.WebGPU

module Setup =
    let window (wgpu, nativeInstance, surface) = null
