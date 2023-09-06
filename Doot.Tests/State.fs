module Doot.Tests.State

open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Threading
open Avalonia
open Avalonia.Controls
// open Avalonia.Themes.Fluent
open Avalonia.Themes.Fluent
open Avalonia.Threading
open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.DSL
open Avalonia.Controls.ApplicationLifetimes
open NUnit.Framework

let mutable update: obj -> obj -> obj * Cmd<obj> = fun _ _ -> null, []
let mutable view: obj -> (obj -> unit) -> IView = fun _ _ -> StackPanel.create []
let mutable setState: obj -> unit = fun _ -> ()
let mutable updateView: obj -> unit = fun _ -> ()
let setView<'t, 'msg> viewFn =
    view <- (fun (model: obj) (dispatch: obj -> unit) ->
        let model = model :?> 't
        let dispatch = fun (msg: 'msg) ->
            dispatch (box msg)
        viewFn model dispatch
    )
    // updateView (box viewFn)
let setUpdate<'t, 'msg> (updateFn: 'msg -> 't -> 't * Cmd<'msg>) =
    update <- (fun (msg: obj) (model: obj) ->
        let model = model :?> 't
        let msg = msg :?> 'msg
        let model, cmd = updateFn msg model
        model, cmd |> Cmd.map box)
let setProgram<'t, 'msg, 'view when 'view :> IView> (state: 't) (update: 'msg -> 't -> 't * Cmd<'msg>) (view: 't -> Dispatch<'msg> -> 'view) =
    setUpdate update
    setView (fun model dispatch -> (view model dispatch) :> IView)
    // updateView (box view)
    setState (box state)
    
    
type Msg =
    | SetState of obj
    | UpdatedView of obj
    | ViewMsg of obj
type Model = { state: obj; view: obj }    
type MainWindow() as this =
    inherit HostWindow()
    let _init () : Model * Cmd<_> = { state = 0; view = null }, Cmd.ofEffect (fun dispatch ->
        updateView <- fun view ->
            Dispatcher.UIThread.Invoke(fun () -> dispatch (UpdatedView view))
        setState <- fun state ->
            Dispatcher.UIThread.Invoke(fun () -> dispatch (SetState state)))
    let _update msg (model: Model) =
        match msg with
        | SetState value ->
            { model with state = value }, []
        | ViewMsg msg ->
            update msg model.state
            |> fun (state, cmds) ->
                { model with state = state }, cmds |> Cmd.map ViewMsg
        | UpdatedView view ->
            { model with view = view }, []
    let _view model dispatch =
        view model.state (dispatch << ViewMsg)
        // StackPanel.create [
        //     StackPanel.children [
        //         TextBlock.create [
        //             TextBlock.fontSize 40.0
        //             TextBlock.text "hello"
        //         ]
        //     ]
        // ]
    do
        base.Title <- "Counter Example"
        // base.Icon <- WindowIcon(System.IO.Path.Combine("Assets","Icons", "icon.ico"))
        base.Height <- 400.0
        base.Width <- 400.0

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        Elmish.Program.mkProgram _init _update _view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run

let mutable window = None
let open_window () =
    Dispatcher.UIThread.Invoke(fun () ->
        window <- Some <| MainWindow()
        window.Value.Show()
    )
