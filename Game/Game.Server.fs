module Dootverse.Game.Server

open Fable.Core
open Browser
open Dootverse.Client.JsImports
open Dootverse.Client.Game
open Dootverse
open Thoth.Json

let start () = promise {
    do! RAPIER.init ()
    let c = WebSocket.Create "ws://127.0.0.1:8000/ws"
    c.onopen <- fun ev ->
        let server = GameServer(
            RAPIER.World.Create(RAPIER.Vector3.Create(0, -9.81, 0)),
            Network.Scene.createScene ()
        )
        c.onmessage <- fun ev ->
            match Decode.Auto.fromString<Network.LobbyConnection.ServerMessage> (string ev.data) with
            | Ok message ->
                match message with
                | Network.LobbyConnection.ServerMessage.ConnectionRequest (clientId, webRtcRequest) ->
                    promise {
                        let! response = server.AnswerRequest webRtcRequest
                        c.send (Encode.Auto.toString (Network.LobbyConnection.ClientMessage.ConnectionResponse (clientId, response)))
                    } |> ignore
                | _else ->
                    printfn "%A" _else
            | error -> console.log error
            
        c.send (Encode.Auto.toString (Network.LobbyConnection.ClientMessage.HostLobby "w00tcamp"))
}
