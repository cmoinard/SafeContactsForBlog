module Persons.State

open Elmish

open Shared
open Types

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