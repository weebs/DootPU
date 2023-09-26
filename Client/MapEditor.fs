module Dootverse.MapEditor

open Dootverse.Client.Svg
open Fable.Core
open Feliz
open Fable.Core
open Browser
open Thoth.Json


type Model = {
    selectedMap: string
    selectedWall: (int * int) option
    walls: Map<int * int, byte * byte * byte>
}

and Level = { walls: Map<int * int, byte * byte * byte> }

let getMaps () =
    [|
        for key in JS.Object.keys localStorage do
            if key.StartsWith "maps/" then key
    |]
let initState =
    match Decode.Auto.fromString localStorage["mapeditor/state"] with
    | Ok state -> state
    | _ ->
        {
            selectedMap = ""
            selectedWall = None
            walls = Map.empty
        }
[<ReactComponent>]
let MapEditor () =
    let state, update = React.useStateWithUpdater initState 
    React.useEffect(fun () ->
        localStorage["mapeditor/state"] <- Encode.Auto.toString state 
    , [| box state |])
    let screen = Screen(440, 440)
    Html.div [
        if state.selectedMap = "" then
            Html.button [
                prop.text "Create map"
                prop.onClick (fun _ ->
                    let name = window.prompt "Map name"
                    if name <> null && name.Trim() <> "" then
                        let level = { walls = Map.empty }
                        let mapName = "maps/" + name
                        localStorage[mapName] <- Encode.Auto.toString level
                        update (fun state -> { state with selectedMap = mapName })
                )
            ]
            Html.ul [
                for map in getMaps () do
                    Html.li [
                        prop.children [
                            Html.span [
                                prop.text map
                                prop.onClick (fun _ ->
                                    let walls = Decode.Auto.unsafeFromString<Level> localStorage[map]
                                    update (fun state -> { state with selectedMap = map }))
                            ]
                            Html.button [
                                prop.text "Delete"
                                prop.onClick (fun _ ->
                                    localStorage.removeItem map
                                    update (fun state -> { state with selectedMap = "" }))
                            ]
                        ]
                    ]
            ]
        else
            Html.div [
                Html.button [
                    prop.text "Back"
                    prop.onClick (fun _ -> update (fun state -> { state with selectedMap = "" }))
                ]
                Html.h4 state.selectedMap
                
                Svg.svg [
                    svg.width screen.Width
                    svg.height screen.Height
                    svg.children [
                        for kv in state.walls do
                            let (x, y) = kv.Key
                            let (r, g, b) = kv.Value
                            screen.voxel 20 (x * 20) (y * 20) $"rgb({r}, {g}, {b})"
                        let w = screen.Width / 20
                        let h = screen.Height / 20
                        for x in -w / 2 + 1..w / 2 - 1 do
                            for y in -h / 2 + 1..h / 2 - 1 do
                                let (svgX, svgY) = screen.ToScreen (x * 20, y * 20)
                                Svg.rect [
                                    svg.x (svgX - 20); svg.y (svgY - 20); svg.width 18; svg.height 18
                                    if x = 0 && y = 0 then
                                        svg.fill "blue"
                                    else
                                        svg.fill "grey"
                                    svg.onClick (fun _ -> console.log (x, y))
                                ]
                                Svg.circle [
                                    svg.cx svgX
                                    svg.cy svgY
                                ]
                        Svg.line [
                            svg.x1 0
                            svg.y1 0
                            svg.x2 screen.Width
                            svg.y2 0
                            svg.stroke "black"
                        ] 
                    ]
                ]
            ]
    ]
    
let mapEditor () =
    (document.getElementById "root" |> ReactDOM.createRoot).render(MapEditor ())