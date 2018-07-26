module Persons.State

open Elmish

open Shared
open Types

let init () : Model * Cmd<Msg> =
    let model = {
        message = "Loading"
        persons = []
    }
    model, Cmd.ofMsg Loading

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    let model' =
        match msg with
        | Loading ->
            { message = "Loading" ; persons = [] }
        | Loaded (Error _) ->
            { message = "Error while loading persons" ; persons = [] }
        | Loaded (Ok persons) ->
            { message = "" ; persons = persons }
        | _ ->
            model
    
    let cmd =
        match msg with
        | Loading ->
            Cmd.ofAsync
                Server.api.getAll
                ()
                (Ok >> Loaded)
                (Error >> Loaded)
        | Delete p ->
            Cmd.ofAsync
                Server.api.delete
                p.id
                (fun _ -> Loading)
                (Error >> Loaded)
        | _ -> Cmd.none
            
    model', cmd