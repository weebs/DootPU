// module Wad.Loop
// open System
// open Raylib_cs
//
// let mutable handleInput = fun (s: string) -> ()
//
// let mutable raylibLoop = fun () -> ()
// Threading.Thread(Threading.ThreadStart(fun () ->
// // task {
//     Raylib.InitWindow(800, 400, "Hello, World!")
//     Raylib.SetTargetFPS 120
//     while true do
//         Raylib.BeginDrawing()
//         Raylib.ClearBackground(Color.WHITE)
//         raylibLoop ()
//         Raylib.EndDrawing()
//         Raylib.CloseWindow()
//     // while true do
//     //     Console.ReadLine()|> handleInput
//         
// // } |> ignore
// )).Start()

module State

open Raylib_cs

open System
open ope
open System.Numerics

let mutable pos = 0
let mutable velocity = 1
let mutable callbackFn = fun () ->
    ()
let mutable callback3dFn = fun () ->
        if pos > 800 then
            velocity <- -1
        elif pos < -800 then
            velocity <- 1
        Raylib.DrawCube(Vector3((single pos * 0.5f) * 0.1f, 0f, 0f), 2f, 2f, 2f, Color.YELLOW)
let mutable initFn = fun () -> ()
let mutable theta = 1.57f
let mutable phi = 0f
let loop = env.CreateVar("LOOP")
if loop.Value = "run" then
    loop.Set("stop")
    while loop.Value = "stop" do Threading.Thread.Sleep 100
let mutable c = Camera3D()
let initWindow () =
    loop.Set("run")
    Raylib.InitWindow(800, 400, "raylib")
    Raylib.SetTargetFPS 120
    c.position <- Vector3(10f, 10f, 10f)
    // c.target <- Vector3(0f, 0f, 0f)
    c.up <- Vector3(0f, 1f, 0f)
    c.fovy <- 45f
    c.projection <- CameraProjection.CAMERA_PERSPECTIVE
let threadStart = fun () ->
    initWindow ()
    initFn ()
    while not (Raylib.WindowShouldClose() || loop.Value = "stop") do
        pos <- pos + velocity
        Raylib.BeginDrawing()
        Raylib.ClearBackground(Raylib_cs.Color.WHITE)

        Raylib.BeginMode3D(c)

        callback3dFn ()

        Raylib.EndMode3D()
        
        callbackFn ()

        // Raylib.DrawText("Yo!", pos, pos, 20, Raylib_cs.Color.BLUE)
        // Raylib.DrawText("Yo!", pos / 2, pos / 2, 20, Raylib_cs.Color.BLUE)
        Raylib.EndDrawing()

    Raylib.CloseWindow()
    loop.Set("stopped")
let mutable raylib3dThread = None
let initGame fn =
    match raylib3dThread with
    | Some thread ->
        fn ()
    | None ->
        initFn <- fn
        raylib3dThread <- Some <| thread.start threadStart
        
printfn "Task started"


