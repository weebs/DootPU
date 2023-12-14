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
// todo rename to Signaling
module Signaling =
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
/// Events broadcasted by the server    
type GameEvent =
    | EntityDestroyed of int
    | EnemyDamaged of int
    | PlayerJoined of int * string
    | PlayerDisconnected of int
type [<RequireQualifiedAccess>] ClientMessage =
    | Update of PlayerState
    | DestroyedEntity of id: int
    | ShotEntity of id: int
type EnemyType =
    | Zombie of Zombie
and Zombie = { health: float; state: ZombieState }
and ZombieState =
    | Idle
type EntityType =
    | Tree
    | Enemy of sprite: string * ``type``: EnemyType
    | Player of name: string
type GameEntity = {
    position: float3
    data: EntityType
    sprite: string option
}
type Scene = {
    Walls: Map<int * int, byte * byte * byte>
    GameObjects: Map<int, GameEntity>
}

type [<RequireQualifiedAccess>] ServerMessage =
    | UpdatePlayer of int * PlayerState
    // | PlayerDisconnected of Guid
    | WorldState of Scene
    | EntityRemoved of int
    | EntityMoved of int * float3
    | GameEvent of GameEvent
    
    
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
            GameObjects = Map.ofArray [|
                for i in 1..2000 do
                    i, { data = Tree; sprite = Some tree; position = { x = random() * area - (area / 2.0); y = 0.5; z = random() * area - (area / 2.0); } }
                    i, { data = Tree; sprite = Some tree; position = { x = random() * area - (area / 2.0); y = 0.5; z = random() * area - (area / 2.0); } }
                for i in 2001..2201 do
                    i, { data = Enemy (zombie, Zombie { health = 100.0; state = ZombieState.Idle }); sprite = Some zombie; position = { x = random() * area - (area / 2.0); y = 0.5; z = random() * area - (area / 2.0); } }
            |]
            Walls = mapData |> Map.map (fun _ value -> 0uy, 0uy, 0uy)
        }
