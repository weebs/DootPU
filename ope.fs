module ope
module thread =
    let start (callback: unit -> unit) =
        let t = System.Threading.Thread(System.Threading.ThreadStart(callback))
        t.Start()
        t
    let start_ callback = start callback |> ignore
module io =
    let readText (path: string) =
        try System.IO.File.ReadAllText path |> Ok
        with error -> Error error
type EnvVar(_name, ?_value) =
    let mutable value = _value |> Option.defaultValue (System.Environment.GetEnvironmentVariable(_name))
    do if _value.IsSome then System.Environment.SetEnvironmentVariable(_name, _value.Value)
    member this.Set(_value) = value <- _value
    member this.Value = value
    member this.Name = _name
type Env() =
    member _.Item 
        with get (key: string) = System.Environment.GetEnvironmentVariable key
        and set (key: string) (value: string) = System.Environment.SetEnvironmentVariable(key, value)
    member _.CreateVar(name: string, ?value: string) = 
        if value.IsSome then EnvVar(name, value.Value) else EnvVar(name)

let env = Env()

// module ope

// module thread =
//     let start (callback: unit -> unit) =
//         let t = System.Threading.Thread(System.Threading.ThreadStart(callback))
//         t.Start()
//         t
// module io =
//     let readText (path: string) =
//         try System.IO.File.ReadAllText path |> Ok
//         with error -> Error error
// type env =
//     static member 
