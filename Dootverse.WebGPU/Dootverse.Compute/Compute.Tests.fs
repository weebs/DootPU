module Dootverse.Compute.Compute_Tests

open System
open System.Runtime.InteropServices
open System.Threading
open Dootverse.WebGPU
open Dootverse.WebGPU.Wgsl
open Microsoft.FSharp.NativeInterop
open Microsoft.FSharp.Quotations

open Silk.NET.WebGPU
open NUnit.Framework
open type Shaders.Setup

type PollDelegate = delegate of nativeptr<Device> * bool * nativeint -> bool

let shader = """
@group(0) @binding(0) var<storage, read_write> output: array<i32>;

@compute
@workgroup_size(1)
fn main(@builtin(global_invocation_id) globalId: vec3<u32>) {
    output[globalId.x] = i32(globalId.x);
}
"""

[<AutoOpen>]
module rec Wrappers =
    type WebGPU'(wgpu: WebGPU) as this =
        inherit WebGPU(wgpu.Context)
        let instance = wgpu.CreateInstance()
        let adapter = wgpu.RequestAdapterAsync(instance).Result
        let device = Device'(this, wgpu.RequestDeviceAsync(adapter).Result)
        member this.Device = device
    type Queue'(wgpu: WebGPU') = class end
    type CommandEncoder'(wgpu: WebGPU', encoder) =
        member this.BeginComputePass (?d: ComputePassDescriptor) =
            ComputePassEncoder'(wgpu, wgpu.EncoderBeginComputePass(encoder, ?desc=d))
        member this.Encoder = encoder
    type ComputePassEncoder'(wgpu: WebGPU', encoder) =
        member this.SetPipeline pipeline = wgpu.ComputePassEncoderSetPipeline (encoder, pipeline)
        member this.SetBindGroup group index =
            wgpu.ComputePassEncoderSetBindGroup(encoder, index, group, unativeint 0, Unchecked.defaultof<nativeptr<_>>)
        member this.DispatchWorkgroups x y z =
            wgpu.ComputePassEncoderDispatchWorkgroups(encoder, x, y, z)
        member this.End () = wgpu.ComputePassEncoderEnd encoder
        member this.Encoder = encoder
        
    type Device'(wgpu: WebGPU', device: nativeptr<Device>) =
        member this.GetQueue () = wgpu.DeviceGetQueue device
        member this.CreateCommandEncoder () = CommandEncoder'(wgpu, wgpu.CreateCommandEncoder device)
        member this.CreateCompute shader entryPoint binds =
            wgpu.CreateCompute shader entryPoint device binds
        member this.Device = device
type Data =
    {
        gridSize: int
    }
[<ReflectedDefinition>]
type Shader(data: Data, grid: int[], output: int[]) =
    // let (|NotNull|) value = if value = null then failwith "" else value
    // let printValue (NotNull value) = ""
    [<Compute; WorkgroupSize 1>]
    member this.main([<BuiltIn(Builtin'.global_invocation_id)>] globalId: vec3<uint>) =
        output[int globalId.x] <- int globalId.x * output[int globalId.x] * data.gridSize
        
    interface WgslShader
    

[<Test>]
let ``refactoring : run a compute shader with automatic setup`` () =
    Environment.SetEnvironmentVariable("RUST_BACKTRACE", "full")
    let wgpu = new WebGPU'(WebGPU.GetApi())
    let device = wgpu.Device
    let binds = wgpu.CreateBinder Shader
    let result = compileModule Shader
    let code = Compiler.Print.module' result
    printfn $"{code}"
    let gridSize = 20
    let n = gridSize * gridSize
    let bufferUsage = BufferUsage.CopySrc ||| BufferUsage.CopyDst ||| BufferUsage.Storage
    let numsArray = [| for i in 1..n do i |]
    let mapBuffer = Array.zeroCreate numsArray.Length
    let (data, binds) =
        Wgpu.Bind binds
            { size = 4; isUniform = false; usage = bufferUsage }
            (Shaders.makeSerialize ())
    let (grid, binds) =
        Wgpu.Bind binds
            { size = (gridSize * gridSize) * sizeof<int>; isUniform = false; usage = bufferUsage }
            BitConverter.GetBytes
    let (output, binds) =
        Wgpu.MapS binds wgpu device.Device
            { size = (gridSize * gridSize) * sizeof<int>; isUniform = false; usage = bufferUsage }
            BitConverter.GetBytes
    // let group = device.CreateCompute shader "main" binds
    let group = device.CreateCompute code "main" binds
    let encoder = device.CreateCommandEncoder()
    let computePassEncoder = encoder.BeginComputePass()
    computePassEncoder.SetPipeline group.pipeline
    computePassEncoder.SetBindGroup group.bindGroup 0u
    computePassEncoder.DispatchWorkgroups (uint numsArray.Length) 1u 1u
    computePassEncoder.End ()
    
    output.AddCopy (wgpu, encoder.Encoder)
    
    let mutable commandBuffer = wgpu.EncoderFinish(encoder.Encoder)
    let queue = wgpu.DeviceGetQueue(device.Device)
    
    data.Write (wgpu, queue, { gridSize = 20 })
    output.Write (wgpu, queue, 0uL, numsArray)
    wgpu.QueueSubmit(queue, unativeint 1, &&commandBuffer)
    let map = output.MapAsync wgpu
    
    wgpu.DevicePoll(device.Device, true, Unchecked.defaultof<_>) |> ignore
    let mapData = map.Result
    use ptr = fixed mapBuffer
        // Marshal.Copy(NativePtr.toNativeInt mapData, 0, NativePtr.toNativeInt ptr, 4 * numsArray.Length)
    NativePtr.copyBlock ptr mapData numsArray.Length
    printfn $"{mapData}"
    printfn $"{mapBuffer}"

    // while loop do Thread.Sleep 100
    do // todo Cleanup
        // wgpu.BufferUnmap(staging_buffer); todo
        wgpu.CommandBufferRelease(commandBuffer);
        wgpu.ComputePassEncoderRelease(computePassEncoder.Encoder);
        wgpu.CommandEncoderRelease(encoder.Encoder);
        wgpu.BindGroupRelease(group.bindGroup);
        wgpu.BindGroupLayoutRelease(group.bindGroupLayout);
        wgpu.ComputePipelineRelease(group.pipeline);
        // wgpu.BufferRelease(storage_buffer); todo
        // wgpu.BufferRelease(staging_buffer); todo
        wgpu.ShaderModuleRelease(group.shaderModule);
        wgpu.QueueRelease(queue);
        wgpu.DeviceRelease(device.Device);
        // wgpu.AdapterRelease(adapter); todo
        // wgpu.InstanceRelease(instance); todo
    // wgpu.QueueWriteBuffer(queue, )
    
    printfn $"%A{result}"
    printfn $"{code}"
// [<Test>]

// let ``run a compute shader with automatic setup`` () =
//     Environment.SetEnvironmentVariable("RUST_BACKTRACE", "full")
//     let wgpu = WebGPU.GetApi()
//     let instance = wgpu.CreateInstance()
//     let adapter = wgpu.RequestAdapterAsync(instance).Result
//     let device = wgpu.RequestDeviceAsync(adapter).Result
//     let binder = wgpu.CreateBinder Shader
//     let result = compileModule Shader
//     let code = Compiler.Print.module' result
//     printfn $"{code}"
//     let (numsAsdf, binds) =
//         Wgpu.Map(binder)
//             { size = 100; isUniform = false; usage = BufferUsage.CopySrc ||| BufferUsage.CopyDst ||| BufferUsage.Storage }
//             (Shaders.makeSerialize typeof<_>)
//             // BitConverter.GetBytes
//     // let (nums, binder) =
//     //     Wgpu.Bind binder
//     //         { size = 100; isUniform = false; usage = BufferUsage.CopySrc ||| BufferUsage.CopyDst ||| BufferUsage.Storage }
//     //         BitConverter.GetBytes
//     // let init = "@group(0) @binding(0) var<storage, read_write> output: array<i32>;" + "\n"
//     // let group = wgpu.CreateCompute (init + "@compute\n@workgroup_size(1)\n" + code) "main" device binder
//     let group = wgpu.CreateCompute shader "main" device binds
//     let encoder = wgpu.CreateCommandEncoder(device)
//     let computePassEncoder = wgpu.EncoderBeginComputePass(encoder)
//     
//     wgpu.ComputePassEncoderSetPipeline(computePassEncoder, group.pipeline);
//     wgpu.ComputePassEncoderSetBindGroup(computePassEncoder, 0u, group.bindGroup, unativeint 0, Unchecked.defaultof<nativeptr<_>>)
//     let numbers_length = uint 0u // todo numbersArray.Length
//     wgpu.ComputePassEncoderDispatchWorkgroups(computePassEncoder, numbers_length, 1u, 1u)
//     wgpu.ComputePassEncoderEnd(computePassEncoder)
//     
//     numsAsdf.AddCopy (wgpu, encoder)
//     
//     let mutable commandBuffer = wgpu.EncoderFinish(encoder)
//     let queue = wgpu.DeviceGetQueue(device)
//     
//     numsAsdf.Write (wgpu, queue, 0uL, [||])
//     // wgpu.QueueWriteBuffer(queue, storage_buffer, 0uL, NativePtr.toVoidPtr numbers, unativeint numbers_size);
//     wgpu.QueueSubmit(queue, unativeint 1, &&commandBuffer)
//     let map = numsAsdf.MapAsync wgpu
//     // wgpu.BufferMapAsync(staging_buffer, MapMode.Read, unativeint 0, unativeint numbers_size,
//                         // handle_buffer_map, Unchecked.defaultof<_>)
//     // wgpu.DevicePoll(device, true, NULL)
//     
//     match wgpu.Context.TryGetProcAddress("wgpuDevicePoll") with
//     | true, poll ->
//         let fn = Marshal.GetDelegateForFunctionPointer<PollDelegate> poll
//         let result = fn.Invoke(device, true, nativeint 0)
//         let mapData = map.Result
//         // let resultBuffer =
//         //     wgpu.BufferGetMappedRange(staging_buffer, unativeint 0, unativeint numbers_size)
//         //     |> NativePtr.ofVoidPtr<float32>
//         // let numbersRead = [|
//         //     for i in 0..numbersArray.Length - 1 do
//         //         NativePtr.get resultBuffer i
//         // |]
//         // printfn $"%A{numbersRead}"
//         printfn $"{mapData}"
//     | false, _ -> ()
//
//     // while loop do Thread.Sleep 100
//     do // todo Cleanup
//         // wgpu.BufferUnmap(staging_buffer);
//         wgpu.CommandBufferRelease(commandBuffer);
//         wgpu.ComputePassEncoderRelease(computePassEncoder);
//         wgpu.CommandEncoderRelease(encoder);
//         wgpu.BindGroupRelease(group.bindGroup);
//         wgpu.BindGroupLayoutRelease(group.bindGroupLayout);
//         wgpu.ComputePipelineRelease(group.pipeline);
//         // wgpu.BufferRelease(storage_buffer); todo
//         // wgpu.BufferRelease(staging_buffer); todo
//         wgpu.ShaderModuleRelease(group.shaderModule);
//         wgpu.QueueRelease(queue);
//         wgpu.DeviceRelease(device);
//         wgpu.AdapterRelease(adapter);
//         wgpu.InstanceRelease(instance);
//     // wgpu.QueueWriteBuffer(queue, )
//     
//     printfn $"%A{result}"
//     printfn $"{code}"
