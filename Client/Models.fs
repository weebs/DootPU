module Dootverse.Models

open Browser.Types

type float3f = { X: float; Y: float; Z: float }
type float2f = { X: float; Y: float }

type Asset =
    Image of ImageData
    
    
type GameWorldState = { playerPosition: float2f; playerRotation: float; entities: (float2f * Asset)[] }
    
type Game = { Assets: Map<string, Asset> }
