namespace Endpoints.FSharp.Interop;

using Microsoft.FSharp.Core;
using Microsoft.FSharp.Control;
using Microsoft.AspNetCore.Http;
public static class Endpoint
{
    /// <summary>
    /// Create delegate compatible with <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder" />.
    /// </summary>
    /// <param name="requestDelegate">The delegate with a structured parameter list marked with <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</param>
    /// <typeparam name="TParam">Type of a structured parameter list. See <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</typeparam>
    /// <typeparam name="TResult">The delegate result type.</typeparam>
    /// <returns>The delegate executed when the endpoint is matched.</returns>
    public static Delegate Of<TParam, TResult>(FSharpFunc<TParam, TResult> requestDelegate)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) => requestDelegate.Invoke(parameters);
    }

    /// <summary>
    /// Create delegate compatible with <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder" />.
    /// </summary>
    /// <param name="requestDelegate">The delegate with a structured parameter list marked with <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</param>
    /// <typeparam name="TParam">Type of a structured parameter list. See <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</typeparam>
    /// <typeparam name="TResult">The delegate result type.</typeparam>
    /// <returns>The delegate executed when the endpoint is matched.</returns>
    public static Delegate OfAsync<TParam, TResult>(FSharpFunc<TParam, FSharpAsync<TResult>> requestDelegate)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) => 
            FSharpAsync.StartAsTask(requestDelegate.Invoke(parameters), null, FSharpOption<CancellationToken>.Some(context.RequestAborted));
    }

    /// <summary>
    /// Create delegate compatible with <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder" />.
    /// </summary>
    /// <param name="factory">The factory of delegate with a structured parameter list marked with <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</param>
    /// <typeparam name="TParam">Type of a structured parameter list. See <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</typeparam>
    /// <typeparam name="TResult">The delegate result type.</typeparam>
    /// <returns>The delegate executed when the endpoint is matched.</returns>
    public static Delegate AsyncFactory<TParam, TResult>(FSharpFunc<HttpContext, FSharpFunc<TParam, FSharpAsync<TResult>>> factory)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) => 
            FSharpAsync.StartAsTask(FSharpFunc<HttpContext, TParam>.InvokeFast(factory, context, parameters), null, FSharpOption<CancellationToken>.Some(context.RequestAborted));
    }

    /// <summary>
    /// Create delegate compatible with <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder" />.
    /// </summary>
    /// <param name="requestDelegate">The delegate with a structured parameter list marked with <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</param>
    /// <typeparam name="TParam">Type of a structured parameter list. See <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</typeparam>
    /// <typeparam name="TResult">The delegate result type.</typeparam>
    /// <returns>The delegate executed when the endpoint is matched.</returns>
    public static Delegate OfTask<TParam, TResult>(FSharpFunc<TParam, FSharpFunc<CancellationToken, Task<TResult>>> requestDelegate)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) =>
            FSharpFunc<TParam, CancellationToken>.InvokeFast(requestDelegate, parameters, context.RequestAborted);
    }

    /// <summary>
    /// Create delegate compatible with <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder" />.
    /// </summary>
    /// <param name="factory">The factory of delegate with a structured parameter list marked with <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</param>
    /// <typeparam name="TParam">Type of a structured parameter list. See <see cref="T:Microsoft.AspNetCore.Http.AsParametersAttribute" />.</typeparam>
    /// <typeparam name="TResult">The delegate result type.</typeparam>
    /// <returns>The delegate executed when the endpoint is matched.</returns>
    public static Delegate TaskFactory<TParam, TResult>(FSharpFunc<HttpContext, FSharpFunc<TParam, FSharpFunc<CancellationToken, Task<TResult>>>> factory)
    {
        return (HttpContext context, [AsParametersAttribute] TParam parameters) =>
            FSharpFunc<HttpContext, TParam>.InvokeFast(factory, context, parameters, context.RequestAborted);
    }
}
