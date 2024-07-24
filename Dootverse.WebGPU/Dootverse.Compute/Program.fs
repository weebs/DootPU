module Program

open System
open System.Runtime.InteropServices
open System.Threading
open Dootverse.WebGPU
open Dootverse.WebGPU.Wgsl
open Microsoft.FSharp.NativeInterop
open Microsoft.FSharp.Quotations
open Silk.NET.WebGPU

type PollDelegate = delegate of nativeptr<Device> * bool * nativeint -> bool

let shaderCode = """
@group(0)
@binding(0)
var<storage, read_write> v_indices: array<vec3f>; // this is used as both input and output for convenience

// The Collatz Conjecture states that for any integer n:
// If n is even, n = n/2
// If n is odd, n = 3n+1
// And repeat this process for each new n, you will always eventually reach 1.
// Though the conjecture has not been proven, no counterexample has ever been found.
// This function returns how many times this recurrence needs to be applied to reach 1.
fn collatz_iterations(n_base: u32) -> u32{
    var n: u32 = n_base;
    var i: u32 = 0u;
    loop {
        if (n <= 1u) {
            break;
        }
        if (n % 2u == 0u) {
            n = n / 2u;
        }
        else {
            // Overflow? (i.e. 3*n + 1 > 0xffffffffu?)
            if (n >= 1431655765u) {   // 0x55555555u
                return 4294967295u;   // 0xffffffffu
            }

            n = 3u * n + 1u;
        }
        i = i + 1u;
    }
    return i;
}

@compute
@workgroup_size(1)
fn main(@builtin(global_invocation_id) global_id: vec3<u32>) {
    // v_indices[global_id.x] = global_id.x * u32(2);
    v_indices[global_id.x] = vec3f(f32(global_id.x) * 2.0);
}
"""
    
// let inline addressof value =
//     &value

// https://github.com/gfx-rs/wgpu-rs/blob/master/examples/hello-compute/main.rs
// todo https://github.com/gfx-rs/wgpu-native/blob/trunk/examples/compute/main.c
let main () =
    let wgpu = WebGPU.GetApi()
    let descriptor = InstanceDescriptor()
    let instance = wgpu.CreateInstance(&descriptor)
    let request = RequestAdapterOptions()
    let adapter = wgpu.RequestAdapterAsync(instance, request).Result
    let callback = new PfnDeviceLostCallback(DeviceLostCallback(fun reason arg1 arg2 -> ()))
    let descriptor = DeviceDescriptor(DeviceLostCallback = callback)
    let device = wgpu.RequestDeviceAsync(adapter, descriptor).Result
    let shaderModule =
        let mutable wgslDesc = ShaderModuleWGSLDescriptor(
             Chain = ChainedStruct(SType = SType.ShaderModuleWgslDescriptor),
             Code = C.string shaderCode
            // NextInChain = &&wgslDesc
        )
        let mutable desc = ShaderModuleDescriptor(NativePtr.ofNativeInt (NativePtr.toNativeInt &&wgslDesc))
        wgpu.DeviceCreateShaderModule(device, &&desc)
    let compute_pipeline =
        let desc = ComputePipelineDescriptor(
            Compute = ProgrammableStageDescriptor(
                Module = shaderModule,
                EntryPoint = C.string "main"
            )
        )
        wgpu.DeviceCreateComputePipeline(device, &desc)
    let bindGroupLayout = wgpu.ComputePipelineGetBindGroupLayout(compute_pipeline, 0u)
    let stagingSize = 420uL
    let numbersArray = [| 1; 2; 3; 4 |]
    use numbers = fixed numbersArray
    let numbers_size = numbersArray.Length * sizeof<uint>
    let storageSize = uint64 numbers_size
    let storage_buffer =
        wgpu.CreateBuffer(device, BufferDescriptor(
            Usage = (BufferUsage.Storage ||| BufferUsage.CopyDst ||| BufferUsage.CopySrc),
            Size = storageSize,
            MappedAtCreation = false
        ))
    let bindGroup =
        let entries = [|
            BindGroupEntry(Binding = 0u, Buffer = storage_buffer, Offset = 0uL, Size = storageSize)
        |]
        wgpu.CreateBindGroup(device, bindGroupLayout, entries)
    let command_encoder = 
        wgpu.CreateCommandEncoder(device, CommandEncoderDescriptor())
    let compute_pass_encoder = 
        wgpu.EncoderBeginComputePass(command_encoder, ComputePassDescriptor())
    wgpu.ComputePassEncoderSetPipeline(compute_pass_encoder, compute_pipeline);
    wgpu.ComputePassEncoderSetBindGroup(compute_pass_encoder, 0u, bindGroup, unativeint 0, Unchecked.defaultof<nativeptr<_>>)
    let numbers_length = uint numbersArray.Length
    wgpu.ComputePassEncoderDispatchWorkgroups(compute_pass_encoder, numbers_length, 1u, 1u)
    wgpu.ComputePassEncoderEnd(compute_pass_encoder);

    let staging_buffer =
        wgpu.CreateBuffer(device, BufferDescriptor(
            Usage = (BufferUsage.MapRead ||| BufferUsage.CopyDst),
            Size = stagingSize,
            MappedAtCreation = false
        ))
    wgpu.CommandEncoderCopyBufferToBuffer(command_encoder, storage_buffer, 0uL,
                                        staging_buffer, 0uL, uint64 numbers_size);

    let mutable command_buffer = 
        wgpu.EncoderFinish(command_encoder, CommandBufferDescriptor())

    let queue = wgpu.DeviceGetQueue(device)
    wgpu.QueueWriteBuffer(queue, storage_buffer, 0uL, NativePtr.toVoidPtr numbers, unativeint numbers_size);
    wgpu.QueueSubmit(queue, unativeint 1, &&command_buffer)

    let handle_buffer_map = new PfnBufferMapCallback(fun status userData ->
        printfn $"{status}"
        ())
    wgpu.BufferMapAsync(staging_buffer, MapMode.Read, unativeint 0, unativeint numbers_size,
                        handle_buffer_map, Unchecked.defaultof<_>)
    // wgpu.DevicePoll(device, true, NULL)
    
    match wgpu.Context.TryGetProcAddress("wgpuDevicePoll") with
    | true, poll ->
        let fn = Marshal.GetDelegateForFunctionPointer<PollDelegate> poll
        let result = fn.Invoke(device, true, nativeint 0)
        let resultBuffer =
            wgpu.BufferGetMappedRange(staging_buffer, unativeint 0, unativeint numbers_size)
            |> NativePtr.ofVoidPtr<float32>
        let numbersRead = [|
            for i in 0..numbersArray.Length - 1 do
                NativePtr.get resultBuffer i
        |]
        printfn $"%A{numbersRead}"
    | false, _ -> ()

    // while loop do Thread.Sleep 100
    wgpu.BufferUnmap(staging_buffer);
    wgpu.CommandBufferRelease(command_buffer);
    wgpu.ComputePassEncoderRelease(compute_pass_encoder);
    wgpu.CommandEncoderRelease(command_encoder);
    wgpu.BindGroupRelease(bindGroup);
    wgpu.BindGroupLayoutRelease(bindGroupLayout);
    wgpu.ComputePipelineRelease(compute_pipeline);
    wgpu.BufferRelease(storage_buffer);
    wgpu.BufferRelease(staging_buffer);
    wgpu.ShaderModuleRelease(shaderModule);
    wgpu.QueueRelease(queue);
    wgpu.DeviceRelease(device);
    wgpu.AdapterRelease(adapter);
    wgpu.InstanceRelease(instance);
    ()

// Thread(ThreadStart(fun () -> main ())).Start()
// while loop do Thread.Sleep 2000
main ()