open System
open System.Diagnostics
open System.Numerics
open Microsoft.FSharp.Core
open Microsoft.FSharp.Reflection
module Patterns = Quotations.Patterns

type vec2<'t when
    't :> IAdditionOperators<'t, 't, 't> and
    't :> IMultiplyOperators<'t, 't, 't> and
    't :> ISubtractionOperators<'t, 't, 't>> = { x: 't; y: 't }
type vec4<'t when 't :> IAdditionOperators<'t, 't, 't> and 't : (static member (-) : 't * 't -> 't)> = 
    { x: 't; y: 't; z: 't; a: 't }
// type vec4<'t>(x: 't, y: 't, z: 't, a: 't) =
//     member this.X with get () = x and set value = ()
//     member this.Y with get () = y and set value = ()
//     member this.Z with get () = z and set value = ()
//     member this.A with get () = a and set value = ()
type vec3<'t when
    't :> IAdditionOperators<'t, 't, 't> and
    't :> IMultiplyOperators<'t, 't, 't> and
    't :> ISubtractionOperators<'t, 't, 't>
    // 't : (static member (-) : 't * 't -> 't)
    // (x: 't, y: 't, z: 't) =
    > = { x: 't; y: 't; z: 't } with
    static member (+)
        (v3: vec3<'t>, v3': vec3<'t>) = { x = v3.x + v3'.x; y = v3.y + v3'.y; z = v3.z + v3'.z }
    static member (*)
        (v3: vec3<'t>, scale: 't) = { x = v3.x * scale; y = v3.y * scale; z = v3.z * scale }
    static member (*)
        (scale: 't, v3: vec3<'t>) = { x = v3.x * scale; y = v3.y * scale; z = v3.z * scale }
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
    static member inline vec3(a, b, c) = { x = a; y = b; z = c }
    static member inline vec4(a, b, c, d) = { x = a; y = b; z = c; a = d; }
    static member vec4(value: float32) = Wgsl.vec4(value, value, value, value)
    static member vec4(value: int32) = Wgsl.vec4(value, value, value, value)
    static member vec4(value: uint32) = Wgsl.vec4(value, value, value, value)
    static member length (v3: vec3f) = MathF.Sqrt((v3.x * v3.x) + (v3.y * v3.y) + (v3.z + v3.z))
open type Wgsl
type output = { position: vec4<float32>; xy: vec2<float32> }
type Fragment<'t> = Fragment of 't
type IsFragment<'t> =
    abstract member Visit<'r> : FragmentV<'t, 'r> -> 'r
and FragmentV<'t, 'r> =
    abstract member Invoke<'a> : 't -> 'a
and Fragment_<'t> = private { value: IsFragment<'t> }
and Fragment_<'t> with
    member this.Value = this.value
let createFragment'' (frag: IsFragment<'t>) : Fragment_<'t> =
    { value = frag }
let createFragment (frag: Quotations.Expr<Fragment<'t -> 'u>>) : Fragment_<'t> =
    { value = Unchecked.defaultof<_> }
let createFragment' (frag: 'a -> Quotations.Expr<Fragment<'t -> 'u>>) : Fragment_<'t> =
    { value = Unchecked.defaultof<_> }
let runFragmentShader (frag: Fragment_<'t>) (value: 't) =
    ()
type WgslType =
    | Float
    | Int
    | Unsigned
    | Vec3f
    | DefinedType of string
and WgslConst =
    | Int of int
    | Float of float32
    | Unsigned of uint32
type WgslExpr =
    | Call of callee: string * args: WgslExpr list
    | Ident of name: string
    | PropGet of source: WgslExpr * field: string
    | Value of WgslConst
    | BinaryAnd of WgslExpr * WgslExpr
type WgslStatement =
    | ExprStatement of WgslExpr
    | WhileLoop of condition: WgslExpr * body: WgslStatement list
    | VarDeclaration of name: string * value: WgslExpr option
    | LetDeclaration of name: string * value: WgslExpr
    | IfThenElse of cond: WgslExpr * whenTrue: WgslStatement list * whenFalse: WgslStatement list
    | Assign of var: string * value: WgslExpr
    | ReturnExpr of expr: WgslExpr
type WgslAttr = string
type WgslModuleStatement =
    | Struct
    | Alias
    | Function of name: string * args: (string * WgslType) list * attrs: WgslAttr list * returning: WgslType option * statements: WgslStatement list

module rec Print =
    let call (callee: string) (args: WgslExpr list) =
        match callee with
        | "op_Multiply" -> $"({expr args[0]} * {expr args[1]})"
        | "op_Addition" -> $"({expr args[0]} + {expr args[1]})"
        | "op_Division" -> $"({expr args[0]} / {expr args[1]})"
        | "op_Subtraction" -> $"({expr args[0]} - {expr args[1]})"
        | "op_GreaterThan" -> $"({expr args[0]} > {expr args[1]})"
        | "op_GreaterThanOrEqual" -> $"({expr args[0]} >= {expr args[1]})"
        | "op_Equals" -> $"({expr args[0]} = {expr args[1]})"
        | "op_LessThan" -> $"({expr args[0]} < {expr args[1]})"
        | "op_LessThanOrEqual" -> $"({expr args[0]} <= {expr args[1]})"
        | _ ->
            let callArgs = String.concat ", " (List.map expr args)
            $"{callee}({callArgs})"
    let expr (expr': WgslExpr) =
        match expr' with
        | Call(callee, args) -> call callee args
        | Ident name -> name
        | PropGet(source, field) -> $"{expr source}.{field}"
        | Value wgslConst ->
            match wgslConst with
            | Int i -> $"{i}"
            | Float f ->
                let s = $"{f}"
                if s.Contains "." then s else s + ".0"
            | Unsigned u -> $"{u}"
        | BinaryAnd(wgslExpr, e) -> $"({expr wgslExpr} && {expr e})"
    let statement (stmt: WgslStatement) =
        match stmt with
        | ExprStatement wgslExpr -> [ $"{expr wgslExpr};" ]
        | WhileLoop(condition, body) ->
            [
                $"while {expr condition} {{"
                yield! List.map statement body |> List.collect id
                "}"
            ]
        | VarDeclaration(name, None) -> [ $"var {name}" ]
        | VarDeclaration(name, Some value) -> [ $"var {name} = {expr value};" ]
        | LetDeclaration(name, value) -> [ $"let {name} = {expr value};" ]
        | IfThenElse(cond, whenTrue, whenFalse) ->
            [
                $"if ({expr cond}) {{"
                yield! List.map statement whenTrue |> List.collect id
                $"}} else {{"
                yield! List.map statement whenFalse |> List.collect id
                "}"
            ]
        | Assign(var, value) -> [ $"{var} = {expr value};" ]
        | ReturnExpr e -> [ $"return {expr e};" ]
    let type' (t: WgslType) =
        match t with
        | WgslType.Float -> "f32"
        | WgslType.Int -> "i32"
        | WgslType.Unsigned -> "u32"
        | WgslType.Vec3f -> "vec3f"
        | WgslType.DefinedType s -> s
    let shader (compiled: WgslModuleStatement list) =
        let items = ResizeArray()
        for item in compiled do
            match item with
            | Struct -> ()
            | Alias -> ()
            | Function(name, args, attrs, returning, statements) ->
                let argsList = args |> List.map (fun (name, t) -> $"{name}: {type' t}") |> String.concat ", "
                items.Add [
                    if attrs.Length = 1 then $"@{attrs[0]}"
                    $"fn {name}({argsList})" + (match returning with None -> "" | Some t -> " -> " + type' t) + " {"
                    yield! List.map Print.statement statements |> List.collect id
                    "}"
                ]
        items
        |> Seq.map (String.concat "\n")
        |> String.concat "\n"
let translateType (t: Type) : WgslType =
    if t = typeof<int32> then WgslType.Int
    elif t = typeof<float32> then WgslType.Float
    elif t = typeof<uint32> then WgslType.Unsigned
    elif t = typeof<vec3<float32>> then WgslType.Vec3f
    else DefinedType (t.FullName.Split("+")[1])
let rec function' (arg: Quotations.Var) (expr: Quotations.Expr) : (string * WgslType) list * WgslStatement list =
    let rec parse acc = function
        | Patterns.Lambda (v, e) -> parse (v :: acc) e
        | e ->
            let args = List.rev acc |> List.map (fun v -> v.Name, translateType v.Type)
            let body = translateStatement e
            args, body
    parse [arg] expr
and call (callee: Quotations.Expr) (arg: Quotations.Expr) =
    let rec parse acc = function
        | Patterns.Application (c, a) -> parse (a :: acc) c
        | Patterns.Var v -> v.Name, acc |> List.map translateExpr
        | e -> failwith $"Unrecognized pattern in call:\n{e}"
    Call (parse [arg] callee)
and exprType (expr: Quotations.Expr) =
    match expr with
    | Patterns.ValueWithName (o, t, name) -> t
    | Patterns.Value (o, t) -> t
    | Patterns.Lambda (var, e) -> FSharpType.MakeFunctionType (var.Type, exprType e)
    | Patterns.Let (var, _,_) -> var.Type
    | Patterns.IfThenElse (_, wt, _) -> exprType wt
    | Patterns.Call (_, methodInfo, _) -> methodInfo.ReturnType
    | Patterns.Application (callee, _) ->
        snd <| FSharpType.GetFunctionElements (exprType callee)
    | Patterns.FieldGet (_, info) -> info.FieldType
    | _ -> typeof<unit>
and translateExpr (expr: Quotations.Expr) =
    match expr with
    | Patterns.IfThenElse (cond, true', Patterns.Value (o, t)) when t = typeof<bool> && (o :?> bool) = false ->
        BinaryAnd (translateExpr cond, translateExpr true')
    // | Patterns.IfThenElse (Patterns.IfThenElse ifte as cond, true', Patterns.Value (o, t)) when t = typeof<bool> && (o :?> bool) = false ->
    //     match cond with
    //     | Patterns.IfThenElse (cond)
    //     BinaryAnd (translateExpr cond, translateExpr true')
    | Patterns.Var v -> Ident v.Name
    | Patterns.Call (thisArg, methodInfo, args) ->
        Call (methodInfo.Name, List.map translateExpr args)
    | Patterns.PropertyGet (Some a, propertyInfo, exprs) ->
        PropGet (translateExpr a, propertyInfo.Name)
    | Patterns.ValueWithName (o, t, name) -> // TODO this must come before Patterns.Value
        Ident name
    | Patterns.Value (o, t) ->
        match o with
        | :? int32 as i -> Value (Int i)
        | :? float32 as f -> Value (Float f)
        | _ -> failwith $"translateExpr: Cannot translate value {o}"
    | Patterns.Application (callee, arg) ->
        call callee arg
    | Patterns.NewUnionCase (caseInfo, values) ->
        failwith $"TODO translateExpr Union: {caseInfo.DeclaringType.FullName}\n%A{values}"
    | _ -> failwith $"Unrecognized pattern in translateExpr: {expr}"
and translateStatement (statement: Quotations.Expr) =
    match statement with
    | Patterns.Application (callee, arg) -> []
    | Patterns.Let (variable, value, e) ->
        match value with
        | Patterns.Lambda (var, absExpr) ->
            [] // TODO lambda
        | _ ->
            if variable.IsMutable then
                VarDeclaration (variable.Name, Some (translateExpr value))
                :: translateStatement e
            else
                LetDeclaration (variable.Name, translateExpr value)
                :: translateStatement e
    | Patterns.IfThenElse (cond, true', false') ->
        [ IfThenElse (translateExpr cond, translateStatement true', translateStatement false') ]
    | Patterns.WhileLoop (cond, loop) ->
        [ WhileLoop (translateExpr cond, translateStatement loop) ]
    | Patterns.Sequential (e, e') ->
        translateStatement e @ translateStatement e'
    | Patterns.VarSet (var, value) ->
        [ Assign (var.Name, translateExpr value) ]
    | e -> [ ExprStatement (translateExpr e) ]
and getLambdaExprReturn e =
    match e with
    | Patterns.Lambda (_, Patterns.Lambda (_, e')) -> getLambdaExprReturn e'
    | _ -> e.Type
and translateModule (module_: Quotations.Expr) =
// and translateModule (module_: Quotations.Expr<Fragment<'t -> 'u>>) =
    match module_ with
    | Patterns.Let (v, Patterns.Lambda (arg1, absExpr), following) ->
        let args, abs = function' arg1 absExpr
        let rt_ = getLambdaExprReturn absExpr
        let rt = match rt_ with | t when t = typeof<unit> -> None | t -> Some (toType t)
        Function (v.Name, args, [], rt, addReturn abs) :: translateModule following
    | Patterns.Let (v, Patterns.Call (None, method, [ Patterns.Lambda (lambdaVar, lambdaExpr) ]), following)
        when method.Name = "FragmentShader" ->
        let args, abs = function' lambdaVar lambdaExpr
        // let rt = match exprType lambdaExpr with | t when t = typeof<unit> -> None | t -> Some (toType t)
        let returnType = toType lambdaExpr.Type
        let rt_string = Print.type' returnType
        let rt = DefinedType $"@location(0) {rt_string}"
        // todo @location(0)
        Function (v.Name, args, [ "fragment" ], Some rt, addReturn abs) :: translateModule following
    | Patterns.Let (v, Patterns.Call (None, method, [ Patterns.Lambda (lambdaVar, lambdaExpr) ]), following)
        when method.Name = "VertexShader" ->
        let args, abs = function' lambdaVar lambdaExpr
        let rt = match exprType lambdaExpr with | t when t = typeof<unit> -> None | t -> Some (toType t)
        // todo @location(0)
        Function (v.Name, args, [ "vertex" ], rt, addReturn abs) :: translateModule following
    | _ -> []
and toType (t: System.Type) =
    match t with
    | t when t = typeof<int> -> WgslType.Int
    | t when t = typeof<float32> -> WgslType.Float
    | t when t = typeof<vec3<float32>> -> WgslType.DefinedType "vec3f"
    | t when t = typeof<vec4<float32>> -> WgslType.DefinedType "vec4f"
    | _ -> WgslType.DefinedType (t.FullName.Replace(".", "_"))
and addReturn (statements: WgslStatement list) =
    let result =
        match List.last statements with
        | ExprStatement wgslExpr -> Some (ReturnExpr wgslExpr)
        | WhileLoop(condition, body) -> None
        | VarDeclaration(name, value) -> None
        | LetDeclaration(name, value) -> None
        | IfThenElse(cond, whenTrue, whenFalse) ->
            Some <| IfThenElse (cond, addReturn whenTrue, addReturn whenFalse)
        | Assign(var, value) -> None
        | ReturnExpr expr -> None
    match result with
    | None -> statements
    | Some s -> List.take (statements.Length - 1) statements @ [ s ]
// let (|App|_|) value = Some 1234
// type Foo() =
//     static member (|App|_|) value = Some
// let f = Foo()
// match f with
// | f.App value -> ()
// | _ -> ()
// let frag' = createFragment' { new IsFragment<_> with member this.Visit fn = fn.Invoke 1234 }
// let fn (f: 'a -> 'b) =
    // f.Invoke(null)
// (+).GetType()
// let inline fn'<'t, 'a, 'b when 't : (member Invoke : 'a -> 'b)> (fn: 't) a =
let inline fn' (fn: FSharpFunc<_,_>) a =
    fn a
    // (^t : (member Invoke : 'a -> 'b) (fn, a))
    // 't.Invoke(fn, null)
    // t.Invoke(null)
// let a = (+) in let b = a 1 2 in a.GetType().GetMethods() |> Array.map _.Name
// let asdf = fn' (+) 1
let _ = fn' (fun _ -> 1234) 0
type Screen = {
    gridSize: int
    posX: float32
    posY: float32
    width: float32
    height: float32
}
let FragmentShader fn = fn
let Location n fn = fn
let rec frag = createFragment' shader'
and shader' (screen: Screen, circles: float32[]) = <@
    // {|
        // fragment = fun (output: VertexOutput) ->
    // let sqDistanceSphere (sphere: vec3f) (point: vec3f) : float32 =
    //     let x = sphere.x - point.x
    //     let y = sphere.y - point.y
    //     let z = sphere.z - point.z
    //     (x * x) + (y * y) + (z * z)
        
    let distanceSphere (sphere: vec3f) (radius: float32) (point: vec3f) : float32 =
        length(sphere - point) - radius
        
    let fragment = FragmentShader begin fun (output: output) ->
        let pixelsPerMeter = 500.0f
        let f = 2.0f
        let r = 1.0f
        let x = output.position.x * (screen.width / pixelsPerMeter)
        let y = output.position.y * (screen.height / pixelsPerMeter)
        let sphere = vec3(x, y, 20.0f)
        let cameraOrigin = vec3(0f, 0f, -2f)
        let mutable point = vec3(x, y, f) + cameraOrigin
        let direction = point - cameraOrigin
        let mutable distance = distanceSphere sphere r point
        let mutable iteration = 0
        while iteration < 1000 && distance > 0.001f && distance < 1000f do
            distance <- distanceSphere sphere r point
            point <- point + (direction * distance) 
            iteration <- iteration + 1
        if distance <= 0.001f then
            vec4(1f, 1f, 1f, 0f)
        else
            vec4(0f)
    end
        // vec4(float32 distance)
        // mat2x2(0f, 0f, 0f, 0f) * 5f
    Fragment fragment
    // |}
@>

// type Result = Hit of x: int * y: int * z: int | None
// <@
// match Hit (1, 2, 3) with
// | Hit (x, y, z) -> x + y + z
// | None -> if 1 = 1 then 0 else 2
// @>

let compiledWgsl = translateModule (shader' ({ gridSize = 0; posX = 0f; posY = 0f; width = 0f; height = 0f }, [||]))
        
let output = Print.shader compiledWgsl
output