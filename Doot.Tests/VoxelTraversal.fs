module Doot.Tests.VoxelTraversal
open System
open System.Numerics
open NUnit.Framework
open Doot.Maths.Voxel.Traversal

[<Test>]
let ``find points along ray in voxel grid`` () =
    let start = Vector2(0f, 0f)
    let dir = Vector2(4f, 1f)
    let info = initialize start dir
    let mutable tMaxX = info.tMaxX
    let mutable tMaxY = info.tMaxY
    let mutable x = MathF.Floor start.X
    let mutable y = MathF.Floor start.Y
    printfn $"({x}, {y})"
    for i in 1..20 do
        if tMaxX < tMaxY then
            tMaxX <- tMaxX + info.deltaX
            x <- x + info.stepX
        else
            tMaxY <- tMaxY + info.deltaY
            y <- y + info.stepY
        
        printfn $"({x}, {y})"
[<Test>]
let ``infinite seq works`` () =
    let start = Vector2(0f, 0f)
    let dir = Vector2(1f, 1f)
    let elements = traverseRay start dir
    for (x, y) in elements |> Seq.take 101 do
        printfn $"({x}, {y})"