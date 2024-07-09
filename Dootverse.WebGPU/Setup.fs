namespace global

open Silk.NET.WebGPU
open Silk.NET.Core.Native
open Microsoft.FSharp.NativeInterop

[<AutoOpen>]
module Extensions =
    type WebGPU with
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
            
            
namespace Dootverse.WebGPU

module Setup =
    let window (wgpu, nativeInstance, surface) = null
