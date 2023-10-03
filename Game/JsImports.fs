module [<AutoOpen>] Dootverse.Client.JsImports

open Fable.Core

let RAPIER: RAPIER.IExports = JsInterop.importAll "@dimforge/rapier3d-compat"
let three: threejs.IExports = JsInterop.importAll "three"
