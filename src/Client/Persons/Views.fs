module Persons.Views

open Types

open Fulma

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

let root (model : Model) (dispatch : Msg -> unit) =
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