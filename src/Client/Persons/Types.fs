module Persons.Types

open Shared

type PersonWithState = {
    person: Person
    isBusy: bool
}

type Model = {
    message: string
    persons: PersonWithState list
}

type Msg =
    | Loading
    | Loaded of Result<Person list, exn>
    | Deleting of Person
    | Deleted of Result<Person, exn>