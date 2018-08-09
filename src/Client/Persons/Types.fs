module Persons.Types

open Shared

type PersonWithState = {
    person: Person
    isBusy: bool
}

module PersonWithState =
    let create p = { person = p ; isBusy = false }
    
    let markAsBusy p ps =
        if ps.person = p then
            { ps with isBusy = true }
        else
            ps

type Model = {
    message: string
    persons: PersonWithState list
}

type Msg =
    | Loading
    | Loaded of Result<Person list, exn>
    | Deleting of Person
    | Deleted of Result<Person, exn>