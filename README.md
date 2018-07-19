# Tiger.Lambda

## What It Is

Tiger.Lambda is a .NET library for simplifying the configuration and development of AWS Lambda Functions written in C#. It provides a common host allowing for configuration and dependency injection nearly identical to that of ASP.NET Core.

## Why You Want It

Even a non-complicated AWS Lambda Function can quickly gain a tedious amount of setup. An `HttpClient` requires a set of `DelegatingHandlers`, each of which requires its own set of dependencies, some of which are `IOptions<TOptions>`, and didn't Microsoft just release a library to _simplify_ HttpClient?

Tiger.Lambda provides a host very similar to the `WebHost` of ASP.NET Core, allowing the application to be configured in all the ways familiar to an ASP.NET Core developer. The most common actions are exposed as overrideable methods on the Function handler. Even appsettings files are supported.

## How You Develop It

This project is using the standard [`dotnet`] build tool. A brief primer:

[`dotnet`]: https://dot.net

- Restore NuGet dependencies: `dotnet restore`
- Build the entire solution: `dotnet build`
- Run all unit tests: `dotnet test`
- Pack for publishing: `dotnet pack -o "$(pwd)/dist"`

The parameter `--configuration` (shortname `-c`) can be supplied to the `build`, `test`, and `pack` steps with the following meaningful values:

- “Debug” (the default)
- “Release”

This repository is attempting to use the [GitFlow] branching methodology. Results may be mixed, please be aware.

[GitFlow]: http://jeffkreeftmeijer.com/2010/why-arent-you-using-git-flow/

## Thank You

Seriously, though. Thank you for using this software. The author hopes it performs admirably for you.
