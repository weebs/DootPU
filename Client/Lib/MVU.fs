module Dootverse.Client.Lib.MVU
open Browser

type AttributeValue = obj
type Element = { tag: string; attributes: Map<string, obj>; nodes: Element list }
type Node =
    | HtmlElement of Element //tag: string * attributes: Map<string, obj> * content: Element list
    | Attribute of string * obj
let rec render (element: Element) =
    let htmlElement = document.createElement element.tag
    for kv in element.attributes do
        htmlElement.setAttribute (kv.Key, string kv.Value)
        for node in element.nodes do
            htmlElement.appendChild (render node)
    htmlElement
    
let rec parse tag (nodes: Node list) : Element =
    let mutable content = []
    let mutable attrs = Map.empty
    for item in nodes do
        match item with
        | Attribute (name, value) -> attrs <- attrs.Add(name, value)
        // | HtmlElement (tag, attributes, nodes) -> content <- content @ [ node ]
        | HtmlElement element -> content <- content @ [ element ]
    {
        tag = tag
        attributes = attrs
        nodes = content
    }
type Html() =
    member _.div (props: Node list) =
        parse "div" props
type Prop() =
    member _.onClick (eventHandler: obj -> unit) =
        Attribute ("onclick", eventHandler)