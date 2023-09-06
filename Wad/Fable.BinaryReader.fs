module Fable.BinaryReader

open Fable.Core
open Fable.Core.JS
// let [<Emit("new Blob($0)")>] createBlob data : Browser.Types.Blob = jsNative
//
// type MemoryStream(bytes: byte[]) =
//     let blob = createBlob bytes
//     let buffer = blob.arrayBuffer ()
//     member this.Blob = blob
//     member this.Buffer = buffer
// type BinaryReader(buffer: ArrayBuffer) as this =
//     let mutable streamPosition = 0
//     // let blob = stream.Blob
//     let view = DataView.Create buffer
//     
//     // let advanceStream<'t> () =
//     //     streamPosition <- streamPosition + sizeof<'t>
//     member this.ReadChars(length: int) : char[] =
//         let chars = blob.slice(streamPosition, streamPosition + length)
//         streamPosition <- streamPosition + length
//         // chars.
//     member this.ReadInt32() : int32 =
//         streamPosition <- streamPosition + sizeof<int32>
//         0