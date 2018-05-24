module Persons.Types

open Shared

type Model = {
    message: string
    persons: Person list
}

type Msg =
    | Loading
    | Loaded of Result<Person list, exn>