module rec Dootverse.WebGPU.Wgsl

open System
open System.Diagnostics
open System.Numerics
open Microsoft.FSharp.Core
open Microsoft.FSharp.Reflection
open type Quotations.Expr
module Patterns = Quotations.Patterns


[<AbstractClass>]
type WgslAttribute() =
    inherit Attribute()
    abstract member Serialize : string
type LocationAttribute(index: int) =
    inherit WgslAttribute()
    override this.Serialize = $"@location({index})"
type VarType =
    | Uniform
    | Storage
    | ReadWrite
type Builtin =
    | Position 
    | VertexIndex
type Builtin' =
    | position = 0
    | vertexIndex = 1
// let Position = { new Builtin with member _.Foo = () }
// type BuiltInAttribute<'t when 't :> Builtin>(name: string) = inherit Attribute()
type BuiltInAttribute(value: Builtin') =
    inherit WgslAttribute()
    override this.Serialize = $"@builtin({value})"
type vec2<'t when
    't :> IAdditionOperators<'t, 't, 't> and
    't :> IMultiplyOperators<'t, 't, 't> and
    't :> IDivisionOperators<'t, 't, 't> and
    't :> ISubtractionOperators<'t, 't, 't>>
    = { x: 't; y: 't }
    with
    member this.xxy = Operators.Unchecked.defaultof<vec3<'t>>
    member this.yxx = Operators.Unchecked.defaultof<vec3<'t>>
    member this.xyx = Operators.Unchecked.defaultof<vec3<'t>>
and vec4<'t when 't :> IAdditionOperators<'t, 't, 't> and 't : (static member (-) : 't * 't -> 't)> = 
    { x: 't; y: 't; z: 't; a: 't }
// type vec4<'t>(x: 't, y: 't, z: 't, a: 't) =
//     member this.X with get () = x and set value = ()
//     member this.Y with get () = y and set value = ()
//     member this.Z with get () = z and set value = ()
//     member this.A with get () = a and set value = ()
and vec3<'t when
    't :> IAdditionOperators<'t, 't, 't> and
    't :> IMultiplyOperators<'t, 't, 't> and
    't :> ISubtractionOperators<'t, 't, 't> and
    't :> IDivisionOperators<'t, 't, 't>
    // 't : (static member (-) : 't * 't -> 't)
    // (x: 't, y: 't, z: 't) =
    > = { x: 't; y: 't; z: 't } with
    static member (+)
        (v3: vec3<'t>, v3': vec3<'t>) = { x = v3.x + v3'.x; y = v3.y + v3'.y; z = v3.z + v3'.z }
    static member (*)
        (v3: vec3<'t>, scale: 't) = { x = v3.x * scale; y = v3.y * scale; z = v3.z * scale }
    static member (*)
        (scale: 't, v3: vec3<'t>) = { x = v3.x * scale; y = v3.y * scale; z = v3.z * scale }
    static member (/)
        (v3: vec3<'t>, scale: 't) = { x = v3.x / scale; y = v3.y / scale; z = v3.z / scale }
    static member op_Multiplication
        (v3: vec3<'t>, scale: 't) = { x = v3.x * scale; y = v3.y * scale; z = v3.z * scale }
    static member op_Multiplication
        (scale: 't, v3: vec3<'t>) = { x = v3.x * scale; y = v3.y * scale; z = v3.z * scale }
    static member op_Subtraction
        // <^a when
        // ^a :> IAdditionOperators<^a,^a,^a> and
        // ^a : (static member (-) : ^a * ^a -> ^a)>
        // (v3: vec3<^a>, v3': vec3<^a>) =
        (v3: vec3<'t>, v3': vec3<'t>) =
        { x = v3.x - v3'.x; y = v3.y - v3'.y; z = v3.z - v3'.z }
    // member this.x with get () = x and set value = ()
    // member this.y with get () = y and set value = ()
    // member this.z with get () = z and set value = ()
type vec3f = vec3<float32>
type mat2x2<'t>(mx: 't, my: 't, ma: 't, mb: 't) =
    static member (*) (m: mat2x2<float32>, b: float32) = m
type Foo<'t> = 't
type Wgsl =
    static member inline vec2(a, b) = { x = a; y = b; }
    static member inline vec3(a, b, c) = { x = a; y = b; z = c }
    static member inline vec4(a, b, c, d) = { x = a; y = b; z = c; a = d; }
    static member vec4(value: float32) = Wgsl.vec4(value, value, value, value)
    static member vec4(value: int32) = Wgsl.vec4(value, value, value, value)
    static member vec4(value: vec2<float32>, a, b) = Wgsl.vec4(value.x, value.y, a, b)
    static member vec4(value: vec2<int32>, a, b) = Wgsl.vec4(value.x, value.y, a, b)
    static member vec4(value: uint32) = Wgsl.vec4(value, value, value, value)
    static member vec4(value: vec3<int32>, a) = Wgsl.vec4(value.x, value.y, value.z, a)
    static member vec4(value: vec3<float32>, a) = Wgsl.vec4(value.x, value.y, value.z, a)
    static member length (v3: vec3f) = MathF.Sqrt((v3.x * v3.x) + (v3.y * v3.y) + (v3.z + v3.z))
    static member normalize (v3: vec3f) = v3 / Wgsl.length(v3)
    static member sqrt f = MathF.Sqrt f
