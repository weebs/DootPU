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
// open type Shaders.Setup

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
        
    // interface WgslShader
    // let (|NotNull|) value = if value = null then failwith "" else value
    // let printValue (NotNull value) = ""

type Init =
    static member setup (shader: 'a * 'b * 'c -> _) = fun (wgpu: WebGPU') ->
        let result = Setup.compileModule shader
        let code = Compiler.Print.module' result
        let binds = wgpu.CreateBinder shader
        binds
[<AutoOpen>]        
module Extensions =
    type Init with
        static member setup (shader: 'a -> _) = fun (wgpu: WebGPU') ->
            let result = Setup.compileModule shader
            let code = Compiler.Print.module' result
            let binds = wgpu.CreateBinderS shader
            code, binds
    // let bufferUsage = BufferUsage.CopySrc ||| BufferUsage.CopyDst ||| BufferUsage.Storage
    
let gridSize = 20
let n = gridSize * gridSize
[<Test>]
let ``refactoring : run a compute shader with automatic setup`` () =
    let numsArray = [| for i in 1..n do 0 |]
    Environment.SetEnvironmentVariable("RUST_BACKTRACE", "full")
    use wgpu = new WebGPU'(WebGPU.GetApi())
    let setup = Init.setup Shader wgpu
    let (data, setup) = Wgpu.Bind setup
    let (grid, setup) = Wgpu.Bind setup n
    let (output, setup) = Wgpu.Map setup n
    use compute = new Extensions.ComputePipeline("main", setup)
    compute.Begin (uint numsArray.Length, 1u, 1u)
        (fun encoder ->
            output.AddCopy(wgpu, encoder))
        (fun queue ->
            data.Write (wgpu, queue, { gridSize = 20 })
            output.Write (wgpu, queue, 0uL, numsArray))
    let fut = output.ReadBufferRange(wgpu, 0, numsArray.Length)
    let result = fut.Result
    printfn $"{result}"
    let fut' = output.ReadBufferRange(wgpu, 0, numsArray.Length)
    let result' = fut.Result
    printfn $"{result'}"

        // wgpu.BufferUnmap(staging_buffer); todo
        
        // wgpu.BufferRelease(storage_buffer); todo
        // wgpu.BufferRelease(staging_buffer); todo