module Raycast.Compute.ImageDisplay

open GLib.Internal
open Gdk
open Gtk
open GLib
open Microsoft.FSharp.NativeInterop
open System

// let label () =
//     let label = new Label()
//     label.SetText "hello"
//     label

let button (label: Label) =
    let button = new Button()
    button.SetLabel "click me"
    let mutable counter = 0

    let clickHnd (_: Button) (_: EventArgs) =
        label.SetText $"hello {counter}"
        counter <- counter + 1

    button.add_OnClicked (new GObject.SignalHandler<Button>(clickHnd))
    button

let box () =
    let box = new Box()
    box.SetOrientation Orientation.Vertical
    box.SetHomogeneous true

    // let l = label ()
    // box.Append l
    // button l |> box.Append
    box

let onActivateApp width height (bytes: byte[]) (sender: Gio.Application) (_: EventArgs) =
    let window = ApplicationWindow.New(sender :?> Application)
    window.Title <- "Gtk4 Window"
    window.SetDefaultSize(300, 300)
    let b = box ()
    let picture = Picture.New()

    // let handle = BytesOwnedHandle.FromUnowned(NativePtr.toNativeInt bytes)
    // let bytes = ByteArray(handle)
    let bytes = Bytes.NewStatic(Span(bytes))
    // let bytes = ByteArray.FreeToBytes bytes

    let t = Gdk.MemoryTexture.New(width, height, Gdk.MemoryFormat.R8g8b8, bytes, unativeint (width * 3))
    // https://stackoverflow.com/questions/74721832/the-easiest-way-of-drawing-an-array-of-pixels-in-gtk-4-0-in-c
    picture.SetPaintable t
    b.Append picture
    window.SetChild(b)
    window.Show()

let showImage width height img =
    let application = Application.New("org.gir.core", Gio.ApplicationFlags.FlagsNone)
    application.add_OnActivate (new GObject.SignalHandler<Gio.Application>(onActivateApp width height img))
    application.RunWithSynchronizationContext(null)
