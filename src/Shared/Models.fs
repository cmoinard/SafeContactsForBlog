namespace Shared

type Address = {
    number: int
    street: string
    postalCode: string
    city: string
}

type Person = {
    id: int
    firstName: string
    lastName: string
    address: Address
}

type PersonRepository = {
    getAll: unit -> Async<Person list>
}