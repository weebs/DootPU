module Doot.Maths.Voxel.Traversal

open System
open System.Numerics
open Fable.Core


// module System =
    // module Numerics =
    
type Vector2(x: float32, y: float32) =
    member this.X = x
    member this.Y = y
    member this.Length () = MathF.Sqrt(x ** 2f + y ** 2f)
    static member (/) (a: Vector2, b) = Vector2(a.X / b, a.Y / b)
    static member (*) (a: Vector2, b: float32) =
        // let a = a / a.Length()
        Vector2(a.X * b, a.Y * b)
    override this.ToString() = $"({x}, {y})"
// #if FABLE_COMPILER
// open System.Numerics
// #endif

let findVoxelsAlongRay (u: Vector2) (v: Vector2) =
    JS.console.log $"findVoxelsAlongRay {u} {v}"
    let distance value slope =
        if MathF.Ceiling value = value then float32 (MathF.Sign(slope))
        elif slope > 0f then MathF.Ceiling(value) - value
        else MathF.Floor(value) - value
    let mutable x = u.X
    let mutable y = u.Y
    let mutable count = 0
    let stepX = if v.X >= 0f then 1 else -1
    let stepY = if v.Y >= 0f then 1 else -1
    let mutable voxelX =
        // if MathF.Round u.X = u.X && MathF.Round v.X = v.X && v.X < 0f then
        if MathF.Round u.X = u.X && v.X < 0f then
            int (MathF.Floor x) - 1
        else
            int (MathF.Floor x)
    let mutable voxelY =
        // if MathF.Round u.Y = u.Y && MathF.Round v.Y = v.Y && v.Y < 0f then
        if MathF.Round u.Y = u.Y && v.Y < 0f then
            int (MathF.Floor y) - 1
        else
            int (MathF.Floor y)
    seq {
        // todo: yield starting voxel
        // yield (x, y)
        yield (voxelX, voxelY)
        while count < 100000 do
            count <- count + 1
            JS.console.log "=========================="
            JS.console.log $"n = {count}, ({x}, {y})"
            let dx = distance x v.X
            let dy = distance y v.Y
            let dxy = (v.Y / v.X) * dx
            let dyx = (v.X / v.Y) * dy
            let tx = Vector2(dx, dxy)
            let ty = Vector2(dyx, dy)
            JS.console.log $"dx = {dx}; dxy = {dxy}; tx = {tx};"
            JS.console.log $"dy = {dy}; dyx = {dyx}; ty = {ty}"
            // let tDifference = tx.Length() - ty.Length()
            let tDifference = ((tx.X * tx.X) + (tx.Y * tx.Y)) - ((ty.X * ty.X) + (ty.Y * ty.Y))
            // todo: use tDifference
            let t =
                // todo: this can be solved with (tx.X ** 2) + (tx.Y ** 2) < (ty.X ** 2) + (ty.Y ** 2)
                if tDifference < 0f then
                    voxelX <- voxelX + stepX
                    tx
                // todo: Cases where tx and ty are roughly equal (a corner is hit)
                elif tDifference < 0.00000001f then
                    voxelX <- voxelX + stepX
                    voxelY <- voxelY + stepY
                    ty
                else
                    voxelY <- voxelY + stepY
                    ty
            x <- x + t.X
            y <- y + t.Y
            // todo: Can we remove the rounding behavior?
            // if MathF.Abs(MathF.Round(x) - x) < 0.0000001f then
                // x <- MathF.Round x
            // if MathF.Abs(MathF.Round(y) - y) < 0.0000001f then
                // y <- MathF.Round y
            JS.console.log $"selecting {t}"
            JS.console.log $"x: {x - t.X} => {x}"
            JS.console.log $"y: {y - t.Y} => {y}"
            JS.console.log "=========================="
            // todo: yield next voxel (based on slope)
            yield (voxelX, voxelY)
            // yield (x, y)
    }

/// Credit: http://www.cse.yorku.ca/~amana/research/grid.pdf
let initialize (voxelWidth: int) (u: Vector2) (v: Vector2) =
    let vNormalized = v // / v.Length()
    
    let stepX = if vNormalized.X >= 0f then 1 else -1
    let stepY = if vNormalized.Y >= 0f then 1 else -1
    // todo: negative values in v
    let tx =
        if u.X <> MathF.Floor(u.X) then
            MathF.Ceiling(u.X + float32 stepX) - u.X
        else
            MathF.Ceiling(u.X + float32 stepX) - u.X
    // let tx = MathF.Ceiling(u.X) - u.X
    // let tx = MathF.Ceiling(u.X + float32 stepX) - u.X
    // todo: t = (u.X + stepX) / u.X
    let tMaxX = MathF.Sqrt((tx ** 2f) + (((vNormalized.Y / vNormalized.X) * tx) ** 2f))
    // let ty = MathF.Ceiling(u.Y) - u.Y
    // let ty = MathF.Floor(u.Y + float32 stepY) - u.Y
    let ty =
        if u.Y <> MathF.Floor(u.Y) then
            // Works on positive slope from non-integer point
            // MathF.Floor(u.Y + float32 stepY) - u.Y
            // Works on negative slope from non-integer point
            MathF.Ceiling(u.Y + float32 stepY) - u.Y
        else
            MathF.Ceiling(u.Y) - u.Y
    // let ty = MathF.Ceiling(u.Y) - u.Y
    let tMaxY = MathF.Sqrt((ty ** 2f) + (((vNormalized.X / vNormalized.Y) * ty) ** 2f))
    
    // let tMaxX = float32 <| (int (MathF.Floor(u.X)) + stepX) * voxelWidth
    // let tMaxY = float32 <| (int (MathF.Floor(u.Y)) + stepY) * voxelWidth
    
    let Frac f1 = f1 - MathF.Floor f1
        // if f1 > 0f then
        //     f1 - MathF.Floor(f1)
        // else
        //     f1 - MathF.Floor(f1)
    let voxelWidth = float32 voxelWidth
    {|
        stepX = stepX
        stepY = stepY
        // todo: negative values in v
        deltaX = voxelWidth / vNormalized.X * float32 stepX
        deltaY = voxelWidth / vNormalized.Y * float32 stepY
        tMaxX = tMaxX
        tMaxY = tMaxY
        // todo: Stackoverflow
        // https://stackoverflow.com/questions/12367071/how-do-i-initialize-the-t-variables-in-a-fast-voxel-traversal-algorithm-for-ray
        // deltaX = v.X - u.X
        // deltaY = v.Y - u.Y
        // tMaxX = (v.X - u.X) * (1f - Frac(u.X / 1f))
        // tMaxY = (v.Y - u.Y) * (1f - Frac(u.Y / 1f))
        
        // tMaxX = (1f - (u.X - (MathF.Floor u.X))) / v.X
        // tMaxY = (1f - (u.Y - (MathF.Floor u.Y))) / v.Y
        // tMaxY = tMaxY
    
        
    |}


let traverseRay voxelWidth (origin: Vector2) (dir: Vector2) =
    let info = initialize voxelWidth origin dir
    let mutable tMaxX = info.tMaxX
    let mutable tMaxY = info.tMaxY
    JS.console.log ("Voxels colliding with ray ", origin, " to ", dir)
    JS.console.log ("tMaxX = ", tMaxX)
    JS.console.log ("tMaxY = ", tMaxY)
    JS.console.log ("deltaX = ", info.deltaX)
    JS.console.log ("deltaY = ", info.deltaY)
    let mutable x = int (MathF.Floor origin.X)
    let mutable y = int (MathF.Floor origin.Y)
    let yOffset = if info.stepY < 0 then -1 else 0
    let xOffset = if info.stepX < 0 then -1 else 0
    let mutable count = 0
    seq {
        JS.console.log (x + xOffset, y + yOffset)
        yield (x + xOffset, y + yOffset)
        while count < 10000 do
            count <- count + 1
            // if dir.X < 0f && dir.Y < 0f then
            //     if tMaxY + info.deltaY > tMaxX + info.deltaX then
            //     // if tMaxX < tMaxY then
            //         JS.console.log $"    tMaxY <- {tMaxY} + {info.deltaY}"
            //         tMaxY <- tMaxY + info.deltaY
            //         y <- y + info.stepY
            //     else
            //         JS.console.log $"    tMaxX <- {tMaxX} + {info.deltaX}"
            //         tMaxX <- tMaxX + info.deltaX
            //         x <- x + info.stepX
            // if dir.X < dir.Y then
            //     if tMaxY + info.deltaY < tMaxX + info.deltaX then
            //     // if tMaxX < tMaxY then
            //         JS.console.log $"    tMaxY <- {tMaxY} + {info.deltaY}"
            //         tMaxY <- tMaxY + info.deltaY
            //         y <- y + info.stepY
            //     else
            //         JS.console.log $"    tMaxX <- {tMaxX} + {info.deltaX}"
            //         tMaxX <- tMaxX + info.deltaX
            //         x <- x + info.stepX
            // else
            JS.console.log $"{tMaxX + info.deltaX} < {tMaxY + info.deltaY}"
            // if tMaxX + info.deltaX < tMaxY + info.deltaY then
            // JS.console.log $"{tMaxX} < {tMaxY}"
            if tMaxX < tMaxY then
                JS.console.log $"    tMaxX <- {tMaxX} + {info.deltaX}"
                tMaxX <- tMaxX + info.deltaX
                x <- x + info.stepX
            else
                JS.console.log $"    tMaxY <- {tMaxY} + {info.deltaY}"
                tMaxY <- tMaxY + info.deltaY
                y <- y + info.stepY
            JS.console.log (x + xOffset, y + yOffset)
            yield (x + xOffset, y + yOffset)
    }
    // seq {
    //     yield (x, y)
    //     let mutable x = x
    //     let mutable y = y
    //     let mutable dx = x
    //     let mutable dy = y
    // }
