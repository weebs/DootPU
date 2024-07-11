module Dootverse.WebGPU.Shaders

open Dootverse.WebGPU.Compiler
open Dootverse.WebGPU.Wgsl
open Microsoft.FSharp.Quotations
open type Dootverse.WebGPU.Wgsl.Wgsl

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
    posX: float32
    posY: float32
    width: float32
    height: float32
}
let FragmentShader fn = fn
let VertexShader fn = fn
let Location n fn = fn
let BuiltIn (b: Builtin) value = value
let Var (items: VarType list) value = value
let rec frag = Shader.createFragment shader'
// and shader' (screen: Screen, circles: float32[]) = <@
// and shader' = <@ fun (Screen: Screen, Circles: float32[]) ->
and shader' = <@ fun (Screen: Screen, Shapes: Shape[]) ->
    let screen = Var [Uniform] Screen
    let shapes = Var [Storage; ReadWrite] Shapes
    // {|
        // fragment = fun (output: VertexOutput) ->
    
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
        
    let shapePoint (shape: Shape) =
        match shape with
        | Sphere(v3, f) -> v3
        | Cube(v3, f) -> v3
        | RoundedCube(v3, f, f1) -> v3
        
    let shapeNormal (shape: Shape) (p: vec3f) =
        // let e = vec2(0f, 0.0001f)
        let e = vec2(0.0001f, 0f)
        
        (normalize(vec3(
            // (shapeDistance shape (p + e.yxx)) - (shapeDistance shape (p - e.yxx)),
            // (shapeDistance shape (p + e.xyx)) - (shapeDistance shape (p - e.xyx)),
            // (shapeDistance shape (p + e.xxy)) - (shapeDistance shape (p - e.xxy))
            (shapeDistance shape (p + e.xyy)) - (shapeDistance shape (p - e.xyy)),
            (shapeDistance shape (p + e.yxy)) - (shapeDistance shape (p - e.yxy)),
            (shapeDistance shape (p + e.yyx)) - (shapeDistance shape (p - e.yyx))
        )))
        
    let getDistance (point: vec3f) : vec4<float32> =
        let mutable minDistance = 1000000f
        let mutable index = 0
        let mutable normalValue = vec3(0f)
        while index < 100 do
            let shape = shapes[index]
            index <- index + 1
            let distance = shapeDistance shape point
            minDistance <- min(minDistance, distance)
            if minDistance <= 0.01f then
                normalValue <- shapeNormal shape point
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
        vec4(normalValue.x, normalValue.y, normalValue.z, minDistance)
        // minDistance
    let fragment = FragmentShader (fun (output: output) -> Location 0 (
        // vec4((1f + output.xy.x) * 0.5f, (1f + output.xy.y) * 0.5f, 0f, 0f)
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
        let mutable distance = getDistance pos
        while distance.w > 0.01f && distance.w < 100f do
            pos <- pos + (dir * distance.w)
            // distance <- length(sphere - pos) - 1f
            distance <- getDistance pos
        if distance.w <= 1f then
            // let n = sphereNormal sphere 1f pos
            let n = vec3(distance.x, distance.y, distance.z)
            // let color = (n + vec3(1f, 1f, 1f)) / 2f
            let color = n
            // vec4(distance.x, distance.y, distance.z, 1f)
            vec4(color, 1f)
            // vec4(1f, 1f, 1f, 1f)
        else
            // vec4(sqrt(output.xy.x), sqrt(output.xy.y), 0f, 0f)
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
        
let output = Print.module' compiledWgsl
