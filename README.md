# Tiger.Lambda

## What It Is

Tiger.Lambda is a .NET library for simplifying the configuration and development of AWS Lambda Functions written in C#. It provides a common host allowing for configuration and dependency injection nearly identical to that of ASP.NET Core.

## Why You Want It

Even a non-complicated AWS Lambda Function can quickly gain a tedious amount of setup.
An `HttpClient` requires a set of `DelegatingHandler`s,
each of which requires its own set of dependencies,
some of which are `IOptions<TOptions>`,
and didn't Microsoft just release a library to _simplify_ HttpClient?

Tiger.Lambda provides a host very similar to the `WebHost` of ASP.NET Core,
allowing the application to be configured in all the ways familiar to an ASP.NET Core developer.
The most common actions are exposed as overrideable methods on the Function handler.
Even appsettings files are supported.

## How To Use It

The concept of a function handler is exposed to dependency injection as a type `IHandler<TIn, TOut>`.
This handler's `Task<TOut> HandleAsync(TIn,ILambdaContext,CancellationToken)` method will be called
after asking DI for an instance satisfying `IHandler<TIn, TOut>`.
The following _extremely simplified_ example illustrates a basic setup.

```csharp
namespace Example
{
  public sealed class PerformAction
    : IHandler<string, int>
  {
    readonly IValueGetter _valueGetter;

    public Handler(IValueGetter valueGetter)
    {
      _valueGetter = valueGetter;
    }

    public async Task<int> HandleAsync(string input, ILambdaContext context, CancellationToken cancellationToken = default)
    {
      var value = await _valueGetter
        .GetValue(input, cancellationToken)
        .ConfigureAwait(false);

      return value;
    }

    // This may of course exist in another file, if desired.
    public sealed class EntryPoint
      : EntryPoint<string, int>
    {
      public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
      {
        base.ConfigureServices(context, services);
        _ = services.AddSingleton<IValueGetter, HttpValueGetter>();
      }
    }
  }
}
```

The lambda's entry point is the `Task<TOut> HandleAsync(TIn,ILambdaContext)` method of the entry point class inheriting from `EntryPoint<TIn, TOut>`.
This can be set in Lambda configuration in the usual fashion.
For the example above, it would resemble "Documentation::Example.PerformAction+EntryPoint::HandleAsync".

## Thank You

Seriously, though. Thank you for using this software. The author hopes it performs admirably for you.
