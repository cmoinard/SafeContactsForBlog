module Persons.Views

open Types

open Fulma
open Shared

open Fable.Helpers.React

let personHeader =
    tr  []
        [ th [] [ str "Id" ]
          th [] [ str "First name" ]
          th [] [ str "Last name" ]
          th [] [ str "Address" ]
          th [] []
        ]

let personLine dispatch ps =
    let p = ps.person
    tr  []
        [ td [] [ p.id |> string |> str ]
          td [] [ str p.firstName ]
          td [] [ str p.lastName ]
          td [] [ str (Address.toString p.address) ]
          td [] [ 
            Button.a 
                [ Button.IsLoading ps.isBusy
                  Button.OnClick (fun _ -> dispatch (Deleting p)) ]
                [ str "delete"] ]
        ]

let personsTable dispatch persons =
    let lines =
        persons
        |> List.map (personLine dispatch)

    Table.table [ Table.IsHoverable ]
        [ thead [] [ personHeader ]
          tbody [] lines
        ]

let containerBox (model : Model) (dispatch : Msg -> unit) =
    let content =
        if System.String.IsNullOrEmpty(model.message) |> not then
            str model.message
        else
            personsTable dispatch model.persons

    Box.box' [ ]
        [ Field.div
            [ Field.IsGrouped ]
            [ content ] ]

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