module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Shared

open Fulma

open Fulma.FontAwesome

type Model = Counter option

type Msg =
| Increment
| Decrement
| Init of Result<Counter, exn>


module Server =

  open Shared
  open Fable.Remoting.Client

  /// A proxy you can use to talk to server directly
  let api : ICounterProtocol =
    Proxy.remoting<ICounterProtocol> {
      use_route_builder Route.builder
    }


let init () : Model * Cmd<Msg> =
  let model = None
  let cmd =
    Cmd.ofAsync
      Server.api.getInitCounter
      ()
      (Ok >> Init)
      (Error >> Init)
  model, cmd

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
  let model' =
    match model,  msg with
    | Some x, Increment -> Some (x + 1)
    | Some x, Decrement -> Some (x - 1)
    | None, Init (Ok x) -> Some x
    | _ -> None
  model', Cmd.none

let safeComponents =
  let intersperse sep ls =
    List.foldBack (fun x -> function
      | [] -> [x]
      | xs -> x::sep::xs) ls []

  let components =
    [
      "Saturn", "https://saturnframework.github.io/docs/"
      "Fable", "http://fable.io"
      "Elmish", "https://elmish.github.io/elmish/"
      "Fulma", "https://mangelmaxime.github.io/Fulma"
      "Bulma\u00A0Templates", "https://dansup.github.io/bulma-templates/"
      "Fable.Remoting", "https://zaid-ajaj.github.io/Fable.Remoting/"
    ]
    |> List.map (fun (desc,link) -> a [ Href link ] [ str desc ] )
    |> intersperse (str ", ")
    |> span [ ]

  p [ ]
    [ strong [] [ str "SAFE Template" ]
      str " powered by: "
      components ]

let show = function
| Some x -> string x
| None -> "Loading..."

let navBrand =
  Navbar.Brand.div [ ]
    [ Navbar.Item.a
        [ Navbar.Item.Props
            [ Href "https://safe-stack.github.io/"
              Style [ BackgroundColor "#00d1b2" ] ] ]
        [ img [ Src "https://safe-stack.github.io/images/safe_top.png"
                Alt "Logo" ] ]
      Navbar.burger [ ]
        [ span [ ] [ ]
          span [ ] [ ]
          span [ ] [ ] ] ]

let navMenu =
  Navbar.menu [ ]
    [ Navbar.End.div [ ]
        [ Navbar.Item.a [ ]
            [ str "Home" ]
          Navbar.Item.a [ ]
            [ str "Examples" ]
          Navbar.Item.a [ ]
            [ str "Documentation" ]
          Navbar.Item.div [ ]
            [ Button.a
                [ Button.Size IsSmall
                  Button.Props [ Href "https://github.com/SAFE-Stack/SAFE-template" ] ]
                [ Icon.faIcon [ ]
                    [ Fa.icon Fa.I.Github; Fa.fw ]
                  span [ ] [ str "View Source" ] ] ] ] ]

let containerBox (model : Model) (dispatch : Msg -> unit) =
  Box.box' [ ]
    [ Field.div [ Field.IsGrouped ]
        [ Control.p [ Control.IsExpanded ]
            [ Input.text
                [ Input.Disabled true
                  Input.Value (show model) ] ]
          Control.p [ ]
            [ Button.a
                [ Button.Color IsPrimary
                  Button.OnClick (fun _ -> dispatch Increment) ]
                [ str "+" ] ]
          Control.p [ ]
            [ Button.a
                [ Button.Color IsPrimary
                  Button.OnClick (fun _ -> dispatch Decrement) ]
                [ str "-" ] ] ] ]

let view (model : Model) (dispatch : Msg -> unit) =
  Hero.hero
    [ Hero.IsFullHeight
      Hero.IsBold ]
    [ Hero.head [ ]
        [ Navbar.navbar [  ]
            [ Container.container [ ]
                [ navBrand
                  navMenu ] ] ]
      Hero.body [ ]
        [ Container.container
            [ Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
            [ Columns.columns [ Columns.IsVCentered ]
                [ Column.column
                    [ Column.Width (Screen.All, Column.Is5) ]
                    [ Image.image [ Image.Is4by3 ]
                        [ img [ Src "http://placehold.it/800x600" ] ] ]
                  Column.column
                   [ Column.Width (Screen.All, Column.Is5)
                     Column.Offset (Screen.All, Column.Is1) ]
                   [ Heading.h1 [ Heading.Is2 ]
                       [ str "Superhero Scaffolding" ]
                     Heading.h2
                       [ Heading.IsSubtitle
                         Heading.Is4 ]
                       [ safeComponents ]
                     containerBox model dispatch ] ] ] ]
      Hero.foot [ ]
        [ Container.container [ ]
            [ Tabs.tabs [ Tabs.IsCentered ]
                [ ul [ ]
                    [ li [ ]
                        [ a [ ]
                            [ str "And this at the bottom" ] ] ] ] ] ] ]


#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
