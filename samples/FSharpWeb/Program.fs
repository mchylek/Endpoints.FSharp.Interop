module FSharpWeb

open Endpoints.FSharp.Interop
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open System.Threading

module Program = 
    
    type HelloRequest = 
        { 
            Logger : ILogger<HelloRequest>
            [<FromRoute>] Name : string
        }

    let hello (req : HelloRequest) =
        req.Logger.LogInformation("Hello request handler.")
        $"Hello {req.Name}!"

    let helloA (req : HelloRequest) = async {
        req.Logger.LogInformation("Hello request handler.")
        return $"Hello {req.Name}!"
    }

    let helloT (req : HelloRequest) (cancellationToken : CancellationToken) = task {
        cancellationToken.ThrowIfCancellationRequested()
        req.Logger.LogInformation("Hello request handler.")
        return $"Hello {req.Name}!"
    }



    type HelloModel = { [<FromRoute>] Name : string }

    let helloF (logger : ILogger<HelloModel>) (model : HelloModel) = async {
        logger.LogInformation("Hello request handler.")
        return $"Hello {model.Name}!"
    }

    let helloFT (logger : ILogger<HelloModel>) (model : HelloModel) (cancellationToken : CancellationToken) = task {
        cancellationToken.ThrowIfCancellationRequested()
        logger.LogInformation("Hello request handler.")
        return $"Hello {model.Name}!"
    }

    type App () =

        static member Main (args : string[]) =

            let builder = WebApplication.CreateBuilder()
            let app = builder.Build()
            app.UseRouting() |> ignore

            app.MapGet("/sync/{name}", Endpoint.Of hello)
            |> ignore

            app.MapGet("/async/{name}", Endpoint.OfAsync helloA)
            |> ignore

            app.MapGet("/task/{name}", Endpoint.OfTask helloT)
            |> ignore

            app.MapGet("/asyncf/{name}"
                       , Endpoint.AsyncFactory (fun ctx -> helloF <| ctx.RequestServices.GetRequiredService()))
            |> ignore

            app.MapGet("/taskf/{name}"
                       , Endpoint.TaskFactory (fun ctx -> helloFT <| ctx.RequestServices.GetRequiredService()))
            |> ignore

            app.MapGet("/anonymous/{name}", Endpoint.Of (fun (req : {| Name : string |} ) -> $"Hello {req.Name}!"))
            |> ignore

            app.Run()
            0

    let [<EntryPoint>] main args = App.Main args
