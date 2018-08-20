module Persons.Types

open Shared
open AsyncResult

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
    | Load of AsyncResult<unit, Person list>
    | Delete of AsyncResult<Person, Person>