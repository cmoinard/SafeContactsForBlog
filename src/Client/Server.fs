module Server

open Fable.Remoting.Client
open Shared

let api : PersonRepository =
    Proxy.remoting<PersonRepository> {
        use_route_builder Route.builder
    }