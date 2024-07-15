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

type Data =
    {
        gridSize: int
    }
[<ReflectedDefinition>]
type Shader(data: Data, grid: int[], output: int[]) =
    [<Compute; WorkgroupSize 1>]
    member this.main([<BuiltIn(Builtin'.global_invocation_id)>] globalId: vec3<uint>) =
        // output[int globalId.x - 1] <- int globalId.x * output[int globalId.x - 1] * data.gridSize
        output[int globalId.x] <- int globalId.x + 4
        
    interface WgslShader
    // let (|NotNull|) value = if value = null then failwith "" else value
    // let printValue (NotNull value) = ""
    

[<Test>]
let ``refactoring : run a compute shader with automatic setup`` () =
    Environment.SetEnvironmentVariable("RUST_BACKTRACE", "full")
    use wgpu = new WebGPU'(WebGPU.GetApi())
    let device = wgpu.Device
    let result = compileModule Shader
    let code = Compiler.Print.module' result
    printfn $"{code}"
    let gridSize = 20
    let n = gridSize * gridSize
    let numsArray = [| for i in 1..n do 0 |]
    let bufferUsage = BufferUsage.CopySrc ||| BufferUsage.CopyDst ||| BufferUsage.Storage
    let binds = wgpu.CreateBinder Shader
    let (data, binds) =
        Wgpu.Bind binds
            { size = 4 * Compiler.sizeofType typeof<Data>; isUniform = false; usage = bufferUsage }
            (Shaders.makeSerialize ())
    let (grid, binds) =
        Wgpu.Bind binds
            { size = (gridSize * gridSize) * sizeof<int>; isUniform = false; usage = bufferUsage }
            BitConverter.GetBytes
    let (output, binds) =
        Wgpu.Map binds wgpu device.Device
            { size = (gridSize * gridSize) * sizeof<int>; isUniform = false; usage = bufferUsage }
            BitConverter.GetBytes
    // let group = device.CreateCompute shader "main" binds
    let compute = Extensions.ComputePipeline(device, code, "main", binds)
    // let group = device.CreateCompute code "main" binds
    // let encoder = device.CreateCommandEncoder()
    // let computePassEncoder = encoder.BeginComputePass()
    // computePassEncoder.SetPipeline group.pipeline
    // computePassEncoder.SetBindGroup group.bindGroup 0u
    // computePassEncoder.DispatchWorkgroups (32u) 1u 1u
    // computePassEncoder.End ()
    
    // todo : this is needed for copying data from the buffer on the gpu
    // todo : to a buffer that the CPU can read
    // todo: should this be automatic?
    // output.AddCopy (wgpu, encoder.Encoder)
    // let mutable commandBuffer = wgpu.EncoderFinish(encoder.Encoder) // todo
    
    // let queue = wgpu.DeviceGetQueue(device.Device)
    compute.Begin (1u, 1u, 1u)
        (fun encoder ->
            output.AddCopy(wgpu, encoder))
        (fun queue ->
            data.Write (wgpu, queue, { gridSize = 20 })
            output.Write (wgpu, queue, 0uL, numsArray))
    // do
    let fut = output.ReadBufferRange(wgpu, 0, numsArray.Length)
    let result = fut.Result
    (compute :> System.IDisposable).Dispose()
    printfn $"{result}"
    // do
    //     let map = output.MapAsync wgpu
    //     // todo : where to call DevicePoll
    //     wgpu.DevicePoll(device.Device, true) |> ignore
    //     let mapData = map.Result
    //     let mapBuffer = Array.zeroCreate numsArray.Length
    //     use ptr = fixed mapBuffer
    //         // Marshal.Copy(NativePtr.toNativeInt mapData, 0, NativePtr.toNativeInt ptr, 4 * numsArray.Length)
    //     NativePtr.copyBlock ptr mapData numsArray.Length
    //     printfn $"{mapData}"
    //     printfn $"{mapBuffer}"

    do // todo Cleanup
        // wgpu.BufferUnmap(staging_buffer); todo
        
        // wgpu.BufferRelease(storage_buffer); todo
        // wgpu.BufferRelease(staging_buffer); todo
        
        // wgpu.ShaderModuleRelease(group.shaderModule);
    
    
    printfn $"%A{result}"
    printfn $"{code}"
