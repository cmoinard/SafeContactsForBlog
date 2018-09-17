module Main.Views

open Types

open Fulma

let root (model : Model) (dispatch : Msg -> unit) =
  let subView =
    Persons.Views.containerBox
        model.persons 
        (PersonsMsg >> dispatch)

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
                   [ subView ] ] ] ] ]