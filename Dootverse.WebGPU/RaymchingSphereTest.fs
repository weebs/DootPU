module Dootverse.WebGPU.RaymchingSphereTest

open Wgsl
open type Wgsl

open NUnit.Framework

let screen = {| width = 800f; height = 600f |}
    
let distanceSphere (sphere: vec3f) (radius: float32) (point: vec3f) : float32 =
    length(sphere - point) - radius

let fragment (output: output) =
    let pixelsPerMeter = 500.0f
    let f = 2.0f
    let r = 1.0f
    let sphere = vec3(0f, 0f, 20.0f)
    let cameraOrigin = vec3(0f, 0f, -2f)
    let x = output.position.x * (screen.width / pixelsPerMeter)
    let y = output.position.y * (screen.height / pixelsPerMeter)
    let mutable point =
        vec3(x, y, f) + cameraOrigin
    let direction = (point - cameraOrigin)
    let mutable distance = distanceSphere sphere r point
    let mutable iteration = 0
    while iteration < 1000 && distance > 0.001f && distance < 1000f do
        distance <- distanceSphere sphere r point
        point <- point + (direction * distance) 
        iteration <- iteration + 1
    if distance <= 0.001f then
        vec4(1f, 1f, 1f, 0f)
    else
        vec4(0f)

[<Test>]
let ``raymarch sphere at origin`` () =
    let output = { position = vec4(0f, 0f, 0f, 1f)
                   xy = vec2(0f, 0f) }
    let result = fragment output
    ()