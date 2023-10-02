module Dootverse.Network
open System

type float3 = { x: float; y: float; z: float }

type PlayerState =
    {
        Position: float3
        Rotation: float
    }
    
type ClientMessage =
    | Update of PlayerState
    
type Scene = {
    Walls: Map<int * int, byte * byte * byte>
    Entities: Map<int, float3>
}

type ServerMessage =
    | UpdatePlayer of Guid * PlayerState
    | WorldState of Scene
    
