open System.IO

open Microsoft.AspNetCore.Builder
open Saturn
open Shared

open Fable.Remoting.Giraffe

let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

let personRepository : PersonRepository =
    let persons = [
        { id = 1
          firstName = "Jean"
          lastName = "Durand"
          address =
            { number = 5
              street = "rue de la baguette"
              postalCode = "44000"
              city = "Nantes"
            }
        } ]

    {
        getAll = fun () -> async {
            do! Async.Sleep 500
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