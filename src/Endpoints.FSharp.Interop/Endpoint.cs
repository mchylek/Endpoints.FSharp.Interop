namespace Endpoints.FSharp.Interop;

using Microsoft.FSharp.Core;
using Microsoft.FSharp.Control;
using Microsoft.AspNetCore.Http;
public static class Endpoint
{
    public static Delegate OfAsync<TParam, TResult>(FSharpFunc<TParam, FSharpAsync<TResult>> action)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) => 
            FSharpAsync.StartAsTask(action.Invoke(parameters), null, FSharpOption<CancellationToken>.Some(context.RequestAborted));
    }

    public static Delegate AsyncFactory<TParam, TResult>(FSharpFunc<HttpContext, FSharpFunc<TParam, FSharpAsync<TResult>>> action)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) => 
            FSharpAsync.StartAsTask(FSharpFunc<HttpContext, TParam>.InvokeFast(action, context, parameters), null, FSharpOption<CancellationToken>.Some(context.RequestAborted));
    }

    public static Delegate OfTask<TParam, TResult>(FSharpFunc<TParam, FSharpFunc<CancellationToken, Task<TResult>>> action)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) =>
            FSharpFunc<TParam, CancellationToken>.InvokeFast(action, parameters, context.RequestAborted);
    }

    public static Delegate TaskFactory<TParam, TResult>(FSharpFunc<HttpContext, FSharpFunc<TParam, FSharpFunc<CancellationToken, Task<TResult>>>> action)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) =>
            FSharpFunc<HttpContext, TParam>.InvokeFast(action, context, parameters, context.RequestAborted);
    }
}
