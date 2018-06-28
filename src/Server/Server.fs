open System.IO

open Microsoft.AspNetCore.Builder
open Saturn

open Bogus

open Shared

open Fable.Remoting.Giraffe

let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

let personRepository =
    let generate id  =
        Faker<Person>("fr")
            .CustomInstantiator(fun f ->
                { id = id
                  firstName = f.Name.FirstName()
                  lastName = f.Name.LastName()
                  address = 
                    { number = f.Random.Number(0, 100)
                      street = f.Address.StreetName()
                      postalCode = f.Address.ZipCode()
                      city = f.Address.City()
                    }
                })
            .Generate()

    let initialNumberOfPersons = 10

    let mutable persons =
        [1..initialNumberOfPersons]
        |> List.map generate

    {
        getAll = fun () -> async {
            do! Async.Sleep 2000
            return persons
        }

        delete = fun id -> async {
            do! Async.Sleep 2000

            persons <-
                persons
                |> List.filter (fun p -> p.id <> id)
        }
    }

let webApp =
    remoting personRepository {
        use_route_builder Route.builder
    }

let configureApp (app:IApplicationBuilder) =
  app.UseDefaultFiles()

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    router webApp
    app_config configureApp
    memory_cache
    use_static publicPath
    use_gzip
    disable_diagnostics
}

run app