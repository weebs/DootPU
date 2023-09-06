module Doot.Tests.UITest
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

module Counter =
    open Avalonia.Controls
    open Avalonia.Layout
    
    type State = { count : int }
    let init() = { count = 0 }, []

    type Msg =
    | Increment
    | Decrement
    | SetCount of int
    | Reset 

    let update (msg: Msg) (state: State) =
        match msg with
        | Increment -> { state with count = state.count + 1 }, []
        | Decrement -> { state with count = state.count - 1 }, []
        | SetCount count  -> { state with count = count }, []
        | Reset -> init ()
    
    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.children [
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Reset)
                    Button.content "reset"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                ]                
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Decrement)
                    Button.content "-"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Increment)
                    Button.content "+"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick ((fun _ -> state.count * 2 |> SetCount |> dispatch), SubPatchOptions.OnChangeOf state.count)
                    Button.content "x2"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                ]
                TextBox.create [
                    TextBox.dock Dock.Bottom
                    TextBox.onTextChanged ((fun text ->
                        let isNumber, number = System.Int32.TryParse text
                        if isNumber then
                            number |> SetCount |> dispatch) 
                    )
                    TextBox.text (string state.count)
                    TextBox.horizontalAlignment HorizontalAlignment.Stretch
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                ]
            ]
        ]

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        // base.Icon <- WindowIcon(System.IO.Path.Combine("Assets","Icons", "icon.ico"))
        base.Height <- 400.0
        base.Width <- 400.0

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        Elmish.Program.mkProgram Counter.init Counter.update Counter.view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run

type Msg = ButtonClicked
[<Test>]
let ``run window `` () =
    let openWindow = false
    
    if openWindow then
        State.open_window ()
    else
        let update = (fun msg (model: int) ->
            printfn "%A %A" msg model
            printfn "howdy doot!!"
            model + 1, []
        )
        let view = (fun (model: int) dispatch ->
            StackPanel.create [
                StackPanel.children [
                    Button.create [
                        Button.onClick (fun _ -> dispatch ButtonClicked)
                        Button.content ("1234! " + string model)
                    ]
                    Canvas.create [
                        Canvas.children [
                            Rectangle.create [
                            ]
                        ]
                    ]
                ]
            ]
        )
        State.setProgram 0 update view
        // State.setState "1234"
        