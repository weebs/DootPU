module Dootverse.Network
open System
open Fable.Core

type WebRtcRequest =
    {
        Offer: string
        Candidates: (string * string option)[]
    }

type WebRtcResponse =
    {
        Answer: string
        Candidates: (string * string option)[]
    }
    
type float3 =
    { x: float; y: float; z: float }
    member inline this.Tuple = (this.x, this.y, this.z)
    static member (/) (a: float3, b: float) =
        { x = a.x / b; y = a.y / b; z = a.z / b }

type PlayerState =
    {
        Position: float3
        Rotation: float
    }
    
type ClientMessage =
    | Update of PlayerState
    | DestroyedEntity of id: int
    
type Scene = {
    Walls: Map<int * int, byte * byte * byte>
    Entities: Map<int, string * float3>
}

type ServerMessage =
    | UpdatePlayer of Guid * PlayerState
    | PlayerDisconnected of Guid
    | WorldState of Scene
    | EntityRemoved of int
    
module LobbyConnection =
    type [<Struct; Erase>] LobbyId = LobbyId of System.Guid
    type ClientMessage =
        | Connect of Guid * WebRtcRequest
        | ConnectionResponse of Guid * WebRtcResponse
        | HostLobby of name: string
        | RefreshLobbies
    type ServerMessage =
        | ConnectionResponse of WebRtcResponse
        | ConnectionRequest of Guid * WebRtcRequest
        | Lobbies of {| id: Guid; name: string; playerCount: int |}[]
    
module Scene =
    let mapSize = 10
    let nextInt n =
        #if FABLE_COMPILER
        Fable.Core.JS.Math.random() * float n |> int
        #else
        Random.Shared.NextDouble() * float n |> int
        #endif
    let random () =
        #if FABLE_COMPILER
        Fable.Core.JS.Math.random()
        #else
        Random.Shared.NextDouble()
        #endif
        
    let createScene () =
        let mapData = Map.ofArray [|
            let mapSize = mapSize * 10
            for i in -mapSize..mapSize do
                (i, mapSize), "pink"
                (i, -mapSize), "pink"
                (-mapSize , i), "pink"
                (mapSize, i), "pink"
            for _ in 1..800 do
                let x = nextInt (mapSize * 2) - mapSize
                let y = nextInt (mapSize * 2) - mapSize
                (x, y), "green"
            for _ in 1..800 do
                let x = nextInt (mapSize * 2) - mapSize
                let y = nextInt (mapSize * 2) - mapSize
                (x, y), "blue"
            for _ in 1..800 do
                let x = nextInt (mapSize * 2) - mapSize
                let y = nextInt (mapSize * 2) - mapSize
                (x, y), "orange"
        |]
        // let level =
        //     let d = Dictionary()
        //     mapData |> Map.iter (fun key value -> d[key] <- toRgb value)
        //     d
        let tree = "textures/rs/yewtree.png"
        let zombie = "textures/rs/zombie_standing.png"
        let area = float mapSize * 10.0 * 2.0
        {
            Entities = Map.ofArray [|
                for i in 1..2000 do
                    i, (tree, { x = random() * area - (area / 2.0); y = 0.5; z = random() * area - (area / 2.0); })
                    i, (tree, { x = random() * area - (area / 2.0); y = 0.5; z = random() * area - (area / 2.0); })
                for i in 2001..2201 do
                    i, (zombie, { x = random() * area - (area / 2.0); y = 0.5; z = random() * area - (area / 2.0); })
            |]
            Walls = mapData |> Map.map (fun _ value -> 0uy, 0uy, 0uy)
        }
