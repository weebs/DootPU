module Dootverse.RTC.JS

open System.Collections.Generic
open Dootverse
open Browser
open Fable.Core
open Thoth.Json
open Fable.Core.JsInterop

let createConnection () : JS.Promise<Types.RTCPeerConnection * Types.RTCDataChannel * Network.WebRtcRequest> = 
    promise {
        let c = WebRTC.RTCPeerConnection.Create(toPlainJsObj {| iceServers = [| {| urls = "stun:stun.l.google.com:19302" |} |] |} :?> _)
        let mutable offer = Unchecked.defaultof<_>
        let mutable candidates = []
        c.onicecandidate <- fun ev ->
            match ev.candidate with
            | Some candidate when candidate.candidate <> "" ->
                candidates <- (candidate.candidate.Trim(), candidate.sdpMid) :: candidates
            | _ -> ()
            
        let channel = c.createDataChannel("data")
            
        let! o = c.createOffer()
        offer <- o
        
        let mutable channelOpen = false
        
        do! c.setLocalDescription offer
        let mutable request: Network.WebRtcRequest option = None
        
        c.ondatachannel <- fun ev ->
            channelOpen <- true
            
        c.onicegatheringstatechange <- fun ev -> 
            promise {
                if ev.target?iceGatheringState = "complete" then
                    request <- Some {
                        Offer = offer.sdp
                        Candidates = Array.ofList candidates 
                    }
            } |> ignore

        while request.IsNone do
            do! Promise.sleep 50
           
        // todo
        // let answer = Decode.Auto.unsafeFromString<Network.WebRtcResponse> response
        // do! c.setRemoteDescription(toPlainJsObj {| ``type`` = "answer"; sdp = answer.Answer |} :?> _)
        // for (candidate, mid) in answer.Candidates do
        //     do! c.addIceCandidate(toPlainJsObj {| candidate = candidate; sdpMid = mid |} :?> _)
        return (c, channel, request.Value)
    }
let answerRequest (request: Network.WebRtcRequest) : JS.Promise<Types.RTCPeerConnection * Types.RTCDataChannel * Network.WebRtcResponse> = promise {
    let connection =
        WebRTC.RTCPeerConnection.Create(toPlainJsObj {|
            iceServers = [| {| urls = "stun:stun.l.google.com:19302" |} |] |} :?> _)
    let mutable answer = ""
    let mutable finished = false
    let mutable candidates = []
    connection.onicegatheringstatechange <- fun ev ->
        if ev.target?iceGatheringState = "complete" then
            finished <- true
        // console.log ev
    // connection.onconnectionstatechange <- fun ev -> console.log ev
    connection.onicecandidate <- fun ev ->
        // console.log ev
        match ev.candidate with
        | Some candidate when candidate.candidate <> "" ->
            candidates <- (candidate.candidate.Trim(), candidate.sdpMid) :: candidates
        | _ -> ()
    let dataChannel = connection.createDataChannel "data"
    do! connection.setRemoteDescription (RTCSessionDescriptionInit.Create(Types.RTCSdpType.Offer, request.Offer))
    let! answer = connection.createAnswer()
    do! connection.setLocalDescription answer
    for (c, sdpMid) in request.Candidates do
        do! connection.addIceCandidate (toPlainJsObj {| candidate = c; sdpMid = sdpMid |} :?> _)
    while finished = false do
        do! Promise.sleep 10
    return connection, dataChannel , {
        Answer = answer.sdp
        Candidates = Array.ofList candidates
    }
}
