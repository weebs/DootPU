module Dootverse.Models

open System.Collections.Generic
open Browser.Types
open System
open Doot.Maths.Voxel.Traversal

type console() =
    static member debug = false
    static member log (msg, [<ParamArray>] args) =
        if console.debug then
            Fable.Core.JS.console.log (msg, args)

type float3f =
    { X: float; Y: float; Z: float }
    with
    member this.Dot v =
        (this.X * v.X) + (this.Y + v.Y) + (this.Z * v.Z)
type float2f =
    { X: float; Y: float }
    member this.Vector2 = Vector2(this.X, this.Y)
    static member (+) (a: float2f, b: Vector2) =
        { X = a.X + b.X; Y = a.Y + b.Y }

type Asset =
    Image of ImageData
type [<Struct>] AssetId = AssetId of string    
type GameWorldState = {
    playerPosition: float2f; playerRotation: float; entities: (float2f * AssetId)[]
    Walls: Dictionary<(int * int), (byte * byte * byte)>
}
    
type Scene = {
    // todo: We are using AssetId => Asset so that the scene can watch & reload changes to assets
    Assets: Map<AssetId, Asset>
    World: GameWorldState
}

type AssetDatabase() =
    let assets = Dictionary<AssetId, Asset>()
    member this.RegisterAsset = assets.Add
    member this.ReadAsset id = if assets.ContainsKey id then Some assets[id] else None
    member this.All = Map.ofList [ for kv in assets do yield (kv.Key, kv.Value) ]
let Assets = AssetDatabase()