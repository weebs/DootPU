namespace Dootverse

open System
open System.Collections.Generic
open Browser.Types
open Browser
open Fable.Core

open Dootverse.Models
open Doot.Maths.Voxel.Traversal
open Dootverse
open PGA

module Keys =
    let mutable debugKeys = false
    let pressed = Dictionary<string, bool>()
    let justPressed = Dictionary<string, bool>()
    let isPressed (key: string) =
        let key = key.ToLower()
        if pressed.ContainsKey key then pressed[key] else false
    let isJustPressed key =
        if justPressed.ContainsKey key then justPressed[key] else false
    window.onkeydown <- fun key ->
        if debugKeys then
            console.log key
        let c = key.key.ToLower()
        // if c = "escape" then
        //     key.preventDefault()
        if c = "tab" then key.preventDefault()
        if not <| isPressed c then
            justPressed[c] <- true
        pressed[c] <- true
    window.onkeyup <- fun key ->
        pressed[key.key.ToLower()] <- false
    window.onblur <- fun _ ->
        for kv in pressed do
            pressed[kv.Key.ToLower()] <- false
            
module Engine =
    let mutable mouseX = 0
    let mutable mouseY = 0
    let mutable mouse1 = false
    document.body.onmousemove <- fun ev ->
        if document.pointerLockElement <> null then
            mouseX <- mouseX + int ev.movementX
            mouseY <- mouseY + int ev.movementY
        else
            mouseX <- 0
            mouseY <- 0
    document.body.onclick <- fun ev ->
        mouse1 <- true
        
    let mutable pointerState = document.pointerLockElement = null
    document.onpointerlockchange <-
        fun ev ->
            console.log ("element = ", document.pointerLockElement)
            pointerState <- document.pointerLockElement = null
            console.log ev
    let endInputFrame () =
        mouseX <- 0
        mouseY <- 0
        mouse1 <- false
        for kv in Keys.justPressed do
            Keys.justPressed[kv.Key] <- false
type Game() =
    class end

