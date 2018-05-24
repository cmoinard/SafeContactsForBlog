module Client

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Shared

open Fulma

open Fulma.FontAwesome

type Model = {
    message: string
    persons: Person list
}

type Msg =
    | Loading
    | Loaded of Result<Person list, exn>

module Server =
    open Fable.Remoting.Client

    let api : PersonRepository =
        Proxy.remoting<PersonRepository> {
            use_route_builder Route.builder
        }

let init () : Model * Cmd<Msg> =
    let model = {
        message = "Loading"
        persons = []
    }
    let cmd =
        Cmd.ofAsync
            Server.api.getAll
            ()
            (Ok >> Loaded)
            (Error >> Loaded)
    model, cmd

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    let model' =
        match msg with
        | Loading ->
            { message = "Loading" ; persons = [] }
        | Loaded (Error _) ->
            { message = "Error while loading persons" ; persons = [] }
        | Loaded (Ok persons) ->
            { message = "" ; persons = persons }
            
    model', Cmd.none

let containerBox (model : Model) (dispatch : Msg -> unit) =
    let content =
        if System.String.IsNullOrEmpty(model.message) |> not then
            model.message
        else
            System.String.Join(
                "\n",
                model.persons
                |> List.map (fun p -> p.firstName + " " + p.lastName))

    Box.box' [ ]
        [ Field.div
            [ Field.IsGrouped ]
            [ Control.p
                [ Control.IsExpanded ]
                [ Input.text
                    [ Input.Disabled true
                      Input.Value content ] ]             
           ]
       ]

let view (model : Model) (dispatch : Msg -> unit) =
  Hero.hero
    [ Hero.IsFullHeight
      Hero.IsBold ]
    [ Hero.body [ ]
        [ Container.container
            [ Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
            [ Columns.columns [ Columns.IsVCentered ]
                [ Column.column
                   [ Column.Width (Screen.All, Column.Is5)
                     Column.Offset (Screen.All, Column.Is1) ]
                   [ containerBox model dispatch ] ] ] ] ]


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
