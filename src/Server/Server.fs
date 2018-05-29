open System.IO

open Microsoft.AspNetCore.Builder
open Saturn

open Bogus

open Shared

open Fable.Remoting.Giraffe

let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

let personRepository =
    let generator =
        Faker<Person>("fr")
            .CustomInstantiator(fun f ->
                { id = f.Random.Number()
                  firstName = f.Name.FirstName()
                  lastName = f.Name.LastName()
                  address = 
                    { number = f.Random.Number(0, 100)
                      street = f.Address.StreetName()
                      postalCode = f.Address.ZipCode()
                      city = f.Address.City()
                    }
                })

    let persons =
        generator.Generate(10)
        |> List.ofSeq

    {
        getAll = fun () -> async {
            do! Async.Sleep 2000
            return persons
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