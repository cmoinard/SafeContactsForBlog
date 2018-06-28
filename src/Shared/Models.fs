namespace Shared

type Address = {
    number: int
    street: string
    postalCode: string
    city: string
}

module Address =
    let toString address =
        string address.number
        + " " + address.street
        + " " + address.postalCode
        + " " + address.city

type Person = {
    id: int
    firstName: string
    lastName: string
    address: Address
}

type PersonRepository = {
    getAll: unit -> Async<Person list>
    delete: int -> Async<unit>
}