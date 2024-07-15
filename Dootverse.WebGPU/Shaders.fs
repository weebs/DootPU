module Dootverse.WebGPU.Shaders

open System
open System.Diagnostics
open System.Reflection
open Dootverse.WebGPU.Compiler
open Dootverse.WebGPU.Wgsl
open Microsoft.FSharp.Quotations
open type Dootverse.WebGPU.Wgsl.Wgsl
open Microsoft.FSharp.Reflection
open Silk.NET.WebGPU

open System.Reflection
open Microsoft.FSharp.Reflection
            
    // fun (b: 'b) -> ()
// open Setup
        
type ComputeShaderInstance() =
    class end
    
type WebGpu(wgpu: WebGPU) =
    class end

type output = {
    [<BuiltIn(Builtin'.position)>] position: vec4<float32>
    [<Location(0)>] xy: vec2<float32>
}

type Shape =
    | Sphere of vec3f * float32
    | Cube of vec3f * float32
    | RoundedCube of vec3f * float32 * float32
    
// type Voxel =
//     | Entry of count: int * arrayIndex: int
//     | None

[<ReflectedDefinition>]
type Raymarching =
    // static member sdfSphere (sphere: vec3f, radius: )
    static member Fragment (shapes: Shape array) =
        let sdfSphere (sphere: vec3f, radius: float32, point: vec3f) =
            length(sphere - point) - radius
        vec4(0f)
        
type Screen = {
    gridSize: int
    voxelGridSize: int
    voxelGridScale: float32
    posX: float32
    posY: float32
    width: float32
    height: float32
}
let sizeofWgslType<'t> () =
    if FSharpType.IsRecord typeof<'t> then FSharpType.GetRecordFields typeof<'t> |> _.Length |> (*) 4
    else Debugger.Break(); failwith ""
type Voxel = { startIndex: int; count: int }
let FragmentShader fn = fn
let VertexShader fn = fn
let Location n fn = fn
let BuiltIn (b: Builtin) value = value
let Var (items: VarType list) value = value

        
let rec frag = Shader.createFragment shader'
// and shader' (screen: Screen, circles: float32[]) = <@
// and shader' = <@ fun (Screen: Screen, Circles: float32[]) ->
and shader' = <@ fun (Screen: Screen, Shapes: Shape[], Voxels: Voxel[], VoxelData: int[]) ->
    let screen = Var [Uniform] Screen
    let shapes = Var [Storage; ReadWrite] Shapes
    let voxels = Var [Storage; ReadWrite] Voxels
    let voxelData = Var [Storage; ReadWrite] VoxelData
    
    let distanceSphere (sphere: vec3f) (radius: float32) (point: vec3f) : float32 =
        length(sphere - point) - radius
        
    let sphereNormal (origin: vec3f) (radius: float32) (p: vec3f) =
        let e = vec2(0f, 0.0001f)
        
        normalize(vec3(
            (distanceSphere origin radius (p + e.yxx)) - (distanceSphere origin radius (p - e.yxx)),
            (distanceSphere origin radius (p + e.xyx)) - (distanceSphere origin radius (p - e.xyx)),
            (distanceSphere origin radius (p + e.xxy)) - (distanceSphere origin radius (p - e.xxy))
        ))
    
    let vertex = VertexShader (BuiltIn Builtin.VertexIndex (fun (index: uint32) -> 
        let pos = [|
            vec2(-1f, 1f)
            vec2(-1f, -1f)
            vec2(1f, -1f)
            
            vec2(1f, 1f)
            vec2(-1f, 1f)
            vec2(1f, -1f)
        |]
        let n = int index
        {
            position = vec4(pos[n], 0f, 1f)
            xy = vec2(pos[n].x,pos[n].y)
        }
    ))
    let sdfBox (box: vec3f) (size: float32) (point: vec3f) : float32 =
        let p = box - point
        let q = abs(p) - vec3(size)
        length(max(q,vec3(0f, 0f, 0f))) + min(max(q.x,max(q.y,q.z)),0f)
    let sdfRoundedBox (b: vec3f) (size: float32) (r: float32) (point: vec3f) =
        // let dimensions = vec3(size)
        // let p = point - b
        // let q = abs(p) - dimensions + r
        // length(max(q,vec3(0f, 0f, 0f))) + min(max(q.x,max(q.y,q.z)),0f) - r
        let p = point - b
        let q = abs(p) - vec3(size) + r;
        length(max(q,vec3(0f))) + min(max(q.x,max(q.y,q.z)),0f) - r;
        
    let shapeDistance (shape: Shape) (point: vec3f) : float32 =
        match shape with
        | Sphere(v3, f) -> distanceSphere v3 f point
        | Cube(v3, f) -> sdfBox v3 f point
        | RoundedCube(v3, f, r) -> sdfRoundedBox v3 f r point
        
    let shapePosition (shape: Shape) =
        match shape with
        | Sphere(v3, f) -> v3
        | Cube(v3, f) -> v3
        | RoundedCube(v3, f, f1) -> v3
    
    let fastVoxelDda (pos: vec3f) (rayDir: vec3f) =
        // let pos = vec3(0f)
        let foo = rayDir / rayDir
        let map = floor(pos / screen.voxelGridScale)
        // let rayDir = pixelPosition - cameraOrigin
        let absRayDir = abs(rayDir)
        let deltaDist = 1f / absRayDir
        let S = step(vec3(0f), rayDir)
        let stepDir = 2f * S - 1f
        let sideDist = (S - stepDir * (pos - map)) * deltaDist
        let conditions = step(sideDist.xxyy, sideDist.yzzx)
        let cases = vec3(0f)
        cases.x <- conditions.x * conditions.y
        cases.y <- (1f - cases.x) * conditions.z * conditions.w
        cases.z <- (1f - cases.x) * (1f - cases.y)
        let newDist = max((2f * cases - 1f) * deltaDist, vec3(0f))
        let offset = cases * stepDir
        round(cases * rayDir / abs(rayDir))
        // round(cases * deltaDist)
    
    let voxelShapeDistance (pos: vec3f) =
        let quadrant = floor(pos / screen.voxelGridScale)
        0f
        
    let getVoxelIndex x y z =
        int z +
        (int y * screen.voxelGridSize) +
        (int x * screen.voxelGridSize * screen.voxelGridSize)
        
    let nextVoxel (start: vec3f) (dir: vec3f) (normalizedDir: vec3f) (scalingVec: vec3f) =
        let offset = fastVoxelDda start dir
        let globalOffset = vec3(0.5f * screen.voxelGridScale * float32 screen.voxelGridSize)
        let mutable pos = start
        let mutable voxel = floor(pos / screen.voxelGridScale)
        let voxelFloat = round(voxel + globalOffset)
        let mutable voxelIndex = getVoxelIndex voxelFloat.x voxelFloat.y voxelFloat.z
        // while voxels[voxelIndex].count = 0 && voxelIndex < arrayLength(voxels) do
        while voxels[voxelIndex].count = 0 && voxelIndex <> -1 do
            let nextVoxel = floor(pos / screen.voxelGridScale) + offset
            let diff = nextVoxel - pos
            voxel <- nextVoxel
            let relativeScaling = (offset * diff * scalingVec)
            let distance = relativeScaling.x + relativeScaling.y + relativeScaling.z
            let nextPosition = pos + (distance * normalizedDir)
            pos <- nextPosition
            let voxelFloat = round(voxel + globalOffset)
            voxelIndex <- getVoxelIndex voxelFloat.x voxelFloat.y voxelFloat.z
        // if voxelIndex = arrayLength(voxels) then
        if voxelIndex = -1 then
            vec4(pos, -1f)
        else
            vec4(pos, float32 voxelIndex)
    
    let voxelGroupDistance (groupId: int) (pos: vec3f) =
        let info = voxels[groupId]
        let mutable index = 0
        let mutable distance = 1000f
        while index < info.count && distance > 0.01f do
            let shape = shapes[voxelData[info.startIndex + index]]
            distance <- min(shapeDistance shape pos, distance)
            index <- index + 1
        distance
        
    let voxelDistance (pos: vec3f) (dir: vec3f) (normalized: vec3f) (scalingVec: vec3f) =
        let next = nextVoxel pos dir normalized scalingVec
        let sdf = voxelGroupDistance (int next.w) pos
        let offset = length(vec3(next.x, next.y, next.z) - pos)
        offset + sdf
        
    let fragment = FragmentShader begin fun (output: output) -> Location 0 begin
        let metersPerPixel = 1f / 500f
        let fl = 4f
        // let cameraOrigin = vec3(0f, 0f, -fl)
        let cameraOrigin = vec3(screen.posX, screen.posY, -fl)
        let pixelPosition = vec3(
            output.xy.x * metersPerPixel * screen.width,
            output.xy.y * metersPerPixel * screen.height, 
            0f)
        let dir = pixelPosition - cameraOrigin
        let normalized = normalize(dir)
        let len = length(dir)
        let lenX = len / normalized.x
        let lenY = len / normalized.y
        let lenZ = len / normalized.z
        let scalingVec = vec3(lenX, lenY, lenZ)
        
        // let nextIndex = nextVoxel pos dir normalized scalingVec
        // vec4(vec3(float32 nextIndex), 1f)
        
        let mutable pos = pixelPosition
        let mutable iteration = 0
        let mutable distance = voxelDistance pos dir normalized scalingVec
        while distance > 0.01f && distance < 100f && iteration < 1000 do
            iteration <- iteration + 1
            pos <- pos + (distance * normalized)
            distance <- voxelDistance pos dir normalized scalingVec
        pos <- pos + (distance * normalized)
        
        if distance <= 0.01f then
            vec4(1f)
        else
            vec4(vec3(0f), 1f)
        
        // if offset.x = 1f then
        //     if dir.x > 0f then
        //         vec4(1f, 0f, 0f, 0f)
        //     else
        //         if (dir / dir).x = -1f then
        //             vec4(1f, 1f, 0f, 0f)
        //         else
        //             vec4(1f, 1f, 1f, 0f)
        // elif abs(1f - offset.y) < 0.01f then
        //     vec4(0f, 1f, 0f, 0f)
        // elif abs(1f - offset.z) < 0.01f then
        //     vec4(0f, 0f, 1f, 0f)
        // elif abs(offset.z) > 0.01f then
        //     vec4(0f, 0f, 1f, 0f)
        // else
        //     vec4(0f)
        
            
        // Check if the next position
        
        
        // if abs(1f - nextVoxel.x) < 0.01f then
        // vec4(nextVoxel * 0.2f, 1f)
    end end
    // {|
        // fragment = fun (output: VertexOutput) ->
    
        
    let shapeNormal (shape: Shape) (p: vec3f) =
        // let e = vec2(0f, 0.0001f)
        // let e = vec2(0.0001f, 0f)
        //
        // normalize(vec3(
        //     // (shapeDistance shape (p + e.yxx)) - (shapeDistance shape (p - e.yxx)),
        //     // (shapeDistance shape (p + e.xyx)) - (shapeDistance shape (p - e.xyx)),
        //     // (shapeDistance shape (p + e.xxy)) - (shapeDistance shape (p - e.xxy))
        //     (shapeDistance shape (p + e.xyy)) - (shapeDistance shape (p - e.xyy)),
        //     (shapeDistance shape (p + e.yxy)) - (shapeDistance shape (p - e.yxy)),
        //     (shapeDistance shape (p + e.yyx)) - (shapeDistance shape (p - e.yyx))
        // ))
        let h = 0.001f; // replace by an appropriate value
        let k = vec2(1f,-1f);
        normalize(
            k.xyy* shapeDistance shape ( p + k.xyy*h ) + 
            k.yyx* shapeDistance shape ( p + k.yyx*h ) + 
            k.yxy* shapeDistance shape ( p + k.yxy*h ) + 
            k.xxx* shapeDistance shape ( p + k.xxx*h ) );
        
    let getDistanceWithNormal (point: vec3f) : vec4<float32> =
        let mutable minDistance = 1000000f
        let mutable index = 0
        let mutable normalValue = vec3(0f)
        while index < 100 && minDistance > 0.01f do
            let shape = shapes[index]
            index <- index + 1
            let distance = shapeDistance shape point
            minDistance <- min(minDistance, distance)
            if minDistance <= 0.01f then
                normalValue <- shapeNormal shape point
                let v = shapePosition shape
                normalValue <- sphereNormal v 0.422f point
            // match shape with
            // | Sphere(v3, f) ->
            //     // minDistance <- min(1000f, minDistance)
            //     minDistance <- min(1000f, minDistance)
            //     // minDistance <- min(distanceSphere v3 f point, minDistance)
            //     minDistance <- min(sdfRoundedBox v3 f 1f point, minDistance)
            // | Cube(v3, f) ->
            //     // minDistance <- min(sdfBox v3 f point, minDistance)
            //     minDistance <- min(1000f, minDistance)
            // | RoundedCube(v3, f, f1) ->
            //     minDistance <- min(sdfRoundedBox v3 f f1 point, minDistance)
        // normalValue.z <- minDistance
        // normalValue
        // vec4(normalValue.x, normalValue.y, normalValue.z, minDistance)
        vec4(normalValue, minDistance)
    let sphereShader (output: output) =
        vec4(0f)
        // minDistance
    let normalForRoundedBox b size round p =
        let h = 0.001f; // replace by an appropriate value
        let k = vec2(1f,-1f);
        normalize(
            k.xyy* sdfRoundedBox b size round ( p + k.xyy*h ) + 
            k.yyx* sdfRoundedBox b size round ( p + k.yyx*h ) + 
            k.yxy* sdfRoundedBox b size round ( p + k.yxy*h ) + 
            k.xxx* sdfRoundedBox b size round ( p + k.xxx*h ) );
    let fragment1 = FragmentShader (fun (output: output) -> Location 0 (
        let metersPerPixel = 1f / 500f
        let fl = 4f
        let cameraOrigin = vec3(0f, 0f, -fl)
        let pixelPosition = vec3(
            output.xy.x * metersPerPixel * screen.width,
            output.xy.y * metersPerPixel * screen.height, 
            0f)
        let sphere = vec3(screen.posX, screen.posY, 4f)
        // let mutable distance = length(sphere - pixelPosition) - 1f
        let dir = normalize(pixelPosition - cameraOrigin)
        let mutable pos = pixelPosition
        // let mutable distance = length (sphere - pos) - 1f
        let mutable distance = sdfRoundedBox sphere 1f 0.4f pos
        while distance > 0.01f && distance < 100f do
            pos <- pos + (dir * distance)
            distance <- sdfRoundedBox sphere 1f 0.4f pos
            // distance <- length(sphere - pos) - 1f
            // distance <- getDistance pos
        pos <- pos + (dir * distance)
        // distance <- length(sphere - pos) - 1f
        distance <- sdfRoundedBox sphere 1f 0.4f pos
        if distance <= 1f then
            // let n = sphereNormal sphere 1f pos
            let n = normalForRoundedBox sphere 1f 0.4f pos
            // let n = abs(vec3(distance.x * 0.01f, distance.y, distance.z * 0.1f))
            let color = (n + vec3(1f, 1f, 1f)) / 2f
            // let color = n
            // let color = vec3(0f, abs(distance.y), abs(distance.z))
            // let color = n
            // let color = n
            // vec4(distance.x, distance.y, distance.z, 1f)
            vec4(color, 1f)
            // vec4(1f, 1f, 1f, 1f)
        else
            // vec4(sqrt(output.xy.x), sqrt(output.xy.y), 0f, 0f)
            vec4(0f)
    ))
    let getDistance (point: vec3f) =
        let mutable minDistance = 10000f
        let mutable index = 0
        // let shapePoint = shapePosition shapes[0] 
        while index < 1000 && minDistance > 0.01f do
            let shape = shapes[index]
            index <- index + 1
            let distance = shapeDistance shape point
            minDistance <- min(minDistance, distance)
        minDistance
        // length(shapePoint - point) - 1f
    let fragment2 = FragmentShader (fun (output: output) -> Location 0 (
        // vec4((1f + output.xy.x) * 0.5f, (1f + output.xy.y) * 0.5f, 0f, 0f)
        let metersPerPixel = 1f / 500f
        let fl = 4f
        let cameraOrigin = vec3(0f, 0f, -fl)
        let pixelPosition = vec3(
            output.xy.x * metersPerPixel * screen.width,
            output.xy.y * metersPerPixel * screen.height, 
            0f)
        // let sphere = vec3(screen.posX, screen.posY, 4f)
        // let mutable distance = length(sphere - pixelPosition) - 1f
        let dir = normalize(pixelPosition - cameraOrigin)
        let shape = shapes[0]
        let shapePos = vec3(0f, 0f, 4f)
        let mutable pos = pixelPosition
        let mutable distance = distanceSphere shapePos 1f pos
        // let mutable distance = getDistance pos
        let mutable iterations = 0
        // let mutable distance = length()
        while iterations < 100 && distance > 0.01f && distance < 100f do
            iterations <- iterations + 1
            pos <- pos + (dir * distance)
            // distance <- length(sphere - pos) - 1f
            // distance <- getDistance pos
            distance <- distanceSphere shapePos 1f pos
        // pos <- pos + (dir * distance.w)
        // distance <- getDistance pos
        if distance <= 0.001f then
            // let n = sphereNormal sphere 1f pos
            // let n = vec3(distance.x * 0.01f, distance.y, distance.z * 0.1f)
            // let n = abs(vec3(distance.x * 0.01f, distance.y, distance.z * 0.1f))
            // let color = (n + vec3(1f, 1f, 1f)) / 2f
            // let color2 = vec3(output.xy.x, output.xy.y, 0f)
            // let color = vec3(0f, abs(distance.y), abs(distance.z))
            // let color = n
            // let color = n
            // vec4(distance.x, distance.y, distance.z, 1f)
            // vec4(color, 1f)
            vec4(1f, 1f, 1f, 1f)
            // vec4(1f, 1f, 1f, 1f)
        else
            // vec4(sqrt(output.xy.x), sqrt(output.xy.y), 0f, 0f)
            vec4(0f)
    ))
    let nearestObject (pos: vec3f) =
        let mutable index = 0
        let mutable result = 0
        let mutable minDistance = 10000f
        while index < 1000 do
            let distance = shapeDistance shapes[index] pos
            if distance < minDistance then
                result <- index
                minDistance <- distance
            index <- index + 1
        shapes[index]
    let fragment3 = FragmentShader (fun (output: output) -> Location 0 (
        let metersPerPixel = 1f / 500f
        let fl = 4f
        let cameraOrigin = vec3(0f, 0f, -fl)
        let pixelPosition = vec3(
            output.xy.x * metersPerPixel * screen.width,
            output.xy.y * metersPerPixel * screen.height, 
            0f)
        let hitDistance = 0.001f
        // let sphere = shapePosition shapes[0]
        // let sphere = vec3(screen.posX, screen.posY, 4f)
        // let mutable distance = length(sphere - pixelPosition) - 1f
        let dir = normalize(pixelPosition - cameraOrigin)
        let mutable pos = pixelPosition
        // let mutable distance = length (sphere - pos) - 1f
        // let mutable distance = sdfRoundedBox sphere 1f 0.4f pos
        let mutable iterations = 0
        let maxIterations = 1000
        let mutable distance = getDistance pos
        let mutable lastDistance = distance
        let mutable stepsDistanceIncreased = 0
        while iterations < maxIterations && distance > hitDistance && distance < 100f && stepsDistanceIncreased < 100 do
            pos <- pos + (dir * distance)
            iterations <- iterations + 1
            // distance <- sdfRoundedBox sphere 1f 0.4f pos
            // distance <- length(sphere - pos) - 1f
            distance <- getDistance pos
            if distance < lastDistance then
                stepsDistanceIncreased <- stepsDistanceIncreased + 1
            lastDistance <- distance
        pos <- pos + (dir * distance)
        // distance <- length(sphere - pos) - 1f
        // distance <- sdfRoundedBox sphere 1f 0.4f pos
        // distance <- getDistance pos
        if distance <= hitDistance then
            // let n = sphereNormal sphere 1f pos
            // let n = normalForRoundedBox sphere 1f 0.4f pos
            let n = shapeNormal (nearestObject pos) pos
            // let n = abs(vec3(distance.x * 0.01f, distance.y, distance.z * 0.1f))
            let color = (n + vec3(1f, 1f, 1f)) / 2f
            // let color = n
            // let color = vec3(0f, abs(distance.y), abs(distance.z))
            // let color = n
            // let color = n
            // vec4(distance.x, distance.y, distance.z, 1f)
            // vec4(float32 iterations / float32 maxIterations, color.y, color.x, 1f)
            vec4(color, 1f)
            // vec4(1f, 1f, 1f, 1f)
        else
            // vec4(sqrt(output.xy.x), sqrt(output.xy.y), 0f, 0f)
            // vec4(float32 iterations / float32 maxIterations, 0f, 0f, 0f)
            vec4(0f)
    ))
    vertex, fragment
@>

let asdf = <@
    fun (a: int) ->
        1
@>
// type Result = Hit of x: int * y: int * z: int | None
// <@
// match Hit (1, 2, 3) with
// | Hit (x, y, z) -> x + y + z
// | None -> if 1 = 1 then 0 else 2
// @>

// let compiledWgsl = translateModule (shader' ({ gridSize = 0; posX = 0f; posY = 0f; width = 0f; height = 0f }, [||]))
let compiledWgsl = translateModule shader' Module.empty
        
// let output = Print.module' compiledWgsl
