namespace global

open Silk.NET.WebGPU
open Silk.NET.Core.Native
open Microsoft.FSharp.NativeInterop

type BufferInfo =
    {
        isUniform: bool
        size: uint64
    }

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
                        Size = info.size,
                        Usage = (usage ||| BufferUsage.CopyDst)
                    )
            |]
            let layout = this.CreateBindGroupLayout(device, layouts)
            {| this.CreateBindGroup(device, layout, descriptors) with bindGroupLayout = layout |}
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
