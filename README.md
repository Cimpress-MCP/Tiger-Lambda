# Tiger.Lambda

## What It Is

Tiger.Lambda is a .NET library for simplifying the configuration and development of AWS Lambda Functions written in C#. It provides a common host allowing for configuration and dependency injection nearly identical to that of ASP.NET Core.

## Why You Want It

Even a non-complicated AWS Lambda Function can quickly gain a tedious amount of setup. An `HttpClient` requires a set of `DelegatingHandlers`, each of which requires its own set of dependencies, some of which are `IOptions<TOptions>`, and didn't Microsoft just release a library to _simplify_ HttpClient?

Tiger.Lambda provides a host very similar to the `WebHost` of ASP.NET Core, allowing the application to be configured in all the ways familiar to an ASP.NET Core developer. The most common actions are exposed as overrideable methods on the Function handler. Even appsettings files are supported.

## Specializations

Currently, the library features a specialization for use in AWS Step Functions. These unavoidably have a form of exceptions-as-flow-control, and any asynchronous Lambda Function would break this by only being able to throw `AggregateException`. The Step Functions specialization manages the async context so that state machines work as intended.

Further specializations are planned, and await implementation experience.

## Thank You

Seriously, though. Thank you for using this software. The author hopes it performs admirably for you.
