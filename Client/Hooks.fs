module Dootverse.Client.Hooks
open Feliz

let useRefState (state: 'a) =
    let (currentState, setCurrentState) = React.useState state
    let stateRef = React.useRef currentState
    stateRef, (fun state ->
        setCurrentState state
        stateRef.current <- state)

