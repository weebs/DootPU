open System.Reflection
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.DerivedPatterns

[<ReflectedDefinition>] 
type Foo =
    static member x = 5
    static member y () = 6
    static member z a = a
    static member unwrap<'t> (value: 't) = <@ Unchecked.defaultof<'t> @>
    
type CallbackFunction =
    abstract member Foo : string -> float32

let foo_members = 
    typeof<Foo>.GetMethods()
    |> Array.choose (function
        | MethodWithReflectedDefinition _ as m ->
            match Expr.TryGetReflectedDefinition(m :> MethodBase) with
            | Some defn ->
                Some (m.Name, defn)
            | None -> None
        | _ -> None)
    |> Map.ofArray
    
let unwrap = foo_members["unwrap"]

let var =
    match unwrap with
    | Patterns.Lambda (var, e) -> var
    
let t = var.Type.IsGenericParameter