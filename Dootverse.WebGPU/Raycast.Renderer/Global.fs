namespace global


type Ptr =
    static member inline arrayPtr(values: 't[]) =
        use ptr = fixed values
        ptr

    static member inline ptr<'t when 't: unmanaged>(value: 't) =
        use ptr = fixed [| value |]
        ptr
[<AutoOpen>]
module AutoImport =
    let arrayPtr = Ptr.arrayPtr
    let ptr = Ptr.ptr
