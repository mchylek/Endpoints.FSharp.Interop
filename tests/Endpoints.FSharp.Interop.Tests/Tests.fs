namespace Endpoints.FSharp.Interop.Tests

open FSharpWeb
open Microsoft.AspNetCore.Mvc.Testing
open System
open System.Threading.Tasks
open Xunit


type InteropTest() =
    
    [<Theory>]
    [<InlineData("Endpoint.OfAsync",      "/async/test")>]
    [<InlineData("Endpoint.OfTask",       "/task/test")>]
    [<InlineData("Endpoint.AsyncFactory", "/asyncf/test")>]
    [<InlineData("Endpoint.TaskFactory",  "/taskf/test")>]
    member x.``Test_Invoke`` (method : string, path : string) : Task = task {
        use factory = new WebApplicationFactory<FSharpWeb.Program.App>()
        let client = factory.CreateClient()
        let! result = client.GetStringAsync(path);

        Assert.Equal("Hello test!", result)
    }
        
