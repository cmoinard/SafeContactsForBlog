module Persons.State

open Elmish

open AsyncResult
open Shared
open Types
open Persons.Types
open Persons.Types

let init () : Model * Cmd<Msg> =
    let model = {
        message = "Loading"
        persons = []
    }
    model, Cmd.ofMsg <| Load (InProgress ())


let updateLoadModel result =
    match result with
    | InProgress _ ->
        { message = "Loading" ; persons = [] }
    | Error _ ->
        { message = "Error while loading persons" ; persons = [] }
    | Done persons ->   
        {
            message = ""
            persons =
                persons
                |> List.map PersonWithState.create
        }

let updateDeleteModel model result =
    match result with
    | InProgress p ->
        { model with
            persons = 
                model.persons
                |> List.map (PersonWithState.markAsBusy p)
        }

    | Error _ ->
        { message = "Error while deleting person" ; persons = [] }
    
    | Done p ->
        { model with
            persons = 
                model.persons
                |> List.filter (fun ps -> ps.person <> p)
        }


let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    let model' =
        match msg with
        | Load r ->
            updateLoadModel r
        
        | Delete r ->
            updateDeleteModel model r
            
    let cmd =
        match msg with
        | Load (InProgress _) ->
            AsyncResult.ofAsyncCmd
                Server.api.getAll
                ()
                Load

        | Delete (InProgress p) ->
            AsyncResult.ofAsyncCmdWithMap
                Server.api.delete
                p
                Delete
                Person.getId

        | _ -> Cmd.none
            
    model', cmd