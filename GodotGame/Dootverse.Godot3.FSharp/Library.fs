namespace Dootverse.Godot3.FSharp

open Godot

type Class1() =
    inherit Godot.Spatial()
    let server = new WebSocketServer()
    override this._Ready() =
        GD.Print "hi!"