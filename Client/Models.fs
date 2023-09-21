module Dootverse.Models

open Browser.Types

type float3f =
    { X: float; Y: float; Z: float }
    with
    member this.Dot v =
        (this.X * v.X) + (this.Y + v.Y) + (this.Z * v.Z)
type float2f = { X: float; Y: float }

type Asset =
    Image of ImageData
    
    
type GameWorldState = { playerPosition: float2f; playerRotation: float; entities: (float2f * Asset)[] }
    
type Game = { Assets: Map<string, Asset> }
