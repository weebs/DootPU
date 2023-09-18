module Dootverse.Models

open Browser.Types

type float2f = { X: float; Y: float }
type GameWorldState = { playerPosition: float2f; playerRotation: float }
type Asset =
    Image of ImageData
type Game = { Assets: Map<string, Asset> }
