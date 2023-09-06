module Doot.Tests.PGA

open System
open System.Diagnostics
open NUnit.Framework
open PGA

[<SetUp>]
let Setup () =
    ()

let distance (a: PGA3D) (b: PGA3D) =
    // let a = a.normalized()
    // let b = b.normalized()
    let result =
        let _a = (a.X - b.X) ** 2f
        let _b = ((a.Z - b.Z) ** 2f)
        // printfn "a = %A b = %A" _a _b
        MathF.Sqrt(_a + _b)
    // printfn "%A" a
    // printfn "distance %A to %A = %A" a.ToPoint b.ToPoint result
    result
let isOnLine (point_a: PGA3D) point_b point =
    let length = distance point_a point_b
    // printfn "line length = %A" length
    let dist_a = distance point_a point
    // printfn "dist a = %A" dist_a
    dist_a <= length && (distance point_b point) <= length
[<Test>]
let ``intersection of two lines produces a euclidean point`` () =
    let point_a = PGA3D.point(-100f, 0f, 0f)
    let point_b = PGA3D.point(-50f, 0f, 0f)
    let line_a = point_a &&& point_b
    
    let point_a2 = PGA3D.point(0f, 0f, 200f)
    let lineTwoEnd = PGA3D.point(0f, 0f, 000f)
    
    let line_b = point_a2 &&& lineTwoEnd
    let plane = line_b &&& PGA3D.point(0f, 1f, 0f)
    // let plane = PGA3D.point(0f, 1f, 0f) &&& PGA3D.point(0f, 0f, 0f) &&& PGA3D.point(1f, 0f, 0f)
    let intersectionOfLines = line_a ^^^ plane
    let point = intersectionOfLines.normalized()
    
    printfn "%A" point_a.ToPoint
    printfn "%A" point_b.ToPoint
    printfn "line_a: %A" line_a
    printfn "line_b: %A" line_b
    printfn "plane = %A" plane
    printfn "point = %A" intersectionOfLines
    printfn "%A" intersectionOfLines.ToPoint
    printfn "%A" (intersectionOfLines.normalized()).ToPoint
    printfn "intersects = %A" (isOnLine point_a2 lineTwoEnd point)
    Assert.Pass()
open type PGA3D    
[<Test>]
let ``test 640 raycasts with a single line`` () =
    let sw = Stopwatch()
    let up = point(0f, 1f, 0f)
    // let line_a = point(100f, 0f, 50f)
    // let line_b = point(0f, 0f, 150f)
    let line_a = point(10f, 0f, 10f)
    let line_b = point(-0f, 0f, 10f)
    let line = line_a &&& line_b
    let lineAsPlane = line &&& up
    
    let cameraEye = point(0f, 0f, -1f)
    let cameraPlaneOrigin = point(0f, 0f, 0f)
    let cameraPlaneUp = point(0f, 1f, 0f)
    let cameraPlaneRight = point(1f, 0f, 0f)
    let cameraPlane = cameraPlaneOrigin &&& cameraPlaneUp &&& cameraPlaneRight
    
    
    let screenWidth = 7000
    sw.Start()
    let mutable count = 0
    // let intersections = [|
    for i in -screenWidth / 2..screenWidth / 2 - 1 do
        let planePoint = point(float32 i, 0f, 0f)
        let rayDir = cameraEye &&& planePoint
        let intersectionWithLine = lineAsPlane ^^^ rayDir
        if isOnLine line_a line_b intersectionWithLine then
            count <- count + 1
            // yield intersectionWithLine
    // |]
    sw.Stop()
    printfn $"done, intersections = {count}"
    printfn $"{screenWidth} casts took {sw.ElapsedMilliseconds} milliseconds"