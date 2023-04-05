# Endpoints.FSharp.Interop

Helper functions for working with Minimal Api in F#.

Provide convenient way to produce `RequestDelegate`.
By depending on `AsParametersAttribute` we can define parameters bag and apply desired metadata attributes.

Supports AspNet Core 7.0+

![example workflow](https://github.com/mchylek/Endpoints.FSharp.Interop/actions/workflows/ci.yml/badge.svg)

## Installation
[![Nuget](https://img.shields.io/nuget/v/Endpoints.FSharp.Interop)](https://www.nuget.org/packages/Endpoints.FSharp.Interop/)

Install the library from [NuGet](https://www.nuget.org/packages/Endpoints.FSharp.Interop):
``` console
❯ dotnet add package Endpoints.FSharp.Interop
```

## Example usage

### Define endpoint that will consume route data.

```fsharp
type HelloRequest = 
    { 
        Logger : ILogger<HelloRequest>
        
        [<FromRoute>]
        Name : string
    }

let hello (req : HelloRequest) = async {
    req.Logger.LogInformation("Hello request handler.")
    return $"Hello {req.Name}!"
}

app.MapGet("/async/{name}", Endpoint.OfAsync hello)
```