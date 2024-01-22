module Dootverse.Godot3.FSharp.Models.Network

open System.Collections.Generic
open Godot
open Thoth.Json.Net
// type Encode<'t> =
//     abstract member Encode: string
//     abstract member Decode: string -> 't
//
type [<Struct>] float3 = { x: float32; y: float32; z: float32 } 
type [<Struct>] ClientMsg =
    | Position of float3
    | DamagedEnemy of id: int
    with
    member this.Serialize() =
        Encode.Auto.toString this
    static member Deserialize data = Decode.Auto.unsafeFromString<ClientMsg> data
//     interface Encode<ClientMsg> with
//         member this.Encode = failwith "todo"
//         member this.Decode(var0) =
//             failwith "todo"
type ServerMsg =
    unit
    
type Server() as this =
    inherit Node()
    
    let mutable server = new WebSocketServer()
    let players = Dictionary()
    let playerObjs = Dictionary<int, CSGBox>()
    override this._Ready () =
        server.Connect("client_connected", this, "ClientConnected") |> ignore
        server.Connect("data_received", this, "DataReceived") |> ignore
        server.Listen 8008 |> ignore
    member this.DataReceived id =
        let packet = server.GetPeer(id).GetPacket()
        let data = packet.GetStringFromUTF8()
        let msg = ClientMsg.Deserialize data
        match msg with
        | Position float3 ->
            players.[id] <- float3
            playerObjs.[id].GlobalTranslation <- Vector3(float3.x, float3.y, float3.z)
            GD.Print float3
        | DamagedEnemy id -> GD.Print id
        GD.Print "received data:"
        GD.Print msg
    override this._PhysicsProcess delta =
        server.Poll()
    member this.ClientConnected (id: int, proto: string) =
        let cube = new CSGBox()
        this.AddChild(cube)
        playerObjs.[id] <- cube
        players.[id] <- { x = 0.0f; y = 0.0f; z = 0.0f }