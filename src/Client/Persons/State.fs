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
            {
                message = ""
                persons =
                    persons
                    |> List.map (fun p -> { person = p ; isBusy = false })
            }
        | Deleting p ->
            let markPersonAsDeleting ps =
                if ps.person = p then
                    { ps with isBusy = true }
                else
                    ps

            { model with
                persons = 
                    model.persons
                    |> List.map markPersonAsDeleting
            }
        | Deleted (Error _) ->
            { message = "Error while deleting person" ; persons = [] }
        | Deleted (Ok p) ->
            { model with
                persons = 
                    model.persons
                    |> List.filter (fun ps -> ps.person <> p)
            }
    
    let cmd =
        match msg with
        | Loading ->
            Cmd.ofAsync
                Server.api.getAll
                ()
                (Ok >> Loaded)
                (Error >> Loaded)
        | Deleting p ->
            Cmd.ofAsync
                Server.api.delete
                p.id
                (fun _ -> Deleted (Ok p))
                (Error >> Deleted)
        | _ -> Cmd.none
            
    model', cmd