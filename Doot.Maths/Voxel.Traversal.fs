module Doot.Maths.Voxel.Traversal

open System
open System.Numerics
open Fable.Core


// module System =
    // module Numerics =
    
// type Vector2(x: float, y: float) =
type Vector2(x: float, y: float) =
    member this.X = x
    member this.Y = y
    member this.Length () = Math.Sqrt(x ** 2. + y ** 2.)
    static member (/) (a: Vector2, b) = Vector2(a.X / b, a.Y / b)
    static member (-) (a: Vector2, b: Vector2) =
        Vector2(a.X - b.X, a.Y - b.Y)
    static member (+) (a: Vector2, b: Vector2) =
        Vector2(a.X + b.X, a.Y + b.Y)
    static member (*) (a: Vector2, b: float) =
        // let a = a / a.Length()
        Vector2(a.X * b, a.Y * b)
    member this.Normalized =
        let length = this.Length()
        if length = 0. then
            this
        else
            Vector2(x / length, y / length)
    override this.ToString() = $"({x}, {y})"
// #if FABLE_COMPILER
// open System.Numerics
// #endif

let findIntersection (u: Vector2) (v: Vector2) checkVoxel =
    let distance (value: float) (slope: float) =
        if Math.Ceiling value = value then float (Math.Sign(slope))
        elif slope > 0. then Math.Ceiling(value) - value
        else Math.Floor(value) - value
    let mutable x = u.X
    let mutable y = u.Y
    let mutable count = 0
    let stepX = if v.X >= 0. then 1 else -1
    let stepY = if v.Y >= 0. then 1 else -1
    let mutable voxelX =
        // if JS.Math.round u.X = u.X && JS.Math.round v.X = v.X && v.X < 0. then
        if JS.Math.round u.X = u.X && v.X < 0. then
            int (Math.Floor x) - 1
        else
            int (Math.Floor x)
    let mutable voxelY =
        // if JS.Math.round u.Y = u.Y && JS.Math.round v.Y = v.Y && v.Y < 0. then
        if JS.Math.round u.Y = u.Y && v.Y < 0. then
            int (Math.Floor y) - 1
        else
            int (Math.Floor y)
    let mutable voxelFound = None
    while count < 100000 && voxelFound = None do
        count <- count + 1
        let dx = distance x v.X
        let dy = distance y v.Y
        let dxy = (v.Y / v.X) * dx
        let dyx = (v.X / v.Y) * dy
        let tx = Vector2(dx, dxy)
        let ty = Vector2(dyx, dy)
        // let tDifference = tx.Length() - ty.Length()
        // todo: we can calculate t based on the distance multiplied by the ratio of
        // todo that side to the length of the vector
        let tDifference = ((tx.X * tx.X) + (tx.Y * tx.Y)) - ((ty.X * ty.X) + (ty.Y * ty.Y))
        let t =
            // todo: Cases where tx and ty are roughly equal (a corner is hit)
            if Math.Abs(tDifference) < 0.00000001 then
                voxelX <- voxelX + stepX
                voxelY <- voxelY + stepY
                ty
            elif tDifference < 0. then
                voxelX <- voxelX + stepX
                tx
            else
                voxelY <- voxelY + stepY
                ty
        x <- x + t.X
        y <- y + t.Y
        // todo: Can we remove the rounding behavior?
        // if Math.Abs(JS.Math.round(x) - x) < 0.0000001f then
            // x <- JS.Math.round x
        // if Math.Abs(JS.Math.round(y) - y) < 0.0000001f then
            // y <- JS.Math.round y
        if checkVoxel (voxelX, voxelY) then    
            voxelFound <- Some ((voxelX, voxelY), (x, y))
    voxelFound
let findVoxelsAlongRay (u: Vector2) (v: Vector2) =
    let debug = false
    if debug then
        JS.console.log $"findVoxelsAlongRay {u} {v}"
    let distance (value: float) (slope: float) =
        if Math.Ceiling value = value then float (Math.Sign(slope))
        elif slope > 0. then Math.Ceiling(value) - value
        else Math.Floor(value) - value
    let mutable x = u.X
    let mutable y = u.Y
    let mutable count = 0
    let stepX = if v.X >= 0. then 1 else -1
    let stepY = if v.Y >= 0. then 1 else -1
    let mutable voxelX =
        // if JS.Math.round u.X = u.X && JS.Math.round v.X = v.X && v.X < 0. then
        if JS.Math.round u.X = u.X && v.X < 0. then
            int (Math.Floor x) - 1
        else
            int (Math.Floor x)
    let mutable voxelY =
        // if JS.Math.round u.Y = u.Y && JS.Math.round v.Y = v.Y && v.Y < 0. then
        if JS.Math.round u.Y = u.Y && v.Y < 0. then
            int (Math.Floor y) - 1
        else
            int (Math.Floor y)
    seq {
        // todo: yield starting voxel
        // yield (x, y)
        yield ((voxelX, voxelY), (x, y))
        while count < 100000 do
            count <- count + 1
            if debug then
                JS.console.log "=========================="
                JS.console.log $"n = {count}, ({x}, {y}) (last step = ({voxelX}, {voxelY})"
            let dx = distance x v.X
            let dy = distance y v.Y
            let dxy = (v.Y / v.X) * dx
            let dyx = (v.X / v.Y) * dy
            let tx = Vector2(dx, dxy)
            let ty = Vector2(dyx, dy)
            if debug then
                JS.console.log $"dx = {dx}; dxy = {dxy}; tx = {tx};"
                JS.console.log $"dy = {dy}; dyx = {dyx}; ty = {ty}"
            // let tDifference = tx.Length() - ty.Length()
            // todo: we can calculate t based on the distance multiplied by the ratio of
            // todo that side to the length of the vector
            let tDifference = ((tx.X * tx.X) + (tx.Y * tx.Y)) - ((ty.X * ty.X) + (ty.Y * ty.Y))
            // todo: use tDifference
            let t =
                // todo: this can be solved with (tx.X ** 2) + (tx.Y ** 2) < (ty.X ** 2) + (ty.Y ** 2)
                if Math.Abs(tDifference) < 0.00000001 then
                    voxelX <- voxelX + stepX
                    voxelY <- voxelY + stepY
                    ty
                elif tDifference < 0. then
                    if debug then
                        JS.console.log ("=========== diff = ", tDifference)
                    voxelX <- voxelX + stepX
                    tx
                // todo: Cases where tx and ty are roughly equal (a corner is hit)
                else
                    voxelY <- voxelY + stepY
                    ty
            // if voxelX = 4 && voxelY = 4 then
            //     JS.debugger ()
                // JS.console.log "yo"
            x <- x + t.X
            y <- y + t.Y
            // todo: Can we remove the rounding behavior?
            // if Math.Abs(JS.Math.round(x) - x) < 0.0000001f then
                // x <- JS.Math.round x
            // if Math.Abs(JS.Math.round(y) - y) < 0.0000001f then
                // y <- JS.Math.round y
            if debug then
                JS.console.log $"selecting {t}"
                JS.console.log $"x: {x - t.X} => {x}"
                JS.console.log $"y: {y - t.Y} => {y}"
                JS.console.log "=========================="
            // todo: yield next voxel (based on slope)
            yield ((voxelX, voxelY), (x, y))
            // yield (x, y)
    }

/// Credit: http://www.cse.yorku.ca/~amana/research/grid.pdf
let initialize (voxelWidth: int) (u: Vector2) (v: Vector2) =
    let vNormalized = v // / v.Length()
    
    let stepX = if vNormalized.X >= 0. then 1 else -1
    let stepY = if vNormalized.Y >= 0. then 1 else -1
    // todo: negative values in v
    let tx =
        if u.X <> Math.Floor(u.X) then
            Math.Ceiling(u.X + float stepX) - u.X
        else
            Math.Ceiling(u.X + float stepX) - u.X
    // let tx = Math.Ceiling(u.X) - u.X
    // let tx = Math.Ceiling(u.X + float stepX) - u.X
    // todo: t = (u.X + stepX) / u.X
    let tMaxX = Math.Sqrt((tx ** 2.) + (((vNormalized.Y / vNormalized.X) * tx) ** 2.))
    // let ty = Math.Ceiling(u.Y) - u.Y
    // let ty = Math.Floor(u.Y + float stepY) - u.Y
    let ty =
        if u.Y <> Math.Floor(u.Y) then
            // Works on positive slope from non-integer point
            // Math.Floor(u.Y + float stepY) - u.Y
            // Works on negative slope from non-integer point
            Math.Ceiling(u.Y + float stepY) - u.Y
        else
            Math.Ceiling(u.Y) - u.Y
    // let ty = Math.Ceiling(u.Y) - u.Y
    let tMaxY = Math.Sqrt((ty ** 2.) + (((vNormalized.X / vNormalized.Y) * ty) ** 2.))
    
    // let tMaxX = float <| (int (Math.Floor(u.X)) + stepX) * voxelWidth
    // let tMaxY = float <| (int (Math.Floor(u.Y)) + stepY) * voxelWidth
    
    let Frac (f1: float) = f1 - Math.Floor f1
        // if f1 > 0. then
        //     f1 - Math.Floor(f1)
        // else
        //     f1 - Math.Floor(f1)
    let voxelWidth = float voxelWidth
    {|
        stepX = stepX
        stepY = stepY
        // todo: negative values in v
        deltaX = voxelWidth / vNormalized.X * float stepX
        deltaY = voxelWidth / vNormalized.Y * float stepY
        tMaxX = tMaxX
        tMaxY = tMaxY
        // todo: Stackoverflow
        // https://stackoverflow.com/questions/12367071/how-do-i-initialize-the-t-variables-in-a-fast-voxel-traversal-algorithm-for-ray
        // deltaX = v.X - u.X
        // deltaY = v.Y - u.Y
        // tMaxX = (v.X - u.X) * (1f - Frac(u.X / 1f))
        // tMaxY = (v.Y - u.Y) * (1f - Frac(u.Y / 1f))
        
        // tMaxX = (1f - (u.X - (Math.Floor u.X))) / v.X
        // tMaxY = (1f - (u.Y - (Math.Floor u.Y))) / v.Y
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
    let mutable x = int (Math.Floor origin.X)
    let mutable y = int (Math.Floor origin.Y)
    let yOffset = if info.stepY < 0 then -1 else 0
    let xOffset = if info.stepX < 0 then -1 else 0
    let mutable count = 0
    seq {
        JS.console.log (x + xOffset, y + yOffset)
        yield (x + xOffset, y + yOffset)
        while count < 10000 do
            count <- count + 1
            // if dir.X < 0. && dir.Y < 0. then
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
